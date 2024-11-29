using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValidationRules
{
    // Validate if the input is an integer within the specified range.
    public static bool ValidateIntInRange(string input, int min, int max)
    {
        return int.TryParse(input, out int result) && result >= min && result <= max;
    }

    // Validate if the input is a float within the specified range.
    public static bool ValidateFloatInRange(string input, float min, float max)
    {
        return float.TryParse(input, out float result) && result >= min && result <= max;
    }

    // Validate if the input is a valid integer.
    public static bool ValidateIsInt(string input)
    {
        return int.TryParse(input, out int result);
    }

    // Validate if the input is a non-empty string.
    public static bool ValidateNonEmptyString(string input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }
}