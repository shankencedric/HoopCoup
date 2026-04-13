using UnityEngine;

/// <summary>
/// Simple gravity applier script to applicable to objects that have implemented <see cref="IGravityAffected"/> and <see cref="IMotor"/> (separately).
/// </summary>
public class GravityHandler : MonoBehaviour
{
    /// <remarks> Accessed by this script and <see cref="IGravityAffected"/> children/implementors (e.g., <seealso cref="PlayerVerticalityHandler"/>). </remarks>
    public PhysicsConfig worldPhysicsConfig;
    private IGravityAffected target;
    private IMotor motor;

    private void Awake()
    {
        target = GetComponent<IGravityAffected>();
        motor  = GetComponent<IMotor>();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (target.IsGrounded || target.BypassGravityApplication)
        {
            motor.SetTargetVelocityY(worldPhysicsConfig.groundedBaseVerticalVelocity);
            return;
        }

        float current = motor.GetTargetVelocityY();
        float gravity = worldPhysicsConfig.GetActiveGravity(current);

        float newVertical = current + (gravity * Time.fixedDeltaTime);
        motor.SetTargetVelocityY(Mathf.Max(newVertical, worldPhysicsConfig.terminalVelocity));
    }
}