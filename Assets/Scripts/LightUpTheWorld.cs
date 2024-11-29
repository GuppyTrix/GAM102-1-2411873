using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpTheWorld : MonoBehaviour
{
    public GameObject environmentParent;

    private void Awake()
    {
        // Disable the debug light
        this.gameObject.GetComponent<Light>().enabled = false;

        // Disable all the mesh renderers for the floor and walls
        foreach (MeshRenderer item in environmentParent.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {

            // Enable the debug light
            this.gameObject.GetComponent<Light>().enabled = !this.gameObject.GetComponent<Light>().isActiveAndEnabled;

            // Enable all the mesh renderers for the floor and walls
            foreach (MeshRenderer item in environmentParent.GetComponentsInChildren<MeshRenderer>())
            {
                item.enabled = !item.enabled;
            }
        }
        
    }

}
