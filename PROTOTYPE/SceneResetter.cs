//============================================================
// SceneResetter.cs
//
// Description:
// This utility script reloads the current active scene,
// effectively resetting the entire AR experience.
//
// Setup Instructions:
// 1. Attach this script to any GameObject in your scene 
//    (e.g., an empty "SceneManager" GameObject).
// 2. Link the `ResetScene()` function to a UI Button's OnClick event 
//    via the Inspector.
//    - To do this: 
//      - Select your Button in the Hierarchy.
//      - In the Inspector, scroll to the OnClick() section.
//      - Click the "+" button.
//      - Drag the GameObject with this script attached into the object field.
//      - Choose `SceneResetter -> ResetScene()` from the dropdown.
//
// Use Case:
// Used to reset AR content, restart simulations, or reload 
// the environment cleanly during testing or in final applications.
//
//============================================================

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetter : MonoBehaviour
{
    public void ResetScene()
    {
        // Reload the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
