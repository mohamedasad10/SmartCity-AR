using UnityEngine;
using UnityEngine.UI;

public class RoadToggle : MonoBehaviour
{
    public GameObject[] roads; // Drag multiple roads here in the Inspector
    private bool roadsVisible = true;  // Track visibility state

    private Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ToggleRoads);
    }

    void ToggleRoads()
    {
        roadsVisible = !roadsVisible;   // Flip the state

        // Loop through all assigned roads and toggle them
        foreach (GameObject road in roads)
        {
            if (road != null)
                road.SetActive(roadsVisible);
        }
    }
}
