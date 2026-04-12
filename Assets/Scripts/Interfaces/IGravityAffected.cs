public interface IGravityAffected
{
    bool IsGrounded { get; }
    
    /// <summary> Indicates the TARGET/INTENDED vertical velocity, to be set by the handler script. </summary>
    float TargetVelocityY { get; set; }
}