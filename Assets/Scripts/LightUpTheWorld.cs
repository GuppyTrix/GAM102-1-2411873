using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpTheWorld : MonoBehaviour
{
    public GameObject environmentParent;

    private void Awake()
    {
        this.gameObject.GetComponent<Light>().enabled = false;
        foreach (MeshRenderer item in environmentParent.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("doing smth");
            this.gameObject.GetComponent<Light>().enabled = !this.gameObject.GetComponent<Light>().isActiveAndEnabled;
            foreach (MeshRenderer item in environmentParent.GetComponentsInChildren<MeshRenderer>())
            {
                item.enabled = !item.enabled;
            }
        }
        
    }

}
