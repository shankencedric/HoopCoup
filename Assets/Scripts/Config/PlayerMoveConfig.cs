using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoveConfig", menuName = "ScriptableObjects/Configs/PlayerMoveConfig", order = 1)]
public class PlayerMoveConfig : ScriptableObject
{
    #region MOVEMENT
    [Header("Movement")]
    public float baseMoveSpeed = 6.7f;
    public float sprintMultiplier = 1.67f;
    public float crouchMultiplier = 0.67f;
    #endregion

    #region JUMP
    [Header("Jumping")]
    public float jumpHeight = 2.7889f; 
    public float baseJumpVelocity = 1.67f;

    [Header("Jumping Mods")]
    public float sprintJumpMultiplier = 1.167f; 
    public float crouchJumpMultiplier = 0.67f; 

    [Header("Ground Check")]
    public Vector3 groundCheckRayOriginOffset = new(0f, 0f, 0f);
    public float groundCheckRayLength = 0.167f;
    public float groundCheckRayRadius = 0.167f;
    public float groundCheckRayConeDegree = 67f;
    #endregion    

    #region EXTRAS
    [Header("Input Smoothing")]

    [Tooltip("Time in seconds to smooth input changes")]
    public float inputSmoothingTime = 0.067f;
    #endregion
}