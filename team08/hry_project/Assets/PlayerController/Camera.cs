using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;  // Reference to the player's transform
    [SerializeField] private float smoothSpeed = 0.125f; // Smoothing factor for camera movement
    [SerializeField] private Vector3 offset;             // Offset from the player

    private void LateUpdate()
    {
        // Keep the camera's original Z position
        Vector3 desiredPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z) + offset;

        // Smoothly interpolate between the current camera position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera position
        transform.position = smoothedPosition;
    }
}
