using UnityEngine;

/// <summary>
/// Handles all the verticality-related control mechanics, including 
/// <list type="bullet">
///     <item> GROUND CHECK </item>
///     <item> JUMP VELOCITY (not physics), and </item>
///     <item> RAYCAST DRAWING (editor only). </item>
/// </list>
/// 
/// Notes: UPWARDS vertical physics is handled by <see cref="PlayerMotor"/>, <br/>
/// while the DOWNWARDS is handled by <see cref="GravityHandler"/> (which implements <see cref="IGravityAffected"/>).
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(GravityHandler))]
public class PlayerVerticalityHandler : MonoBehaviour, IGravityAffected
{
    /// <remarks> Taken from <see cref="PlayerMotor"/>. </remarks>
    private PlayerMoveConfig moveConfig;
    /// <remarks> Taken from <see cref="GravityHandler"/>. </remarks>
    private PhysicsConfig worldPhysicsConfig;

    [SerializeField] private LayerMask groundLayerMask;

    private CapsuleCollider playerCollider;
    private IMotor motor;
    private PlayerInputState inputState;

    // IGravityAffected
    public bool IsGrounded { get; private set; } 
    public bool BypassGravityApplication { get; private set; }
    public float TargetVelocityY
    {
        get => motor.GetTargetVelocityY();
        set => motor.SetTargetVelocityY(value);
    }

    private void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        motor          = GetComponent<PlayerMotor>();

        worldPhysicsConfig = GetComponent<GravityHandler>().worldPhysicsConfig;
        moveConfig = GetComponent<PlayerMotor>().moveConfig;
    }

    /// <summary> Listens to input changes from <see cref="PlayerInputHandler"/> and stores it locally. </summary>
    public void UpdateInputState(PlayerInputState playerInputState) => inputState = playerInputState;

    private void FixedUpdate()
    {
        UpdateIsGrounded();
        if (IsGrounded && inputState.IsPressingJump) Jump();
    }

    #region GROUND CHECK
    private void UpdateIsGrounded()
    {
        Vector3 rayOrigin = GetRayOrigin();
        IsGrounded = CastGroundCone(rayOrigin, out RaycastHit bestHit);
    }

    private Vector3 GetRayOrigin()
    {
        Vector3 bottom = transform.position
            + playerCollider.center
            + Vector3.down * (playerCollider.height / 2);

        return bottom + Vector3.up * moveConfig.groundCheckRayOriginOffset.y;
    }

    /// <summary>
    /// Creates 5 ground check rays starting just before the player's feet. <br/>
    /// One exactly downwards, plus four angled towards the cardinal directions.
    /// </summary>
    private bool CastGroundCone(Vector3 origin, out RaycastHit closestHit)
    {
        closestHit  = default;
        float closestDist = float.MaxValue;
        bool anyHit = false;

        float outwardRadius = playerCollider.radius + moveConfig.groundCheckRayRadius;
        float coneRad       = moveConfig.groundCheckRayConeDegree * Mathf.Deg2Rad;
        float sinAngle      = Mathf.Sin(coneRad);
        float cosAngle      = Mathf.Cos(coneRad);

        Vector3[] cardinals = {
            transform.forward, -transform.forward,
            transform.right,   -transform.right
        };

        foreach (Vector3 lateral in cardinals)
        {
            Vector3 coneOrigin    = origin + lateral * outwardRadius;
            Vector3 coneDirection = (lateral * sinAngle + Vector3.down * cosAngle).normalized;
            TryRay(coneOrigin, coneDirection, moveConfig.groundCheckRayLength, ref closestHit, ref closestDist, ref anyHit);
        }

        TryRay(origin, Vector3.down, moveConfig.groundCheckRayLength, ref closestHit, ref closestDist, ref anyHit);

        return anyHit;
    }

    /// <summary> Helper for the built-in raycast function. </summary>
    private void TryRay(
        Vector3 origin, Vector3 direction, float length,
        ref RaycastHit closestHit, ref float closestDist, ref bool anyHit)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hit, length, groundLayerMask))
        {
            if (hit.distance < closestDist)
            {
                closestDist = hit.distance;
                closestHit  = hit;
            }
            anyHit = true;
        }
    }
    #endregion

    #region JUMP
    private void Jump()
    {
        IsGrounded = false;
        float height = GetJumpHeight();
        float targetVelocity = Mathf.Sqrt(2f * height * Mathf.Abs(worldPhysicsConfig.gravityUpwards));
        TargetVelocityY = targetVelocity;
    }

    /// <returns> Input state-dependent jump height. </returns>
    private float GetJumpHeight()
    {
        bool isCrouching = inputState.IsPressingCrouch;
        bool isSprinting = inputState.IsPressingSprint && inputState.MoveInput.y > 0;

        if (isCrouching) return moveConfig.jumpHeight * moveConfig.crouchJumpMultiplier;
        if (isSprinting) return moveConfig.jumpHeight * moveConfig.sprintJumpMultiplier;
        return moveConfig.jumpHeight;
    }
    #endregion

    #region UNITY EDITOR 
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (playerCollider == null) return;

        Vector3 origin = GetRayOrigin();

        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(origin, Vector3.down * moveConfig.groundCheckRayLength);

        float outwardRadius = playerCollider.radius + moveConfig.groundCheckRayRadius;
        float coneRad       = moveConfig.groundCheckRayConeDegree * Mathf.Deg2Rad;
        float sinAngle      = Mathf.Sin(coneRad);
        float cosAngle      = Mathf.Cos(coneRad);

        Vector3[] cardinals = {
            transform.forward, -transform.forward,
            transform.right,   -transform.right
        };

        foreach (Vector3 lateral in cardinals)
        {
            Vector3 coneOrigin    = origin + lateral * outwardRadius;
            Vector3 coneDirection = (lateral * sinAngle + Vector3.down * cosAngle).normalized;
            Gizmos.DrawRay(coneOrigin, coneDirection * moveConfig.groundCheckRayLength);
        }
    }
    #endif
    #endregion
}