using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLookConfig", menuName = "ScriptableObjects/Configs/PlayerLookConfig", order = 2)]
public class PlayerLookConfig : ScriptableObject
{
    [Header("Look Sensitivity")]
    public float sensitivity = 6.7f;
    public float maxUpwardAngle = 67f;
    public float maxDownwardAngle = -67f;
}