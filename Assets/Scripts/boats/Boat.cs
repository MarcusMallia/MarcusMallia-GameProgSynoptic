using UnityEngine;

public class Boat : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float constantAcceleration = 0.2f;
    [SerializeField] private float maxDeceleration = 1f;
    private float currentDeceleration;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float currentSpeed = 0f;
    private bool lastKeyPressedLeft = false;
    private bool canMove = false;
    [SerializeField] private float passiveDeceleration = 0.1f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;

    [SerializeField] private float staminaDepletionRate = 10f;
    [SerializeField] private float baseRefillRate = 5f;

    public delegate void BoatStateHandler(float speed, float stamina);
    public static event BoatStateHandler OnBoatStateUpdated;
    
    public float ConstantAcceleration
    {
        get => constantAcceleration;
        private set => constantAcceleration = value;
    }

    public float MaxDeceleration
    {
        get => maxDeceleration;
        private set => maxDeceleration = value;
    }

    public float MaxSpeed
    {
        get => maxSpeed;
        private set => maxSpeed = value;
    }

    public float CurrentSpeed
    {
        get => currentSpeed;
        private set => currentSpeed = value;
    }

    public float PassiveDeceleration
    {
        get => passiveDeceleration;
        private set => passiveDeceleration = value;
    }

    public float MaxStamina
    {
        get => maxStamina;
        private set => maxStamina = value;
    }

    public float CurrentStamina
    {
        get => currentStamina;
        private set => currentStamina = value;
    }

    public float StaminaDepletionRate
    {
        get => staminaDepletionRate;
        private set => staminaDepletionRate = value;
    }

    public float BaseRefillRate
    {
        get => baseRefillRate;
        private set => baseRefillRate = value;
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
            Debug.Log("Speed: " + currentSpeed + ", Stamina: " + currentStamina + ", Deceleration: " + currentDeceleration);

            OnBoatStateUpdated?.Invoke(currentSpeed, currentStamina);
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
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

        currentSpeed = Mathf.Max(currentSpeed, 0);  // Ensure speed doesn't go negative
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

        // Apply resistance even during acceleration
        float effectiveAcceleration = constantAcceleration * (1 - resistance);
        
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
        float speedFactor = 1 - (currentSpeed / maxSpeed);  // Inverse relation to speed
        float modifiedRefillRate = baseRefillRate * speedFactor;

        currentStamina += modifiedRefillRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }

    float CalculateDeceleration(float stamina)
    {
        float normalizedStamina = stamina / maxStamina;
        return maxDeceleration * (1 - Mathf.Pow(normalizedStamina, 1.5f));
    }

    // TODO duplicate code
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("other.gameObject.CompareTag(FinishLine)" + other.gameObject.CompareTag("FinishLine"));
        if (other.gameObject.CompareTag("FinishLine"))
        {
            // Direct method call approach
            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.FinishTimer();
            }
        }
    }

    public void ResetBoat(Vector3 initialPosition, Quaternion initialRotation)
    {
        // Reset the boat's position and rotation
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Reset the boat's stats
        currentSpeed = 0;
        currentStamina = maxStamina;

        // Reset Rigidbody2D velocity
        // rb.velocity = Vector2.zero;
        // rb.angularVelocity = 0f;
    }
}