using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // Reference to the player's body for rotation
    private float xRotation = 0f;

    void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    // Optionally, pause the game or toggle cursor lock here if desired.
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = true;
        //}
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player body based on horizontal mouse movement
        playerBody.Rotate(Vector3.up * mouseX);

        // Apply vertical rotation to the camera, with clamping to prevent flipping
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit up/down rotation to prevent camera flipping
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
