using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWheel : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;  // Assign your player camera in the Inspector
    [SerializeField] private float zoomSpeed = 2f; // Speed of zoom
    [SerializeField] private float minZoom = 2f;   // Minimum zoom limit
    [SerializeField] private float maxZoom = 10f;  // Maximum zoom limit

    void Update()
    {
        HandleZoom();
    }

    private void HandleZoom()
    {
        // Get the scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Modify the camera's orthographic size for zoom (for 2D) or field of view (for 3D)
        playerCamera.orthographicSize -= scrollInput * zoomSpeed;

        // Clamp the zoom to stay within min and max bounds
        playerCamera.orthographicSize = Mathf.Clamp(playerCamera.orthographicSize, minZoom, maxZoom);
    }
}
