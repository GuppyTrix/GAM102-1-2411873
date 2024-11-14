using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 700f;

    public Transform cameraTransform; // Reference to the camera's transform
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input from WASD or arrow keys
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction relative to the camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Flatten the forward and right vectors to avoid tilting issues
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Calculate the direction based on input and camera orientation
        Vector3 direction = (forward * vertical + right * horizontal).normalized;

        // Check if the player is moving
        bool isMoving = direction.magnitude >= 0.1f;
        animator.SetBool("IsRunning", isMoving);

        if (isMoving)
        {
            // Move the player in the direction relative to the camera
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            // Rotate the player to face the movement direction
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
