using UnityEngine;

/// <summary>
/// Simply detects proper entrance and exit of the basketball through the hoop.
/// </summary>
public class HoopDetector : MonoBehaviour
{
    private bool hasEntered = false;
    private bool hasExited = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (hasExited) ResetBools();
            else hasEntered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            
            if (!hasEntered) ResetBools();
            else hasExited = other.attachedRigidbody.linearVelocity.y < 0f; // still downwards velocity
        }
    }

    private void Update()
    {
        if (hasEntered && hasExited)
        {
            GameManager.Instance.Score();
            ResetBools();
        }
    }

    private void ResetBools()
    {
        hasEntered = false;
        hasExited = false;
    }
}
