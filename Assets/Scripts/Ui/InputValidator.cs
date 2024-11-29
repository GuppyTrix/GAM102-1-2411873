using UnityEngine;
using TMPro;
using System;

public class InputValidator : MonoBehaviour
{
    public Func<string, bool> validateFunc; // Delegate function to define custom validation logic.
    public string validationRule; // Name of the validation rule to apply.

    private TMP_InputField inputField; // Reference to the TextMeshPro input field.

    private void Awake()
    {
        // Check if a validation rule is set.
        if (validationRule.Length <= 0)
        {
            Debug.LogError($"Unset validationRule for {this.name}");
        }

        // Get the TMP_InputField component attached to this GameObject.
        inputField = GetComponent<TMP_InputField>();
        if (inputField != null)
        {
            // Add listeners for input field events: editing ends and value changes.
            inputField.onEndEdit.AddListener(OnEndEditValidateInput);
            inputField.onValueChanged.AddListener(OnValueChangedValidateInput);
        }
    }

    // Retrieve the name of the validation rule for this input field.
    public string GetValidationRule()
    {
        return validationRule;
    }

    // Event handler to validate input when editing ends.
    public void OnEndEditValidateInput(string input)
    {
        input = inputField.text; // Get the current text from the input field.

        // Apply the validation function, if defined, and reset text if invalid.
        if (validateFunc != null && !validateFunc(input))
        {
            inputField.text = null; // Reset the field if input is invalid.
        }
    }

    // Event handler to validate input when the value changes.
    public void OnValueChangedValidateInput(string input)
    {
        input = inputField.text; // Get the current text from the input field.

        // Change text color based on validation results.
        if (validateFunc != null && !validateFunc(input))
        {
            inputField.textComponent.color = Color.red; // Invalid input.
        }
        else
        {
            inputField.textComponent.color = Color.black; // Valid input.
        }
    }
}