using System;
using UnityEngine;
using UnityEngine.InputSystem;

public struct PlayerInputState
{
    public Vector2 MoveInput;
    public Vector2 LookInput;
    public bool IsPressingSprint;
    public bool IsPressingCrouch;
    public bool IsPressingJump; // could be refactored to use "HasPressedJumpThisFrame" architecture
}

/// <summary> Event-driver that handles all the player-movement input (e.g., wasd, mouse look, sprint/crouch buttons, etc.) </summary>
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction lookAction; 
    private InputAction sprintAction; 
    private InputAction crouchAction; 
    private InputAction jumpAction;

    public event Action<PlayerInputState> OnInputChanged;

    private void Awake()
    {
        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");
        sprintAction = inputActions.FindAction("Sprint");
        crouchAction = inputActions.FindAction("Crouch");
        jumpAction = inputActions.FindAction("Jump");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        crouchAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();
        crouchAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        UpdatePlayerInputState();
    }

    /// <summary> Determines new inputs and sets off the event to trigger for subscribers. </summary>
    private void UpdatePlayerInputState()
    {
        var state = new PlayerInputState
        {
            MoveInput = moveAction.ReadValue<Vector2>().normalized,
            LookInput = lookAction.ReadValue<Vector2>(),
            IsPressingSprint = sprintAction.IsPressed(),
            IsPressingCrouch = crouchAction.IsPressed(),
            IsPressingJump = jumpAction.IsPressed()
        };

        OnInputChanged?.Invoke(state);
    }
}