using System.Collections.Generic;
using UnityEngine;

public class CarPool : MonoBehaviour
{
    public GameObject carPrefab;
    public int initialSize = 20;
    public bool expandable = true;

    readonly Queue<GameObject> _pool = new Queue<GameObject>();

    void Awake()
    {
        if (carPrefab == null)
        {
            Debug.LogError("CarPool: Assign a carPrefab.");
            enabled = false;
            return;
        }

        Prewarm(initialSize);
    }

    void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(carPrefab, transform);
            go.SetActive(false);
            _pool.Enqueue(go);
        }
    }

    public GameObject Get()
    {
        if (_pool.Count == 0)
        {
            if (!expandable)
                return null;
            Prewarm(1);
        }
        var go = _pool.Dequeue();
        go.SetActive(true);
        return go;
    }

    public void Return(GameObject go)
    {
        if (go == null) return;
        go.SetActive(false);
        go.transform.SetParent(transform, false);
        _pool.Enqueue(go);
    }
}
