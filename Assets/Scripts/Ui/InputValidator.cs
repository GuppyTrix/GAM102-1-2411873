using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputValidator : MonoBehaviour
{
    private string defaultValue = "0"; 

    public void ValidateInput(string input, TMP_InputField inputField)
    {
        if (int.TryParse(input, out _))
        {
        }
        else
        {
            Debug.LogWarning($"Invalid input for {inputField.name}. Resetting to default value.");
            inputField.text = defaultValue;
        }
    }
}
