using UnityEngine;

/// <summary>
/// Dependency handler for all scripts ball-related. 
/// In particular, this script handles input update event subscriptions and unsubscriptions, 
/// removing the need to reference the <see cref="BallInputHandler"/> directly.
/// </summary>
/// <remarks> Very similar with <seealso cref="PlayerDependencyHandler"/>.</remarks>
[RequireComponent(typeof(BallGrabHandler))]
public class BallDependencyHandler : MonoBehaviour
{
    private BallInputHandler inputHandler;
    private BallGrabHandler grab;

    [SerializeField] private GrabbableHandler grabbableHandler;

    private void Awake()
    {
        inputHandler = GetComponent<BallInputHandler>();
        grab = GetComponent<BallGrabHandler>();

        inputHandler.OnInputChanged += grab.UpdateInputState;
        grabbableHandler.OnPlayerNear += grab.UpdateBallNearState;
    }

    private void OnDestroy()
    {
        inputHandler.OnInputChanged -= grab.UpdateInputState;
        grabbableHandler.OnPlayerNear -= grab.UpdateBallNearState;
    }
}
