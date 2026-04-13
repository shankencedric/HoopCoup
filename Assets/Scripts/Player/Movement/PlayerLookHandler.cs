using UnityEngine;

/// <summary> Handles all player camera controls. </summary>
public class PlayerLookHandler : MonoBehaviour
{
    [SerializeField] private PlayerLookConfig lookConfig;
    [SerializeField] private Transform playerFace;

    private Vector2 lookInput;

    private float currentPitch = 0f;
    private float currentYaw = 0f;

    /// <summary> Listens to input changes from <see cref="PlayerInputHandler"/> and stores state. </summary>
    public void UpdateLookInputState(PlayerInputState playerInputState) => lookInput = playerInputState.LookInput;

    private void Update()
    {
        // left and right
        float yawDelta = lookInput.x * lookConfig.sensitivity; 
        currentYaw = (currentYaw + yawDelta) % 360f; 
        transform.rotation = Quaternion.AngleAxis(currentYaw, Vector3.up);

        // up and down
        float pitchDelta = lookInput.y * lookConfig.sensitivity; 
        currentPitch = Mathf.Clamp(
            currentPitch + pitchDelta, 
            lookConfig.maxDownwardAngle, 
            lookConfig.maxUpwardAngle);
        playerFace.localRotation = Quaternion.AngleAxis(currentPitch, Vector3.left);
    }    

    public Vector3 GetAimForward() => playerFace.forward;
}
