using UnityEngine;

/// <summary>
/// Makes use of consecutive bounces (within a set timer) to determine the smoothing of the rest of the bounces (aka, ground settling).
/// </summary>
/// <remarks> See <seealso cref="BallConfig"/>.</remarks>
[RequireComponent(typeof(SphereCollider))]
public class BallVerticalityHandler : MonoBehaviour, IGravityAffected
{
    [SerializeField] private BallConfig ballConfig;
    [SerializeField] private LayerMask groundLayerMask;

    // Ground Check / Smoothing
    private PhysicsMaterial physicsMaterial;
    private uint bounceCount = 0;
    private float bounceCountdown = 0f;


    // IGravityAffected
    public bool IsGrounded { get; private set; }
    public bool BypassGravityApplication { get; private set; }
    public float TargetVelocityY
    {
        get => TargetVelocityY;
        set => TargetVelocityY = value;
    }

    private void Awake()
    {
        physicsMaterial = GetComponent<SphereCollider>().material;
        physicsMaterial.bounciness = ballConfig.bounciness;
    }

    #region GROUND CHECK & SMOOTHING
    private void OnCollisionEnter(Collision collision)
    {
        if (IsInGroundLayerMask(collision.gameObject)) HandleBounceEnter();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsInGroundLayerMask(collision.gameObject)) HandleBounceExit();
    }

    private void HandleBounceEnter()
    {
        IsGrounded = true;
        if (bounceCountdown > 0) bounceCount++;
        else StartCountdown();
    }

    private void HandleBounceExit()
    {
        IsGrounded = false;
        if (bounceCountdown <= 0) bounceCount = 0;
    }

    #region COUNTDOWN
    private void Update()
    {
        Countdown();

        // Smoothing logic
        if (bounceCount >= ballConfig.consecutiveBounceCount)
        {
            physicsMaterial.bounciness = ballConfig.groundedBounciness;
        }
        else physicsMaterial.bounciness = ballConfig.bounciness;
    }

    private void Countdown()
    {
        if (bounceCountdown > 0) bounceCountdown -= Time.deltaTime;
        else bounceCountdown = 0f;
    }
    private void StartCountdown() => bounceCountdown = ballConfig.consecutiveBounceTimer;

    private bool IsInGroundLayerMask(GameObject obj) => ((1 << obj.layer) & groundLayerMask.value) != 0;

    #endregion

    /// <summary> Use to bypass gravity application (e.g., when ball is held). </summary>
    public void BypassGroundedness(bool bypass) => BypassGravityApplication = bypass;

    #endregion
}
