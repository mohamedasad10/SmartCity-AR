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
