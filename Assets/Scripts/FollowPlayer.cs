using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // Assign the player object here
    public Vector3 offset; // Set an appropriate offset in the Inspector

    private void LateUpdate()
    {
        if (playerTransform)
        {
            // Update camera position based on player's position + offset
            transform.position = playerTransform.position + offset;
        }
    }
}
