using UnityEngine;

public class BoatVersus : MovementBehaviour
{
    [SerializeField] private bool isPlayerOne = true;
    [SerializeField] private GameObject gameOverPanelV;

    public delegate void BoatStateHandler(float speed, float stamina);
    public static event BoatStateHandler OnBoatStateUpdatedPlayer1;
    public static event BoatStateHandler OnBoatStateUpdatedPlayer2;

    protected override void Start()
    {
        base.Start();
        currentStamina = maxStamina;
    }

    protected override void Update()
    {
        base.Update();
        if (canMove)
        {
            if (isPlayerOne)
            {
                OnBoatStateUpdatedPlayer1?.Invoke(currentSpeed, currentStamina);
            }
            else
            {
                OnBoatStateUpdatedPlayer2?.Invoke(currentSpeed, currentStamina);
            }
        }
    }

    protected override void HandleInput()
    {
        float resistance = CalculateResistance(currentStamina);
        float effectiveAcceleration = constantAcceleration * (1 - resistance);

        if (isPlayerOne)
        {
            HandlePlayerOneInput(effectiveAcceleration);
        }
        else
        {
            HandlePlayerTwoInput(effectiveAcceleration);
        }
    }

    private void HandlePlayerOneInput(float effectiveAcceleration)
    {
        if (currentStamina > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !lastKeyPressedLeft)
            {
                IncreaseSpeed(effectiveAcceleration);
                lastKeyPressedLeft = true;
                DecreaseStamina();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && lastKeyPressedLeft)
            {
                IncreaseSpeed(effectiveAcceleration);
                lastKeyPressedLeft = false;
                DecreaseStamina();
            }
        }
    }

    private void HandlePlayerTwoInput(float effectiveAcceleration)
    {
        if (currentStamina > 0)
        {
            if (Input.GetKeyDown(KeyCode.Z) && !lastKeyPressedLeft)
            {
                IncreaseSpeed(effectiveAcceleration);
                lastKeyPressedLeft = true;
                DecreaseStamina();
            }
            else if (Input.GetKeyDown(KeyCode.C) && lastKeyPressedLeft)
            {
                IncreaseSpeed(effectiveAcceleration);
                lastKeyPressedLeft = false;
                DecreaseStamina();
            }
        }
    }

    private float CalculateResistance()
    {
        return (1 - currentStamina / maxStamina) * maxDeceleration;
    }

    public void ResetBoat(Vector3 initialPosition, Quaternion initialRotation)
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        currentSpeed = 0;
        currentStamina = maxStamina;
    }

    private void FinishRace()
    {
        UIManagerVersus uiManager = FindObjectOfType<UIManagerVersus>();
        float raceTime = Time.time - uiManager.StartTime;
        uiManager.FinishTimer(isPlayerOne, raceTime);
        gameOverPanelV.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("FinishLinePlayer1") && isPlayerOne) || 
            (other.gameObject.CompareTag("FinishLinePlayer2") && !isPlayerOne))
        {
            FinishRace();
        }
    }
}
