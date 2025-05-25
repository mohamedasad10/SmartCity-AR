
//   What This Script Does:
// - Toggles the visibility (active state) of the water GameObject when a button is clicked.
// - Useful for showing or hiding water in an AR scene or city model.
// 
// 📌 How to Use:
// 1. Attach this script to any active GameObject in your scene (e.g., an empty "UIManager" or the button itself).
// 2. In the Inspector, assign the water GameObject to the "water" field.
// 3. In your UI Button’s OnClick() list, drag in the GameObject with this script.
// 4. From the dropdown, select `WaterToggle → ToggleWater()`.
// 5. Make sure the water GameObject is active by default so it can be toggled off.
// ==========================================

using UnityEngine;

public class WaterToggle : MonoBehaviour
{
    public GameObject water; // Assign the water GameObject in the Inspector
    private bool isVisible = true;

    public void ToggleWater()
    {
        if (water != null)
        {
            isVisible = !isVisible;
            water.SetActive(isVisible);
        }
    }
}
