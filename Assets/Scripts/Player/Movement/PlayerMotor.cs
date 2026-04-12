using UnityEngine;

/// <summary>
/// Overrides <see cref="RigidbodyMotor.ApplyVelocity"/> to apply horizontal input smoothing
/// via <see cref="Vector3.MoveTowards"/>. <br/>
/// Note: Vertical velocity (gravity, jump) is always written directly--no smoothing--since PlayerVerticalityHandler handles this. <br/><br/>
/// Related: <seealso cref="PlayerMoveConfig"/>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : RigidbodyMotor
{
    /// <remarks> Accessed by this script and PlayerVerticalityHandler </remarks>
    public PlayerMoveConfig moveConfig;

    /// <summary> Tracks the current smoothed horizontal velocity toward the target. </summary>
    private Vector3 smoothedHorizontal = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
        if (!moveConfig) Debug.LogError($"{nameof(moveConfig)} is null on {name}");
    }

    /// <summary>
    /// Applies horizontal velocity with input smoothing toward <paramref name="targetVelocity"/>. <br/>
    /// Smoothing rate is derived from <see cref="PlayerMoveConfig.inputSmoothingTime"/>. <br/>
    /// <br/><br/>
    /// Note: vertical component is written directly to bypass smoothing.
    /// </summary>
    public override void ApplyVelocity(Vector3 targetVelocity, ForceMode forceMode = ForceMode.VelocityChange)
    {
        Vector3 targetHorizontalVelocity = RemoveVerticalComponent(targetVelocity);
        float targetSpeed = targetHorizontalVelocity.magnitude;
        float rate = (targetSpeed > 0f ? 
            targetSpeed : moveConfig.baseMoveSpeed) // latter is fallback since targetSpeed could be 0 => rate = 0 => no movement
            / moveConfig.inputSmoothingTime; 

        smoothedHorizontal = Vector3.MoveTowards(
            smoothedHorizontal,
            targetHorizontalVelocity,
            rate * Time.fixedDeltaTime
        );

        rb.linearVelocity = new(
            smoothedHorizontal.x,
            targetVelocity.y, // see verticality handler for this component
            smoothedHorizontal.z
        );
    }
}