using UnityEngine;

public struct BallGrabState
{
    public bool IsNear;
    public bool IsHeld;
}

[RequireComponent(typeof(BallMotor))]
public class BallGrabHandler : MonoBehaviour
{
    [SerializeField] private Transform holdTransform;
    [SerializeField] private BallConfig config;

    private BallInputState inputState;
    private BallGrabState currGrabState;

    private BallMotor motor;

    private void Awake()
    {
        motor = GetComponent<BallMotor>();

        currGrabState = new BallGrabState
        {
            IsNear = false,
            IsHeld = false
        };
    }

    /// <summary> Listens to input changes from <see cref="BallInputHandler"/> and stores state. </summary>
    public void UpdateInputState(BallInputState input) => inputState = input;

    private void Update()
    {
        if (currGrabState.IsNear && !currGrabState.IsHeld && inputState.HasPressedGrabThisFrame)
        {
            Grab();
        }
        else if (currGrabState.IsHeld && inputState.HasPressedReleaseThisFrame)
        {
            Release();
        }
        else if (currGrabState.IsHeld && inputState.HasPressedShootThisFrame)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (currGrabState.IsHeld)
        {
            FollowPosition();
        }
    }

    private void FollowPosition()
    {
        motor.FollowPosition(holdTransform.position, config.followSpeed);
    }

    /// <summary> Grab the ball. </summary>
    private void Grab()
    {
        currGrabState.IsHeld = true;
        motor.TurnRbKinematic(true);
        motor.ToggleCollision(false);
    }

    /// <summary> Release the ball without shooting. </summary>
    private void Release()
    {
        currGrabState.IsHeld = false;
        motor.TurnRbKinematic(false);
        motor.ToggleCollision(true);
    }

    /// <summary> Shoot the ball. </summary>
    private void Shoot()
    {
        currGrabState.IsHeld = false;
        motor.TurnRbKinematic(false);
        motor.ToggleCollision(true);

        Vector3 shootDirection = transform.forward;
        shootDirection = (shootDirection + Vector3.up * config.upwardBias).normalized;

        Vector3 shootVelocity = shootDirection * config.shootForce;

        motor.ApplyVelocity(shootVelocity, ForceMode.Force);
    }

    public void UpdateBallNearState(bool near) => currGrabState.IsNear = near;
}