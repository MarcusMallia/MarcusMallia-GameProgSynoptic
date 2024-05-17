using UnityEngine;

public class BoatVersus : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float constantAcceleration = 0.2f; // Constant acceleration when a key is pressed
    [SerializeField] private float maxDeceleration = 1f; // Maximum deceleration when stamina is 0
    [SerializeField] private float currentDeceleration;
    [SerializeField] private float maxSpeed = 5f; // Theoretical maximum speed
    private float currentSpeed = 0f;
    private bool lastKeyPressedLeft = false;
    private bool canMove = false;
    [SerializeField] private float passiveDeceleration = 0.1f;
    [SerializeField] private GameObject gameOverPanelV;

    [SerializeField] private bool isPlayerOne = true;

    // Stamina variables
    [SerializeField] private float maxStamina = 100f;
    private float currentStamina;

    // Stamina depletion and refill rates
    [SerializeField] private float staminaDepletionRate = 10f;
    [SerializeField] private float baseRefillRate = 5f;

    public delegate void BoatStateHandler(float speed, float stamina);
    public static event BoatStateHandler OnBoatStateUpdatedPlayer1;
    public static event BoatStateHandler OnBoatStateUpdatedPlayer2;

    public float ConstantAcceleration
    {
        get => constantAcceleration;
        set => constantAcceleration = value;
    }

    public float MaxDeceleration
    {
        get => maxDeceleration;
        set => maxDeceleration = value;
    }

    public float CurrentDeceleration
    {
        get => currentDeceleration;
        set => currentDeceleration = value;
    }

    public float MaxSpeed
    {
        get => maxSpeed;
        set => maxSpeed = value;
    }

    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = value;
    }

    public bool LastKeyPressedLeft
    {
        get => lastKeyPressedLeft;
        set => lastKeyPressedLeft = value;
    }

    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }

    public float PassiveDeceleration
    {
        get => passiveDeceleration;
        set => passiveDeceleration = value;
    }

    public GameObject GameOverPanelV
    {
        get => gameOverPanelV;
        set => gameOverPanelV = value;
    }

    public bool IsPlayerOne
    {
        get => isPlayerOne;
        set => isPlayerOne = value;
    }

    public float MaxStamina
    {
        get => maxStamina;
        set => maxStamina = value;
    }

    public float CurrentStamina
    {
        get => currentStamina;
        set => currentStamina = value;
    }

    public float StaminaDepletionRate
    {
        get => staminaDepletionRate;
        set => staminaDepletionRate = value;
    }

    public float BaseRefillRate
    {
        get => baseRefillRate;
        set => baseRefillRate = value;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina;
       
    }

    void Update()
    {
        if (canMove)
        {
            HandleInput();
            RefillStamina();

            // Calculate deceleration for logging
            float currentDeceleration = CalculateDeceleration(currentStamina);

            // Log current speed, stamina, and deceleration
            Debug.Log("Speed: " + currentSpeed + ", Stamina: " + currentStamina + ", Deceleration: " +
                      currentDeceleration);


            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

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

    void FixedUpdate()
    {
        currentDeceleration = CalculateDeceleration(currentStamina);
        if (currentSpeed > 0 && !Input.anyKey)
        {
            currentSpeed -= currentDeceleration * Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, 0);
        }

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

        float currentDynamicDeceleration = CalculateDeceleration(currentStamina);

        if (!Input.anyKey)
        {
            ApplyPassiveDeceleration();
        }
        else
        {
            // Apply dynamic deceleration during player input
            currentSpeed -= currentDynamicDeceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.Max(currentSpeed, 0); // Ensure speed doesn't go negative
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    void ApplyPassiveDeceleration()
    {
        // Apply passive deceleration when there is no player input
        currentSpeed -= passiveDeceleration * Time.deltaTime;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    void HandleInput()
    {
        float resistance = CalculateResistance(currentStamina);
        float effectiveAcceleration = constantAcceleration * (1 - resistance);

        // Differentiate input handling based on whether it's Player 1 or Player 2
        if (isPlayerOne)
        {
            HandlePlayerOneInput(effectiveAcceleration);
            OnBoatStateUpdatedPlayer1?.Invoke(currentSpeed, currentStamina);
        }
        else
        {
            HandlePlayerTwoInput(effectiveAcceleration);
            OnBoatStateUpdatedPlayer2?.Invoke(currentSpeed, currentStamina);
        }
    }

    void HandlePlayerOneInput(float effectiveAcceleration)
    {
        // Player 1 controls using Left and Right Arrows
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

    void HandlePlayerTwoInput(float effectiveAcceleration)
    {
        // Player 2 controls using Z and C keys
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

    float CalculateResistance(float currentStamina)
    {
        // Example: Inversely proportional to stamina
        return (1 - currentStamina / maxStamina) * maxDeceleration;
    }

    void IncreaseSpeed(float acceleration)
    {
        currentSpeed += acceleration;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    void DecreaseStamina()
    {
        currentStamina -= staminaDepletionRate * Time.deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0);
    }

    void RefillStamina()
    {
        float speedFactor = 1 - (currentSpeed / maxSpeed); // Inverse relation to speed
        float modifiedRefillRate = baseRefillRate * speedFactor;

        currentStamina += modifiedRefillRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }

    float CalculateDeceleration(float stamina)
    {
        float normalizedStamina = stamina / maxStamina;
        return maxDeceleration * (1 - Mathf.Pow(normalizedStamina, 1.5f));
    }
    
    void FinishRace()
    {
        UIManagerVersus uiManager = FindObjectOfType<UIManagerVersus>();

        Debug.Log("fishline");

        float raceTime = Time.time - uiManager.StartTime;
        uiManager.FinishTimer(isPlayerOne, raceTime);
        GameOverPanelV.SetActive(true);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        FinishRace();
    }

    // TODO remove extra boat
    public void ResetBoat(Vector3 playerOneBoatInitialPosition, Quaternion playerOneBoatInitialRotation)
    {
        // Reset the boat's position and rotation
        transform.position = playerOneBoatInitialPosition;
        transform.rotation = playerOneBoatInitialRotation;
        
        // Reset the boat's stats
        currentSpeed = 0;
        currentStamina = maxStamina;

        // Reset Rigidbody2D velocity
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public void ResetBoat2(Vector3 playerTwoBoatInitialPosition, Quaternion playerTwoBoatInitialRotation)
    {
        // Reset the boat's position and rotation
        transform.position = playerTwoBoatInitialPosition;
        transform.rotation = playerTwoBoatInitialRotation;
        

        // Reset the boat's stats
        currentSpeed = 0;
        currentStamina = maxStamina;

        // Reset Rigidbody2D velocity
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    
}
    

        
    
    