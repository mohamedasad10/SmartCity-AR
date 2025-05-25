/*
 * This script allows toggling the visibility of multiple electricity-related GameObjects (e.g. power lines, substations) in your AR scene.
 * 
 * 📌 How It Works:
 * - Each time the linked button is clicked, the script toggles the `SetActive` state of all assigned objects in the `electricityObjects` array.
 * 
 * 🧩 Setup Instructions:
 * 1. Attach this script to a UI GameObject, such as the button itself or a UI manager.
 * 2. In the Inspector, assign all electricity-related GameObjects (like wires, poles, lights, etc.) to the `electricityObjects` array.
 * 3. On your UI Button’s OnClick() event:
 *      - Drag in the GameObject that holds this script.
 *      - Select the method: `ElectricityToggle → ToggleElectricity()`
 * 4. Ensure the electricity objects are active at the start if you want them to be visible initially.
 */

using UnityEngine;

public class ElectricityToggle : MonoBehaviour
{
    public GameObject[] electricityObjects; // Assign all electricity-related GameObjects here
    private bool isVisible = true;

    public void ToggleElectricity()
    {
        isVisible = !isVisible;

        foreach (GameObject obj in electricityObjects)
        {
            if (obj != null)
            {
                obj.SetActive(isVisible);
            }
        }
    }
}
