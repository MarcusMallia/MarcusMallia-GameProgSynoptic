using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameObject instructionsPanel;

    public GameObject StartScene;
    public GameObject TimeTrial;
    public GameObject Versus;

    public GameObject Boat;

    public Animator animator;
    
    public static GameStateManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }

    void Start()
    {
          StartScene.SetActive(true);
          ChangeState(GameState.MainMenu);
        // Hide instructions panel on start
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            animator = GetComponent<Animator>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        HandleStateChange(newState);
    }
    
    private void HandleStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                animator.SetTrigger("MainMenu");
                StartScene.SetActive(true);
                Versus.SetActive(false);
                TimeTrial.SetActive(false);
                break;
            case GameState.Versus:
                animator.SetTrigger("LoadVersus");
                Versus.SetActive(true);        
                StartScene.SetActive(false);
                TimeTrial.SetActive(false);
                break;
            case GameState.TimeTrial:
                animator.SetTrigger("TimeTrial");
                TimeTrial.SetActive(true);
                StartScene.SetActive(false);
                Versus.SetActive(false);
                break;
            // case GameState.GameOver:
            //     animator.SetTrigger("GameOver");
            //     break;
        }
    }
    // Loads time trial mode
    public void LoadTimeTrial()
    { 
        ChangeState(GameState.TimeTrial);
    }

    // Loads versus mode
    public void LoadVersus()
    {
         ChangeState(GameState.Versus);
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
