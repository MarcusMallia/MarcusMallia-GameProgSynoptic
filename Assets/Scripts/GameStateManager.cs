using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        // Handle state-specific behavior
        switch (newState)
        {
            case GameState.MainMenu:
                LoadMainMenu();
                break;
            case GameState.TimeTrial:
                StartTimeTrial();
                break;
            case GameState.TimeTrialGameOver:
                ShowTimeTrialGameOver();
                break;
            case GameState.Versus:
                StartVersus();
                break;
            case GameState.VersusGameOver:
                ShowVersusGameOver();
                break;
        }
    }

    private void LoadMainMenu()
    {
        // Load Main Menu logic
    }

    private void StartTimeTrial()
    {
        // Start Time Trial logic
    }

    private void ShowTimeTrialGameOver()
    {
        // Show Time Trial Game Over logic
    }

    private void StartVersus()
    {
        // Start Versus logic
    }

    private void ShowVersusGameOver()
    {
        // Show Versus Game Over logic
    }
}
