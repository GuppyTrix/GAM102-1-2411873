using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class MultiReqInputs : MonoBehaviour
{
    public TMP_InputField[] andInputFields; // Fields where all inputs are required.
    public TMP_InputField[] xorInputFields; // Fields where at least one input is required.

    public void resetValues(TMP_InputField inputField)
    {
        // Clears the text in the specified input field.
        inputField.text = "";
    }

    public void CircleSearchValues()
    {
        // Validate all "AND" input fields.
        foreach (TMP_InputField andinp in andInputFields)
        {
            if (andinp.text.Length <= 0)
            {
                andinp.text = "*Required*"; // Set default message if input is empty.
            }
        }

        // Check "XOR" input fields to ensure at least one is filled.
        if (xorInputFields[0].text.Length <= 0 && xorInputFields[1].text.Length <= 0)
        {
            xorInputFields[0].text = "*At Least One Required*";
            xorInputFields[1].text = "*At Least One Required*";
        }
    }
}
