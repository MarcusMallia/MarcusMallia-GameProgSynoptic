using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerVersus : MonoBehaviour
{
    // UI elements for Player 1
    
    [SerializeField] private Text speedTextPlayer1;
    [SerializeField] private Slider staminaSliderPlayer1;

    // UI elements for Player 2
    [SerializeField] private Text speedTextPlayer2;
    [SerializeField] private Slider staminaSliderPlayer2;

    [SerializeField] private Text highScoreText;
    [SerializeField] private Text timerText;         
    [SerializeField] private GameObject gameOverPanel; 
    [SerializeField] private Text finalTimeText;

    private float startTime;
    private bool timerRunning = false;
    private float finalRaceTime;
    private const string HighScoreKey = "HighScore";

    
    public delegate void BoatStateHandlerPlayer1(float speed, float stamina);
    public static event BoatStateHandlerPlayer1 OnBoatStateUpdatedPlayer1;

    public delegate void BoatStateHandlerPlayer2(float speed, float stamina);
    public static event BoatStateHandlerPlayer2 OnBoatStateUpdatedPlayer2;
    
    public bool RaceFinished { get; set; } 
    public float StartTime => startTime;
    void OnEnable()
    {
        BoatVersus.OnBoatStateUpdatedPlayer1 += UpdatePlayerOneUI;
        BoatVersus.OnBoatStateUpdatedPlayer2 += UpdatePlayerTwoUI;
    }

    void OnDisable()
    {
        BoatVersus.OnBoatStateUpdatedPlayer1 -= UpdatePlayerOneUI;
        BoatVersus.OnBoatStateUpdatedPlayer2 -= UpdatePlayerTwoUI;
    }

    void UpdatePlayerOneUI(float speed, float stamina)
    {
        UpdatePlayerUI(speedTextPlayer1, staminaSliderPlayer1, speed, stamina);
    }

    void UpdatePlayerTwoUI(float speed, float stamina)
    {
        UpdatePlayerUI(speedTextPlayer2, staminaSliderPlayer2, speed, stamina);
    }

    void Start()
    {
         if (gameOverPanel != null) gameOverPanel.SetActive(false);
        // Initialize stamina sliders for both players
        InitializeStaminaSlider(staminaSliderPlayer1);
        InitializeStaminaSlider(staminaSliderPlayer2);

        // Hide Game Over Panel initially
       
    }

    void InitializeStaminaSlider(Slider slider)
    {
        if (slider != null)
        {
            slider.maxValue = 100;
            slider.value = 100;
        }
    }

    void Update()
    {
        if (timerRunning)
        {
            UpdateTimer();
        }
    }

    // Update the UI for both players
    void UpdateBoatUI(float speed1, float stamina1, float speed2, float stamina2)
    {
        UpdatePlayerUI(speedTextPlayer1, staminaSliderPlayer1, speed1, stamina1);
        UpdatePlayerUI(speedTextPlayer2, staminaSliderPlayer2, speed2, stamina2);
    }

    void UpdatePlayerUI(Text speedText, Slider staminaSlider, float speed, float stamina)
    {
        if (speedText != null)
        {
            speedText.text = "Speed: " + (speed * 10).ToString("0");
        }

        if (staminaSlider != null)
        {
            staminaSlider.value = stamina;
        }
    }
    
    public void StartTimer()
    {
        startTime = Time.time;
        timerRunning = true;
    }

    public void UpdateTimer()
    {
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        if (timerText != null)
        {
            timerText.text = minutes + ":" + seconds;
        }
    }

    public void FinishTimer(bool isPlayerOneWinner, float winningTime)
    {
        Debug.Log ("RaceFinished" + RaceFinished);
        if (!RaceFinished) // Check if the race is already finished
        {
            RaceFinished = true;
            timerRunning = false;
            finalRaceTime = winningTime;
            ShowGameOverScreen(isPlayerOneWinner, winningTime);
            if (timerText != null)
            {
                timerText.color = Color.yellow;
            }
        }
    }

    public void ShowGameOverScreen(bool isPlayerOneWinner, float winningTime)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            finalTimeText.text = (isPlayerOneWinner ? "Player 1" : "Player 2") + " Wins !" + "\nTime: " + FormatTime(winningTime);
        }
    }
    
    
    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        float seconds = time % 60;
        return string.Format("{0:00}:{1:00.00}", minutes, seconds);
    }
    
   
   public void RestartGame()
    {
      if (startTime != null)
    {
        startTime = Time.time;
    }
    if (speedTextPlayer1 != null)  
    {
        speedTextPlayer1.text = "Speed: 0";
    }
    if (speedTextPlayer2 != null)  
    {
        speedTextPlayer2.text = "Speed: 0";   
    }
    if (staminaSliderPlayer1 != null)  
    {
        staminaSliderPlayer1.value = staminaSliderPlayer1.maxValue;
    }
     if (staminaSliderPlayer2 != null)  
    {
        staminaSliderPlayer2.value = staminaSliderPlayer2.maxValue;
    }

    }


}
