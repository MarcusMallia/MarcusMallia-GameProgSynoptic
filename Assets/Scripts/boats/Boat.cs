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

    public void ResetBoat(Vector3 initialPosition, Quaternion initialRotation)
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        currentSpeed = 0;
        currentStamina = maxStamina;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FinishLine"))
        {
            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.FinishTimer();
            }
        }
    }
}
