//============================================================
// FreeCameraController.cs
//
// Description:
// This script allows the user to freely control the AR Camera using 
// touch gestures on a mobile device. It supports:
// - 1-finger drag to rotate the camera around its current position
// - 2-finger pinch to zoom in/out along the camera's forward direction
// - Double tap on a surface to reposition the camera closer to that point
//
// Use case:
// Gives users full manual control over viewing angles and distance 
// when exploring a 3D model in an AR scene.
//
// Setup Instructions:
// 1. Attach this script to the **AR Camera Pivot or Offset GameObject**,
//    NOT directly to the ARCamera itself. This object should be the parent 
//    of the ARCamera and responsible for moving and rotating the camera.
// 2. Ensure this GameObject starts positioned and rotated correctly in your scene.
// 3. Add colliders (e.g., ground, mesh colliders on objects) to
//    allow the double-tap movement to detect surfaces.
// 4. Adjust rotation speed, zoom limits, and sensitivity in the Inspector.
//============================================================


using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public float rotationSpeed = 0.2f;
    public float zoomSpeed = 0.5f;
    public float minZoomDistance = 2f;
    public float maxZoomDistance = 100f;

    private float yaw = 0f;   // Horizontal rotation
    private float pitch = 0f; // Vertical rotation

    public float minPitch = -45f;
    public float maxPitch = 45f;

    private Vector2 lastTouchPos;
    private bool isRotating = false;

    private float lastTapTime = 0f;
    private Vector2 lastTapPosition;
    private float doubleTapThreshold = 0.4f;
    private float tapPositionThreshold = 50f;

    void Start()
    {
        // Initialize yaw and pitch based on current camera rotation
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        // === 1-Finger Drag: Rotate Camera ===
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                float timeSinceLastTap = Time.time - lastTapTime;

                if (timeSinceLastTap <= doubleTapThreshold &&
                    Vector2.Distance(touch.position, lastTapPosition) <= tapPositionThreshold)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        Vector3 targetPosition = hit.point - transform.forward * 5f;
                        transform.position = targetPosition;
                    }

                    lastTapTime = 0f;
                }
                else
                {
                    lastTapTime = Time.time;
                    lastTapPosition = touch.position;
                }

                lastTouchPos = touch.position;
                isRotating = true;
            }
            else if (touch.phase == TouchPhase.Moved && isRotating)
            {
                Vector2 delta = touch.deltaPosition;

                yaw += delta.x * rotationSpeed;
                pitch -= delta.y * rotationSpeed;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

                transform.rotation = Quaternion.Euler(pitch, yaw, 0f); // Clean: no roll
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isRotating = false;
            }
        }

        // === 2-Finger Pinch Zoom ===
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(t0.position, t1.position);
            float previousDistance = Vector2.Distance(t0.position - t0.deltaPosition, t1.position - t1.deltaPosition);

            float deltaDistance = currentDistance - previousDistance;

            Vector3 forward = transform.forward;
            Vector3 newPosition = transform.position + forward * deltaDistance * zoomSpeed * 0.01f;

            float distanceFromOrigin = Vector3.Distance(newPosition, Vector3.zero);

            if (distanceFromOrigin > minZoomDistance && distanceFromOrigin < maxZoomDistance)
            {
                transform.position = newPosition;
            }
        }
    }
}
