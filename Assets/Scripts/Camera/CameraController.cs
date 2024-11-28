using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public Transform playerTransform; // Assign the player GameObject here
    public float mouseSensitivity = 100f;
    public float verticalClampAngle = 80f;
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;

    private float verticalRotation = 0f;

    private bool cameraCanMove = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        // Lock the cursor for better camera control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        HandleCameraRotation();
        FollowPlayer();
    }

    public void ToggleCameraMove()
    {
        cameraCanMove = !cameraCanMove;
    }
    public void SetCameraMove(bool state)
    {
        cameraCanMove = state;
    }

    private void HandleCameraRotation()
    {
        if (!cameraCanMove)
        {
            return;
        }

        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player horizontally
        playerTransform.Rotate(Vector3.up * mouseX);

        // Handle vertical rotation, clamped to avoid flipping
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalClampAngle, verticalClampAngle);

        // Apply vertical rotation to the camera
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void FollowPlayer()
    {
        // Calculate the desired position behind the player
        Vector3 cameraOffset = -playerTransform.forward * cameraDistance + Vector3.up * cameraHeight;

        // Move the camera to the desired position
        transform.position = playerTransform.position + cameraOffset;

        // Ensure the camera faces the same direction as the player
        transform.rotation = Quaternion.Euler(verticalRotation, playerTransform.eulerAngles.y, 0f);
    }
}
