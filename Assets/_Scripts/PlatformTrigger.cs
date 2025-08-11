using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public Rigidbody detectedRigidbody = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            detectedRigidbody = other.attachedRigidbody;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            detectedRigidbody = null;
        }
    }
}
