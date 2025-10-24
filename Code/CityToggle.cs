using UnityEngine;
using UnityEngine.UI;

public class CityToggle : MonoBehaviour
{
    public GameObject sustainableCity;     // The sustainable city GameObject in the scene
    public GameObject unsustainableCity;   // The unsustainable city GameObject in the scene

    private bool showingSustainable = true;  // Start with sustainable shown
    private Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ToggleCity);

        // Ensure initial state is correct
        UpdateCityView();
    }

    void ToggleCity()
    {
        // Flip the state
        showingSustainable = !showingSustainable;

        // Update which city is active
        UpdateCityView();
    }

    void UpdateCityView()
    {
        // Activate/deactivate the cities instead of destroying/instantiating
        if (sustainableCity != null)
            sustainableCity.SetActive(showingSustainable);

        if (unsustainableCity != null)
            unsustainableCity.SetActive(!showingSustainable);
    }
}
