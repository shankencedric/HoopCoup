using UnityEngine;

/// <summary>
/// Abstract <see cref="MonoBehaviour"/> base for any <see cref="Rigidbody"/>-driven motor. <br/>
/// Implements <see cref="IMotor"/> with direct velocity assignment as the default behavior. <br/>
/// Subclasses override <see cref="ApplyVelocity"/> to customize application
/// (e.g., smoothing in <see cref="PlayerMotor"/>, impulse in <see cref="BoulderMotor"/>).
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class RigidbodyMotor : MonoBehaviour, IMotor
{
    /// <summary> The underlying physics body. Accessible to subclasses. </summary>
    protected Rigidbody rb;

    private Vector3 targetVelocityXZ = Vector3.zero;
    private float targetVelocityY = 0f;

    protected virtual void Awake() => rb = GetComponent<Rigidbody>(); 

    protected virtual void FixedUpdate() {
        DebugLogger.LogPhysicsDetailsIfSelected(this, gameObject);
        ApplyVelocity(GetTargetVelocity()); 
    }

    #region IMotor.Setters
    public void SetTargetVelocity(Vector3 velocity)
    {
        targetVelocityXZ = RemoveVerticalComponent(velocity);
        targetVelocityY = velocity.y;
    }
    public void SetTargetVelocityXZ(Vector3 velocity) => targetVelocityXZ = velocity;
    public void SetTargetVelocityY(float velocity) => targetVelocityY = velocity;
    #endregion

    #region IMotor.Getters
    public Vector3 GetTargetVelocity() => AggregateVelocity(GetTargetVelocityXZ(), GetTargetVelocityY());
    public Vector3 GetTargetVelocityXZ() => targetVelocityXZ;
    public float GetTargetVelocityY() => targetVelocityY;
    public Vector3 GetVelocity() => rb.linearVelocity;
    public Vector3 GetVelocityXZ() => RemoveVerticalComponent(rb.linearVelocity);
    public float GetVelocityY() => rb.linearVelocity.y;
    #endregion

    #region IMotor.Application
    /// <summary>
    /// Default implementation: unadulterated call of <see cref="Rigidbody.AddForce"/>. <br/>
    /// Override in subclasses for custom behavior.
    /// </summary>
    public virtual void ApplyVelocity(Vector3 targetVelocity, ForceMode forceMode = ForceMode.VelocityChange)
    {
        rb.linearVelocity = targetVelocity;
    }
    #endregion

    #region Utilities 
    /// <returns> 3D aggregated velocity from horizontal (2D) and vertical (1D) velocity (optional). </returns>
    protected static Vector3 AggregateVelocity(Vector3 velocityXZ, float velocityY) 
        => new(velocityXZ.x, velocityY, velocityXZ.z);
    protected static Vector3 AggregateVelocity(float velocityX, float velocityY, float velocityZ)
        => new(velocityX, velocityY, velocityZ);

    /// <returns> 3D vector with zero vertical (Y) component. </returns>
    protected static Vector3 RemoveVerticalComponent(Vector3 velocity)
        => new(velocity.x, 0f, velocity.z);
    #endregion
}