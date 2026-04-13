using UnityEngine;

public class BallShootingHandler : MonoBehaviour
{
    [SerializeField] private Transform holdTransform;
    private BallConfig config;

    private BallMotor motor;

    private void Awake()
    {
        motor = GetComponent<BallMotor>();
        config = motor.moveConfig;
    }

    /// <remarks> Incorporates an upward bias based on the <see cref="BallConfig.upwardBias"/> </remarks>
    public void Shoot(Vector3 direction)
    {
        Vector3 adjustedDirection = (direction + Vector3.up * config.upwardBias).normalized;
        Vector3 shootVelocity = adjustedDirection * config.shootForce;
        motor.ApplyVelocity(shootVelocity, ForceMode.Impulse);
    }
}
