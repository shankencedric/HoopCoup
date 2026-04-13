using UnityEngine;

public class HoopDetector : MonoBehaviour
{
    private bool hasEntered = false;
    private bool hasExited = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            hasEntered = other.attachedRigidbody.linearVelocity.y < 0f; // downwards velocity
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            hasExited = other.attachedRigidbody.linearVelocity.y < 0f; // still downwards velocity
        }
    }

    private void Update()
    {
        if (hasEntered && hasExited)
        {
            hasEntered = false;
            hasExited = false;
        }
    }
}
