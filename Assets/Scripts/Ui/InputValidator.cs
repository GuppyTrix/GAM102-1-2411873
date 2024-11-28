using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Windows;
using System;

public class InputValidator : MonoBehaviour
{
    public Func<string, bool> validateFunc; // Validation logic
    //public string defaultValue; // Value to reset to if validation fails
    public string validationRule;

    private TMP_InputField inputField;

    private void Awake()
    {
        if (validationRule.Length <= 0)
        {
            Debug.LogError($"Unset validationRule for {this.name}");
        }

        inputField = GetComponent<TMP_InputField>();
        if (inputField != null)
            Debug.Log("adding listener");
            inputField.onEndEdit.AddListener(OnEndEditValidateInput);
            inputField.onValueChanged.AddListener(OnValueChangedValidateInput);
    }

    public string GetValidationRule()
    {
        return validationRule;
    }

    public void OnEndEditValidateInput(string input)
    {
        input = inputField.text;
        if (validateFunc != null && !validateFunc(input))
        {
            Debug.LogWarning($"Invalid input for {inputField.name}: {input}");
            inputField.text = null; // Reset to a default value
        }
    }

    public void OnValueChangedValidateInput(string input)
    {
        input = inputField.text; 

        if (validateFunc != null && !validateFunc(input))
        {
            inputField.textComponent.color = Color.red;
        }
        else
        {
            inputField.textComponent.color = Color.black;
        }
    }
}
