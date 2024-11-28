using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

public class MultiReqInputs : MonoBehaviour
{
    public TMP_InputField[] andInputFields;
    public TMP_InputField[] xorInputFields;

    private void Awake()
    {
    }


    public void resetValues(TMP_InputField inputField)
    {
        inputField.text = "";
    }

    public void CircleSearchValues()
    {
        foreach (TMP_InputField andinp  in andInputFields)
        {
            if (andinp.text.Length <= 0)
            {
                andinp.text = "*Required*";
            }
        }

        if (xorInputFields[0].text.Length <= 0 && xorInputFields[1].text.Length <= 0)
        {
            xorInputFields[0].text = "*At Least One Required*";
            xorInputFields[1].text = "*At Least One Required*";
        }

    }

}
