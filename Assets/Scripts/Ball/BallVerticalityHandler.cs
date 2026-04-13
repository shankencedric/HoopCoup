using UnityEngine;

/// <remarks>Very similar with <seealso cref="PlayerVerticalityHandler"/>.</remarks>
public class BallVerticalityHandler : MonoBehaviour, IGravityAffected
{
    [SerializeField] private BallConfig ballConfig;
    [SerializeField] private LayerMask groundLayerMask;

    // Ground Check
    private SphereCollider ballCollider;

    // IGravityAffected
    public bool IsGrounded { get; private set; }
    public float TargetVelocityY
    {
        get => TargetVelocityY;
        set => TargetVelocityY = value;
    }

    private void Awake()
    {
        ballCollider = GetComponent<SphereCollider>();
    }

    #region GROUND CHECK
    private void Update() => UpdateIsGrounded();

    private void UpdateIsGrounded()
    {
        Vector3 rayOrigin = GetRayOrigin();
        IsGrounded = Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, ballConfig.groundCheckRayLength, groundLayerMask)
            && IsInGroundLayerMask(hit.transform.gameObject);
    }

    private Vector3 GetRayOrigin()
    {
        Vector3 bottom = transform.position
            + ballCollider.center
            + Vector3.down * ballCollider.radius;
        return bottom + Vector3.up * ballConfig.groundCheckRayOriginOffset.y;
    }

    private bool IsInGroundLayerMask(GameObject obj) => ((1 << obj.layer) & groundLayerMask.value) != 0;
    #endregion

    #region UNITY EDITOR 
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (ballCollider == null) return;
        Vector3 origin = GetRayOrigin();
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(origin, Vector3.down * ballConfig.groundCheckRayLength);
    }
    #endif
    #endregion
}
