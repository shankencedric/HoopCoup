using UnityEngine;

public class BallMotor : RigidbodyMotor
{
    /// <remarks> Accessed by this script and PlayerVerticalityHandler </remarks>
    public BallConfig moveConfig;

    public override void ApplyVelocity(Vector3 targetVelocity, ForceMode forceMode = ForceMode.Force)
    {
        if (rb.isKinematic == true) return;
        rb.AddForce(targetVelocity, forceMode);
    }

    public void FollowPosition(Vector3 targetFollowPosition, float speed)
    {   
        transform.position = Vector3.Lerp(
            transform.position,
            targetFollowPosition,
            speed * Time.fixedDeltaTime
        );
    }

    public void TurnRbKinematic(bool isKinematic) => rb.isKinematic = isKinematic;
    
    public void ToggleCollision(bool shouldCollide) => rb.detectCollisions = shouldCollide;
}
