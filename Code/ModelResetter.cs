//============================================================
// ModelResetter.cs
//
// Description:
// Repositions the AR model in front of the camera and resets 
// its visibility and orientation.
//
// Setup Instructions:
// 1. Attach this script to an empty GameObject (e.g., "ModelManager").
// 2. Drag the ARCamera into the 'arCamera' field in the Inspector.
// 3. Drag the main model or model parent into 'modelRoot'.
// 4. Link the 'RepositionModel()' function to a UI Button's OnClick event.
//
//============================================================

using UnityEngine;

public class ModelResetter : MonoBehaviour
{
    public Transform arCamera;       // Reference to the AR Camera
    public GameObject modelRoot;     // The model's parent GameObject
    public float distanceFromCamera = 2.0f; // Distance in front of camera to place model

    public void RepositionModel()
    {
        if (arCamera == null || modelRoot == null)
        {
            Debug.LogWarning("AR Camera or Model Root is not assigned.");
            return;
        }

        // Get forward direction, flatten Y axis to keep it on the ground
        Vector3 forward = arCamera.forward;
        forward.y = 0;
        forward.Normalize();

        // Calculate new position
        Vector3 newPosition = arCamera.position + forward * distanceFromCamera;

        // Move and reset rotation
        modelRoot.transform.position = newPosition;
        modelRoot.transform.rotation = Quaternion.identity;

        // Ensure model is visible
        modelRoot.SetActive(true);

        Debug.Log("Model repositioned in front of user.");
    }
}
