using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform boatTransform;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    public Transform BoatTransform
    {
        get => boatTransform;
        private set => boatTransform = value;
    }

    public float SmoothSpeed
    {
        get => smoothSpeed;
        private set => smoothSpeed = value;
    }

    public Vector3 Offset
    {
        get => offset;
        private set => offset = value;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(boatTransform.position.x + offset.x, transform.position.y, offset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
