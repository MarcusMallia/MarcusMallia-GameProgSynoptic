using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RaceStarter : MonoBehaviour
{
    [SerializeField] private Text startText; 
    [SerializeField] private Text countdownText; 
    private UIManager uiManager; 
    private Boat playerBoat;
    private bool raceStarted = false;

    public Text StartText
    {
        get => startText;
        private set => startText = value;
    }

    public Text CountdownText
    {
        get => countdownText;
        private set => countdownText = value;
    }

    public bool RaceStarted
    {
        get => raceStarted;
        private set => raceStarted = value;
    }

   public void Start()
    {
       
        uiManager = FindObjectOfType<UIManager>(); 
        if (startText != null) 
        {
            startText.gameObject.SetActive(true); 
        }
        if (countdownText != null) 
        {
            countdownText.gameObject.SetActive(false); 
        }
        
        playerBoat = FindObjectOfType<Boat>();
        if (playerBoat != null)
        {
            playerBoat.DisableMovement();
        }
    }

    public void Update()
    {
        if (!raceStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StartCountdown());
            if (startText != null) 
            {
                startText.gameObject.SetActive(false); // Hide the start text
            }
        }
    }

    IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSeconds(1);

        countdownText.text = "2";
        yield return new WaitForSeconds(1);

        countdownText.text = "1";
        yield return new WaitForSeconds(1);

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1);

        countdownText.gameObject.SetActive(false);
        StartRace();
        
        if (playerBoat != null)
        {
            playerBoat.EnableMovement();
        }
    }

    void StartRace()
    {
        if (uiManager != null)
        {
            uiManager.StartTimer(); // Start the timer via UIManager
        }
        raceStarted = true;
    }

    public void ResetRace()
    {
        raceStarted = false; 
    }

    public bool IsRaceStarted()
    {
        return raceStarted;
    }


}