using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    public float movementSpeed = 5f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (!controller)
        {
            Debug.LogError("CharacterController component is missing!");
        }
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 moveDir = Vector3.zero;

        // Get input and calculate movement relative to the player's forward direction
        if (Input.GetKey(KeyCode.W))
            moveDir += transform.forward;

        if (Input.GetKey(KeyCode.A))
            moveDir += -transform.right;

        if (Input.GetKey(KeyCode.D))
            moveDir += transform.right;

        if (Input.GetKey(KeyCode.S))
            moveDir += -transform.forward;

        // Normalize direction and apply movement speed
        moveDir = moveDir.normalized * movementSpeed;

        // Apply movement
        controller.SimpleMove(moveDir);

        // Update Animator parameters
        if (animator)
        {
            float moveMagnitude = moveDir.magnitude / movementSpeed;
            animator.SetFloat("Speed", moveMagnitude); // Assumes "Speed" parameter exists in Animator
        }
    }
}
