using System.Collections;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    [Header("References")]
    public CarPool pool;
    public Camera simCamera;

    [Header("Paths")]
    public RoadPath[] paths;

    [Header("Traffic Settings")]
    [Tooltip("Cars per path to maintain (approx).")]
    public int carsPerPath = 6;

    [Tooltip("Seconds between spawns during fill.")]
    public float spawnInterval = 0.35f;

    [Tooltip("Min/Max car speed in m/s (6–11 ~ 22–40 km/h).")]
    public Vector2 speedRange = new Vector2(6f, 11f);

    [Tooltip("Number of lanes simulated visually (offset left/right). 1 means center, 2 means left/right.")]
    public int lanes = 1; // 1 or 2 is safe

    [Tooltip("If true, each car starts from a random waypoint along the path.")]
    public bool randomizeStart = true;

    [Tooltip("Max active distance from camera (passed to CarAgent).")]
    public float maxActiveDistance = 65f;

    void Start()
    {
        if (simCamera == null) simCamera = Camera.main;
        StartCoroutine(FillAllPaths());
    }

    IEnumerator FillAllPaths()
    {
        if (pool == null || paths == null) yield break;

        foreach (var p in paths)
        {
            if (p == null || p.Count < 2) continue;

            int count = Mathf.Max(0, carsPerPath);
            for (int i = 0; i < count; i++)
            {
                SpawnOne(p);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    void SpawnOne(RoadPath path)
    {
        var go = pool.Get();
        if (go == null) return;

        var agent = go.GetComponent<CarAgent>();
        if (agent == null)
        {
            Debug.LogError("Car prefab missing CarAgent.");
            pool.Return(go);
            return;
        }

        float laneOffset = 0f;
        if (lanes >= 2)
        {
            // Alternate left/right lanes
            float half = path.laneWidth * 0.5f * 0.9f; // slight margin
            laneOffset = (Random.value < 0.5f) ? -half : +half;
        }

        float spd = Random.Range(speedRange.x, speedRange.y);

        agent.Initialize(path, spd, laneOffset, simCamera, path.loop);
        agent.maxActiveDistance = maxActiveDistance;

        if (randomizeStart)
            agent.JumpToRandomWaypoint();
    }
}
