using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 0.5f;
    public float minZoomDistance = 2f;
    public float maxZoomDistance = 100f;

    void Update()
    {
        // === 2-Finger Pinch Zoom ===
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(t0.position, t1.position);
            float previousDistance = Vector2.Distance(t0.position - t0.deltaPosition, t1.position - t1.deltaPosition);

            float deltaDistance = currentDistance - previousDistance;

            // Move along forward axis
            Vector3 forward = transform.forward;
            Vector3 newPosition = transform.position + forward * deltaDistance * zoomSpeed * 0.01f;

            // Clamp zoom
            float distanceFromOrigin = Vector3.Distance(newPosition, Vector3.zero);
            if (distanceFromOrigin > minZoomDistance && distanceFromOrigin < maxZoomDistance)
            {
                transform.position = newPosition;
            }
        }
    }
}
