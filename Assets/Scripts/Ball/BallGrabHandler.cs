using UnityEngine;

public struct BallGrabState
{
    public bool IsNear;
    public bool IsHeld;
}

[RequireComponent(typeof(BallMotor))]
[RequireComponent(typeof(BallShootingHandler))]
public class BallGrabHandler : MonoBehaviour
{
    [SerializeField] private Transform holdTransform;
    [SerializeField] private PlayerLookHandler playerLooker;

    private BallShootingHandler ballShooter;
    private BallConfig config;

    private BallInputState inputState;
    private BallGrabState currGrabState;

    private BallMotor motor;

    private void Awake()
    {
        ballShooter = GetComponent<BallShootingHandler>();
        motor = GetComponent<BallMotor>();
        config = motor.moveConfig;

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

    /// <summary> Shoot the ball. The actual application is continued in <see cref="ballShooter"/>. </summary>
    private void Shoot()
    {
        Release();
        ballShooter.Shoot(playerLooker.GetAimForward());
    }

    public void UpdateBallNearState(bool near) => currGrabState.IsNear = near;
}