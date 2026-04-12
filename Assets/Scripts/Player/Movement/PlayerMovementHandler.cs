using UnityEngine;

public enum PlayerMovementState
{
    Idling,
    Walking,
    Sprinting,
    Crouching
}

/// <summary> 
/// Handles all player movement controls, including movement state management. <br/><br/>
/// Related: <seealso cref="PlayerMotor"/>
/// </summary>
[RequireComponent(typeof(PlayerMotor))]
public class PlayerMovementHandler : MonoBehaviour
{
    /// <remarks> Taken from <see cref="PlayerMotor"/>. </remarks>
    private PlayerMoveConfig moveConfig; 

    private PlayerMotor motor;
    private PlayerInputState inputState;
    private PlayerMovementState currMoveState = PlayerMovementState.Idling;
    
    private Vector3 targetVelocityXZ = Vector3.zero;

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        moveConfig = motor.moveConfig;
    }

    #region STATES
    /// <summary> Listens to input changes from <see cref="PlayerInputHandler"/>, stores it locally, and updates the corresponding movement state. </summary>
    public void UpdateInputState(PlayerInputState input)
    {
        inputState = input;
        UpdateMovementState();
    }

    private void UpdateMovementState()
    {
        if (inputState.IsPressingCrouch)
            currMoveState = PlayerMovementState.Crouching;
        else if (inputState.IsPressingSprint && inputState.MoveInput.y > 0)
            currMoveState = PlayerMovementState.Sprinting;
        else if (inputState.MoveInput != Vector2.zero)
            currMoveState = PlayerMovementState.Walking;
        else
            currMoveState = PlayerMovementState.Idling;
    }
    #endregion

    #region MOVEMENT
    // Must be calculated in Update for responsiveness, but applied in FixedUpdate for physics consistency.
    private void Update() => CalculateMovement();
    private void FixedUpdate() => motor.SetTargetVelocityXZ(targetVelocityXZ);

    /// <summary>
    /// Calculate velocity (speed from <see cref="GetTargetSpeed"/>). <br/>
    /// Note: actual physics application will be handled by <see cref="PlayerMotor"/>.
    /// </summary>
    private void CalculateMovement()
    {
        Vector3 forward = transform.forward; forward.y = 0f;
        Vector3 right = transform.right; right.y = 0f;

        Vector3 moveDirection =
            forward.normalized * inputState.MoveInput.y +
            right.normalized * inputState.MoveInput.x;

        targetVelocityXZ = moveDirection * GetTargetSpeed();
    }

    /// <returns> Appropriate speed based on local movement state. </returns>
    private float GetTargetSpeed()
    {
        return currMoveState switch
        {
            PlayerMovementState.Idling    => 0f,
            PlayerMovementState.Walking   => moveConfig.baseMoveSpeed,
            PlayerMovementState.Sprinting => moveConfig.baseMoveSpeed * moveConfig.sprintMultiplier,
            PlayerMovementState.Crouching => moveConfig.baseMoveSpeed * moveConfig.crouchMultiplier,
            _                             => moveConfig.baseMoveSpeed
        };
    }
   #endregion
}