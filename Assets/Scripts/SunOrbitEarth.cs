using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunOrbitEarth : MonoBehaviour
{
    public GameObject Sun;  
    public GameObject Earth; 

    public float sunOrbitRadius = 14f; 
    public float orbitSpeed = 10.0f; 

    void Update()
    {
        OrbitSunAroundEarth();
    }

    void OrbitSunAroundEarth()
    {
        float angle = Time.time * orbitSpeed; 
        float angleRad = Mathf.Deg2Rad * angle;

        float x = sunOrbitRadius * Mathf.Cos(angleRad);
        float z = sunOrbitRadius * Mathf.Sin(angleRad);

        Sun.transform.position = new Vector3(x, 0, z);
    }
}

