using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject StartScene;
    public GameObject TimeTrial;
    public GameObject Versus;
    public GameObject Boat;
    public GameObject GameOverPanel;
    private float currentSpeed = 0f;
    public Animator animator;
    private Rigidbody2D rb;

    public static GameStateManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }
 
    private Vector3 boatInitialPosition = new Vector3(0.1099998f, 0.0200001f, 0f);
    private Quaternion boatInitialRotation = Quaternion.Euler(0f, 0f, 180.93f);

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

    void Start()
    {
          StartScene.SetActive(true);
          ChangeState(GameState.MainMenu);
        // Hide instructions panel on start
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
        rb = GetComponent<Rigidbody2D>();
      
    }

    public void ToggleInstructions()
    {
        if (instructionsPanel != null)
        {
            bool isActive = instructionsPanel.activeSelf;
            instructionsPanel.SetActive(!isActive);
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
                GameOverPanel.SetActive(false);
                StartScene.SetActive(false);
                Versus.SetActive(false);
                break;
            case GameState.GameOver:
                animator.SetTrigger("GameOver");
                GameOverPanel.SetActive(true);
                StartScene.SetActive(false);
                Versus.SetActive(false);
                TimeTrial.SetActive(false);
                break;
            
        }
    }
    // Loads time trial mode
    public void LoadTimeTrial()
    { 
        ChangeState(GameState.TimeTrial);
        Boat.GetComponent<Boat>().ResetBoat(boatInitialPosition, boatInitialRotation);

            RaceStarter raceStarter = FindObjectOfType<RaceStarter>();
                raceStarter.Start();
                raceStarter.Update();
                raceStarter.ResetRace();
    
            if (GameOverPanel != null)
            {
                GameOverPanel.SetActive(false);
            }

    }

    // Loads versus mode
    public void LoadVersus()
    {
         ChangeState(GameState.Versus);
    }

    // Open the main menu 'StartScene'
     public void LoadMenu()
    {
        ChangeState(GameState.MainMenu);
    }
    

    // Open the GameOverPanel
    public void GameOver()
    {
             ChangeState(GameState.GameOver);
    
            // Direct method call approach
            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.FinishTimer();
            }
        }

    public void RestartGame()
    {
           Boat.GetComponent<Boat>().ResetBoat(boatInitialPosition, boatInitialRotation);
            
            ChangeState(GameState.TimeTrial);

            RaceStarter raceStarter = FindObjectOfType<RaceStarter>();
            // if (raceStarter != null)
            // {
                raceStarter.Start();
                raceStarter.Update();
                raceStarter.ResetRace();

            // }
    
            if (GameOverPanel != null)
            {
                GameOverPanel.SetActive(false);
            }

            
         
    }
}
        
    

