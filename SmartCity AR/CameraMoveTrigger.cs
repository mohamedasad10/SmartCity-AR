using UnityEngine;
using UnityEngine.UI;

public class CameraMoveTrigger : MonoBehaviour
{
    public Transform cameraPivot;     // Parent object of ARCamera (Pivot/Offset)
    public Transform modelCenter;     // City center
    public Transform cameraWaypoint;  // The position/angle to move the pivot to
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    private Button btn;
    private static Coroutine currentMoveCoroutine;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(MoveCameraHere);
    }

    void MoveCameraHere()
    {
        if (currentMoveCoroutine != null)
            StopCoroutine(currentMoveCoroutine);

        if (cameraWaypoint == null || cameraPivot == null) return;

        currentMoveCoroutine = StartCoroutine(MoveCamera(cameraWaypoint.position));
    }

    System.Collections.IEnumerator MoveCamera(Vector3 targetPos)
    {
        while (Vector3.Distance(cameraPivot.position, targetPos) > 0.01f)
        {
            // Smooth position
            cameraPivot.position = Vector3.Lerp(cameraPivot.position, targetPos, Time.deltaTime * moveSpeed);

            // Smooth rotation towards city
            Quaternion targetRotation = Quaternion.LookRotation(modelCenter.position - cameraPivot.position);
            cameraPivot.rotation = Quaternion.Slerp(cameraPivot.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            yield return null;
        }

        // Snap final position/rotation
        cameraPivot.position = targetPos;
        cameraPivot.LookAt(modelCenter);
    }
}
