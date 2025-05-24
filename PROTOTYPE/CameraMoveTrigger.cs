using UnityEngine;
using UnityEngine.UI;

public class CameraMoveTrigger : MonoBehaviour
{
    public Transform cameraTarget;      // ARCamera Pivot (what should move)
    public Transform modelCenter;       // Center of the city model
    public float moveSpeed = 2f;

    private Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(MoveCameraHere);
    }

    void MoveCameraHere()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCamera(transform.position));
    }

    System.Collections.IEnumerator MoveCamera(Vector3 targetPos)
    {
        while (Vector3.Distance(cameraTarget.position, targetPos) > 0.01f)
        {
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, targetPos, Time.deltaTime * moveSpeed);
            cameraTarget.LookAt(modelCenter.position);
            yield return null;
        }

        cameraTarget.position = targetPos;
        cameraTarget.LookAt(modelCenter.position);
    }
}
