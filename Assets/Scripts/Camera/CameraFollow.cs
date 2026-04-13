using UnityEngine;

/// <summary> 
/// Simple third-person follow camera that stays behind and above the target at a fixed angle.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Offset")]
    [SerializeField] private Vector3 offset = new(0f, 4f, -6f);

    [Header("Follow")]
    [SerializeField] private float followSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Look")]
    [SerializeField] private Vector3 lookOffset = new(0f, 1.5f, 0f);

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.TransformPoint(offset);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        Vector3 lookTarget = target.position + lookOffset;
        Quaternion desiredRotation = Quaternion.LookRotation(lookTarget - transform.position);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}