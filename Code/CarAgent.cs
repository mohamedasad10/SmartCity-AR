using UnityEngine;

[RequireComponent(typeof(Transform))]
public class CarAgent : MonoBehaviour
{
    [Header("Runtime")]
    public RoadPath path;
    public int currentIndex;
    public float segmentT; // 0..1 along current segment
    public float speed = 7f; // m/s
    public float laneOffset = 0f; // meters (+right, -left)
    public bool loopPath = true;
    public Camera simCamera;
    public float maxActiveDistance = 65f; // meters from camera to simulate
    public float lookAhead = 0.5f; // how close to snap to next WP (meters)

    Transform _tr;
    Renderer _rend;
    bool _initialized;

    public void Initialize(RoadPath p, float speedMps, float offsetMeters, Camera cam, bool loop)
    {
        path = p;
        speed = speedMps;
        laneOffset = offsetMeters;
        simCamera = cam;
        loopPath = loop;
        currentIndex = 0;
        segmentT = 0f;
        _initialized = true;
        SnapToCurrent();
    }

    public void JumpToRandomWaypoint()
    {
        if (path == null || path.Count < 2) return;
        currentIndex = Random.Range(0, path.Count - 1);
        segmentT = Random.value; // random position along segment
        SnapToCurrent();
    }

    void Awake()
    {
        _tr = transform;
        _rend = GetComponentInChildren<Renderer>();
    }

    void SnapToCurrent()
    {
        if (path == null || path.Count < 2) return;

        var a = path.GetWaypoint(currentIndex).position;
        var b = path.GetWaypoint(path.NextIndex(currentIndex)).position;
        Vector3 dir = (b - a).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, dir);
        Vector3 pos = Vector3.Lerp(a, b, Mathf.Clamp01(segmentT)) + right * laneOffset;

        _tr.position = pos;
        // Face forward
        if (dir.sqrMagnitude > 0.0001f)
            _tr.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    void Update()
    {
        if (!_initialized || path == null || path.Count < 2) return;

        if (!ShouldSimulate())
            return;

        MoveAlongPath(Time.deltaTime);
    }

    bool ShouldSimulate()
    {
        if (simCamera == null) simCamera = Camera.main;
        if (simCamera == null) return true; // last resort

        float d2 = (simCamera.transform.position - _tr.position).sqrMagnitude;
        return d2 <= maxActiveDistance * maxActiveDistance;
    }

    void MoveAlongPath(float dt)
    {
        int next = path.NextIndex(currentIndex);
        Vector3 a = path.GetWaypoint(currentIndex).position;
        Vector3 b = path.GetWaypoint(next).position;

        Vector3 segment = b - a;
        float segLen = Mathf.Max(0.001f, segment.magnitude);
        Vector3 dir = segment / segLen;

        // step in segment space
        float worldStep = speed * dt;
        float tStep = worldStep / segLen;
        segmentT += tStep;

        while (segmentT >= 1f - (lookAhead / segLen))
        {
            segmentT -= 1f;
            currentIndex = next;
            if (!path.loop && currentIndex >= path.Count - 1)
            {
                // Reached end of path ? despawn or destroy
                if (Application.isPlaying)
                {
                    // Try returning to pool first
                    var pool = FindObjectOfType<CarPool>();
                    if (pool != null)
                    {
                        pool.Return(gameObject);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                return; // stop moving
            }

            next = path.NextIndex(currentIndex);
            a = path.GetWaypoint(currentIndex).position;
            b = path.GetWaypoint(next).position;
            segment = b - a;
            segLen = Mathf.Max(0.001f, segment.magnitude);
            dir = segment / segLen;
        }

        Vector3 right = Vector3.Cross(Vector3.up, dir);
        Vector3 pos = Vector3.Lerp(a, b, Mathf.Clamp01(segmentT)) + right * laneOffset;

        _tr.position = pos;
        if (dir.sqrMagnitude > 0.0001f)
            _tr.rotation = Quaternion.Lerp(_tr.rotation, Quaternion.LookRotation(dir, Vector3.up), 0.5f);
    }
}
