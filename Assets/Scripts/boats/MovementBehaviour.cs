using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField] protected float constantAcceleration = 0.2f;
    [SerializeField] protected float maxDeceleration = 1f;
    protected float currentDeceleration;
    [SerializeField] protected float maxSpeed = 5f;
    [SerializeField] protected float currentSpeed = 0f;
    protected bool lastKeyPressedLeft = false;
    protected bool canMove = false;
    [SerializeField] protected float passiveDeceleration = 0.1f;
    [SerializeField] protected float maxStamina = 100f;
    [SerializeField] protected float currentStamina;

    [SerializeField] protected float staminaDepletionRate = 10f;
    [SerializeField] protected float baseRefillRate = 5f;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina;
    }

    protected virtual void Update()
    {
        if (canMove)
        {
            HandleInput();
            RefillStamina();

            float currentDeceleration = CalculateDeceleration(currentStamina);

            Debug.Log("Speed: " + currentSpeed + ", Stamina: " + currentStamina + ", Deceleration: " + currentDeceleration);

            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        }
    }

    protected virtual void FixedUpdate()
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
            currentSpeed -= currentDynamicDeceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.Max(currentSpeed, 0);
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    protected virtual void HandleInput()
    {
        float resistance = CalculateResistance(currentStamina);
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

    protected void ApplyPassiveDeceleration()
    {
        currentSpeed -= passiveDeceleration * Time.deltaTime;
    }

    protected float CalculateResistance(float currentStamina)
    {
        return (1 - currentStamina / maxStamina) * maxDeceleration;
    }

    protected void IncreaseSpeed(float acceleration)
    {
        currentSpeed += acceleration;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    protected void DecreaseStamina()
    {
        currentStamina -= staminaDepletionRate * Time.deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0);
    }

    protected void RefillStamina()
    {
        float speedFactor = 1 - (currentSpeed / maxSpeed);
        float modifiedRefillRate = baseRefillRate * speedFactor;

        currentStamina += modifiedRefillRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }

    protected float CalculateDeceleration(float stamina)
    {
        float normalizedStamina = stamina / maxStamina;
        return maxDeceleration * (1 - Mathf.Pow(normalizedStamina, 1.5f));
    }

    public void EnableMovement()
    {
        canMove = true;
    }
    
    public void DisableMovement()
    {
        canMove = false;
    }
}
