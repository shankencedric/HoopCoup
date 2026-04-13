using UnityEngine;

[CreateAssetMenu(fileName = "BallConfig", menuName = "ScriptableObjects/Configs/BallConfig")]
public class BallConfig : ScriptableObject
{
    [Header("Grab")]
    public float followSpeed = 1f;

    [Header("Physics")]
    //public float bounciness = 1f;
    public float shootForce = 12f;
    public float upwardBias = 0.2f;

    [Header("Ground Check")]
    public Vector3 groundCheckRayOriginOffset = Vector3.zero;
    public float groundCheckRayLength = 1f;
}
