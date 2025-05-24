//============================================================
// CameraMoveTrigger.cs
//
// Description:
// This script is attached to a world-space UI Button placed 
// around a 3D model (e.g., a city). When the button is clicked,
// it smoothly moves and rotates the AR Camera Pivot to that 
// button's position and makes the camera look at the model center.
//
// Use case:
// Allows users to tap on different parts of the model (via buttons)
// and view it from those predefined positions.
//
// Attach this script to each button and assign:
// - `cameraTarget`: The AR Camera Pivot (the object that moves)
// - `modelCenter`: The central point the camera should look at
//
//============================================================

using UnityEngine;
using UnityEngine.UI;

public class CameraMoveTrigger : MonoBehaviour
{
    public Transform cameraTarget;      // ARCamera Pivot
    public Transform modelCenter;       // City center or object to focus on
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    private Button btn;
    private static Coroutine currentMoveCoroutine; // static to cancel other camera triggers globally

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(MoveCameraHere);
    }

    void MoveCameraHere()
    {
        // Stop previous move if it's running
        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
        }

        // Start new movement
        currentMoveCoroutine = StartCoroutine(MoveCamera(transform.position));
    }

    System.Collections.IEnumerator MoveCamera(Vector3 targetPos)
    {
        while (Vector3.Distance(cameraTarget.position, targetPos) > 0.01f)
        {
            // Smooth position
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, targetPos, Time.deltaTime * moveSpeed);

            // Smooth rotation towards the model center
            Quaternion targetRotation = Quaternion.LookRotation(modelCenter.position - cameraTarget.position);
            cameraTarget.rotation = Quaternion.Slerp(cameraTarget.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            yield return null;
        }

        // Final position & rotation
        cameraTarget.position = targetPos;
        cameraTarget.LookAt(modelCenter);
    }
}
