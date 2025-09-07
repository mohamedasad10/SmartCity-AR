using UnityEngine;

public class LockARCamera : MonoBehaviour
{
    void LateUpdate()
    {
        // Force ARCamera to stay locked inside its pivot
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
