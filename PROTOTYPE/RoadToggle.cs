/*
 * RoadToggle.cs
 * 
 * DESCRIPTION:
 * This script toggles the visibility of road GameObjects in your AR scene when a UI button is clicked.
 * It flips the visibility state (on/off) each time the button is pressed.
 *
 * SETUP INSTRUCTIONS:
 * - Attach this script to the UI Button GameObject that will control the road visibility.
 * - In the Inspector, assign the "roads" GameObject — this should be the parent object that contains all road meshes.
 * - Ensure the roads GameObject is active by default (if you want it visible on app start).
 * - Make sure the button component is present and properly linked to this script.
 */


using UnityEngine;
using UnityEngine.UI;

public class RoadToggle : MonoBehaviour
{
    public GameObject roads; // The parent object that contains all road meshes
    private bool roadsVisible = true;  // Track visibility state

    private Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ToggleRoads);
    }

    void ToggleRoads()
    {
        roadsVisible = !roadsVisible;         // Flip the state
        roads.SetActive(roadsVisible);        // Show/hide roads based on new state
    }
}
