using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject TimeTrial;
    public GameObject StartScene;
    public GameObject Versus;
    
    void Start()
    {
          StartScene.SetActive(true);
        // Hide instructions panel on start
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
    }
    
    // Loads time trial mode
    public void LoadTimeTrial()
    { 
        TimeTrial.SetActive(true);

        StartScene.SetActive(false);
        Versus.SetActive(false);
    }

    // Loads time versus mode
    public void LoadVersus()
    {
        Versus.SetActive(true);
        
        StartScene.SetActive(false);
        TimeTrial.SetActive(false);
    }

    // Open the main menu 'StartScene'
     public void LoadMenu()
    {
        StartScene.SetActive(true);

        Versus.SetActive(false);
        TimeTrial.SetActive(false);
    }

    public void ToggleInstructions()
    {
        if (instructionsPanel != null)
        {
            bool isActive = instructionsPanel.activeSelf;
            instructionsPanel.SetActive(!isActive);
        }
    }
}
