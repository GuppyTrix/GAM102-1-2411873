using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller; // Reference to the CharacterController component.
    private Animator animator; // Reference to the Animator component for handling animations.

    public float movementSpeed = 5f; // Speed at which the player moves.

    private void Start()
    {
        // Fetch the CharacterController and Animator components.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Log an error if the CharacterController is missing.
        if (!controller)
        {
            Debug.LogError("CharacterController component is missing!");
        }
    }

    private void Update()
    {
        HandleMovement(); // Call movement handling logic every frame.
    }

    private void HandleMovement()
    {
        Vector3 moveDir = Vector3.zero; // Initialize movement direction.

        // Check for input and adjust the movement direction accordingly.
        if (Input.GetKey(KeyCode.W))
            moveDir += transform.forward; // Move forward relative to the player.

        if (Input.GetKey(KeyCode.A))
            moveDir += -transform.right; // Move left relative to the player.

        if (Input.GetKey(KeyCode.D))
            moveDir += transform.right; // Move right relative to the player.

        if (Input.GetKey(KeyCode.S))
            moveDir += -transform.forward; // Move backward relative to the player.

        // Normalize the movement direction to ensure consistent speed and scale it by the movement speed.
        moveDir = moveDir.normalized * movementSpeed;

        // Apply movement using the CharacterController's SimpleMove method.
        controller.SimpleMove(moveDir);

        // Update the Animator's "Speed" parameter based on movement magnitude.
        if (animator)
        {
            float moveMagnitude = moveDir.magnitude / movementSpeed; // Normalize magnitude for the Animator.
            animator.SetFloat("Speed", moveMagnitude); // Assumes "Speed" exists as a float parameter in the Animator.
        }
    }
}
