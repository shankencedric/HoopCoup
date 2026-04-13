public interface IGravityAffected
{
    /// <summary> Main trigger for gravity. </summary>
    bool IsGrounded { get; }
    
    /// <summary> Sneaky bypass for gravity application. Doesn't touch the base grounded gravity. </summary>
    bool BypassGravityApplication { get; }
    
    /// <summary> Indicates the TARGET/INTENDED vertical velocity, to be set by the handler script. </summary>
    float TargetVelocityY { get; set; }

}