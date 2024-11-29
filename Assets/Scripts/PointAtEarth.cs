using UnityEngine;

public class PointAtEarth : MonoBehaviour
{
    public GameObject target; // The GameObject this object will point towards.

    void Update()
    {
        if (target != null) // Ensure the target is set before performing calculations.
        {
            // Calculate the direction vector pointing from this object to the target.
            Vector3 direction = target.transform.position - transform.position;

            // Compute the desired rotation to look at the target.
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Apply the rotation to this object.
            transform.rotation = targetRotation;
        }
    }
}
