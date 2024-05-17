using UnityEngine;

public class Boat : MovementBehaviour
{
    public delegate void BoatStateHandler(float speed, float stamina);
    public static event BoatStateHandler OnBoatStateUpdated;

    protected override void Update()
    {
        base.Update();

        if (canMove)
        {
            OnBoatStateUpdated?.Invoke(currentSpeed, currentStamina);
        }
    }

    // Additional Boat-specific logic can be added here
    public void ResetBoat(Vector3 initialPosition, Quaternion initialRotation)
    {
        // Reset the boat's position and rotation
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Reset the boat's stats
        currentSpeed = 0;
        currentStamina = maxStamina;

        // // Reset Rigidbody2D velocity
        // rb.velocity = Vector2.zero;
        // rb.angularVelocity = 0f;
    }

    // Event handler for OnTriggerEnter2D
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
}
