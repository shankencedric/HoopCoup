using System;
using UnityEngine;
using UnityEngine.InputSystem;

public struct BallInputState
{
    public bool HasPressedGrabThisFrame;
    public bool HasPressedReleaseThisFrame;
    public bool IsPressingShoot;
}

/// <summary> Event-driver that handles all the ball-related input. </summary>
/// <remarks> Very similar with <seealso cref="PlayerInputHandler"/>.</remarks>
public class BallInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction grabAction;
    private InputAction releaseAction;
    private InputAction shootAction;

    public event Action<BallInputState> OnInputChanged;

    private void Awake()
    {
        grabAction = inputActions.FindAction("Grab");
        releaseAction = inputActions.FindAction("Release");
        shootAction = inputActions.FindAction("Shoot");
    }

    private void OnEnable()
    {
        grabAction.Enable();
        releaseAction.Enable();
        shootAction.Enable();
    }

    private void OnDisable()
    {
        grabAction.Disable();
        releaseAction.Disable();
        shootAction.Disable();
    }

    private void Update()
    {
        UpdateBallInputState();
    }

    /// <summary> Determines new inputs and sets off the event to trigger for subscribers. </summary>
    private void UpdateBallInputState()
    {
        var state = new BallInputState
        {
            HasPressedGrabThisFrame = grabAction.WasPressedThisFrame(),
            HasPressedReleaseThisFrame = releaseAction.WasPressedThisFrame(),
            IsPressingShoot = shootAction.IsPressed()
        };

        OnInputChanged?.Invoke(state);
    }
}