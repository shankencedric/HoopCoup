using UnityEngine;

public class BallShootingHandler : MonoBehaviour
{
    [SerializeField] private Transform holdTransform;
    private BallMotor motor;
    private BallConfig config;

    private void Awake()
    {
        motor = GetComponent<BallMotor>();
        config = motor.moveConfig;
    }

    /// <remarks> Incorporates an upward bias based on the <see cref="BallConfig.upwardBias"/> </remarks>
    public void Shoot(Vector3 direction)
    {
        motor.ApplyVelocity(direction * config.shootForce, ForceMode.Impulse);
    }
}
