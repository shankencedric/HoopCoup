using System;
using UnityEngine;

public class GrabbableHandler : MonoBehaviour
{
    public Action<bool> OnPlayerNear;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerNear?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerNear?.Invoke(false);
        }
    }
}
