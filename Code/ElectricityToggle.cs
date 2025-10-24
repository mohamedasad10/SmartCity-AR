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
