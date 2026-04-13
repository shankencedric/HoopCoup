using UnityEngine;

[CreateAssetMenu(fileName = "PhysicsConfig", menuName = "ScriptableObjects/Configs/PhysicsConfig", order = 3)]
public class PhysicsConfig : ScriptableObject
{
    [Header("Gravity")]
    public float gravityUpwards = -16.7f;
    public float gravityDownwards = -26.7f;
    public float terminalVelocity = -36.7f;

    [Tooltip("Fallback downwards pull value when motor isGrounded")]
    public float groundedBaseVerticalVelocity = -1f;

    /// <summary>
    /// Centralized gravity logic for switching between <see cref="gravityUpwards"/> and <see cref="gravityDownwards"/>.
    /// </summary>
    /// <param name="velocityY"></param>
    /// <returns>Gravity depending on the y-direction of the object</returns>
    public float GetActiveGravity(float velocityY) => velocityY > 0f ? gravityUpwards : gravityDownwards;
}