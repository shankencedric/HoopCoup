using UnityEngine;

[CreateAssetMenu(fileName = "BallConfig", menuName = "ScriptableObjects/Configs/BallConfig")]
public class BallConfig : ScriptableObject
{
    [Header("Grab")]
    public float followSpeed = 1f;

    [Header("Shoot")]
    public float shootForce = 12f;
    public float upwardBias = 0.2f;

    [Header("Ground Check / Smoothing")]
    public float bounciness = 0.5f;
    public float groundedBounciness = 0.1f;
    
    public uint consecutiveBounceCount = 3;
    public float consecutiveBounceTimer = 0.25f;
}
