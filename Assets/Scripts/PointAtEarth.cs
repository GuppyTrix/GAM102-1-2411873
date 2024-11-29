using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtEarth : MonoBehaviour
{
    public GameObject target; // The GameObject to point at

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}

