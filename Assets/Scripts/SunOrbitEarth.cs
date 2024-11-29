using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunOrbitEarth : MonoBehaviour
{
    public GameObject Sun;  // Reference to the Sun GameObject.
    public GameObject Earth; // Reference to the Earth GameObject (center of orbit).

    public float sunOrbitRadius = 14f; // Radius of the Sun's orbit around the Earth.
    public float orbitSpeed = 10.0f; // Speed at which the Sun orbits the Earth.

    void Update()
    {
        OrbitSunAroundEarth(); // Perform the orbit logic every frame.
    }

    void OrbitSunAroundEarth()
    {
        // Calculate the orbit angle in degrees based on elapsed time and orbit speed.
        float angle = Time.time * orbitSpeed;

        // Convert the angle from degrees to radians for trigonometric calculations.
        float angleRad = Mathf.Deg2Rad * angle;

        // Compute the Sun's new position in 3D space using polar coordinates.
        float x = sunOrbitRadius * Mathf.Cos(angleRad); // X-coordinate using cosine.
        float z = sunOrbitRadius * Mathf.Sin(angleRad); // Z-coordinate using sine.

        // Update the Sun's position to follow the orbit path.
        Sun.transform.position = new Vector3(x, 0, z);
    }
}
