using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RoadPath : MonoBehaviour
{
    [Tooltip("Leave empty to auto-load all child waypoints")]
    public List<Transform> waypoints = new List<Transform>();

    [Tooltip("If true, cars wrap from last waypoint to first.")]
    public bool loop = true;

    [Tooltip("Approx lane width; used to offset cars left/right.")]
    public float laneWidth = 3.5f;

    void OnValidate()
    {
        if (waypoints == null || waypoints.Count == 0)
        {
            waypoints = new List<Transform>();
            foreach (Transform child in transform)
                waypoints.Add(child);
        }
    }

    public int Count => waypoints?.Count ?? 0;

    public Transform GetWaypoint(int index)
    {
        if (Count == 0) return null;
        if (loop)
            index = (index % Count + Count) % Count;
        else
            index = Mathf.Clamp(index, 0, Count - 1);
        return waypoints[index];
    }

    public int NextIndex(int i)
    {
        if (Count == 0) return 0;
        if (loop) return (i + 1) % Count;
        return Mathf.Min(i + 1, Count - 1);
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count < 2) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            if (!waypoints[i] || !waypoints[i + 1]) continue;
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
        if (loop && waypoints[0] && waypoints[^1])
        {
            Gizmos.DrawLine(waypoints[^1].position, waypoints[0].position);
        }
    }
}
