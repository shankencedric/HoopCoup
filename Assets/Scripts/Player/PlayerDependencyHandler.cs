using UnityEngine;

/// <summary>
/// Dependency handler for all scripts player-related. 
/// In particular, this script handles input update event subscriptions and unsubscriptions, 
/// removing the need to reference the <see cref="PlayerInputHandler"/> directly.
/// </summary>
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovementHandler))]
[RequireComponent(typeof(PlayerLookHandler))]
[RequireComponent(typeof(PlayerVerticalityHandler))]
[RequireComponent(typeof(GravityHandler))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerDependencyHandler : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private PlayerMovementHandler movement;
    private PlayerLookHandler look;
    private PlayerVerticalityHandler jump;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        movement     = GetComponent<PlayerMovementHandler>();
        look         = GetComponent<PlayerLookHandler>();
        jump         = GetComponent<PlayerVerticalityHandler>();

        inputHandler.OnInputChanged += movement.UpdateInputState;
        inputHandler.OnInputChanged += look.UpdateLookInputState;
        inputHandler.OnInputChanged += jump.UpdateInputState;
    }

    private void OnDestroy()
    {
        inputHandler.OnInputChanged -= movement.UpdateInputState;
        inputHandler.OnInputChanged -= look.UpdateLookInputState;
        inputHandler.OnInputChanged -= jump.UpdateInputState;
    }
}