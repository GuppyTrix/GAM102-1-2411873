using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValidationRules
{
    public static bool ValidateIntInRange(string input, int min, int max)
    {
        return int.TryParse(input, out int result) && result >= min && result <= max;
    }

    public static bool ValidateNonEmptyString(string input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }

    public static bool ValidateFloatInRange(string input, float min, float max)
    {
        return float.TryParse(input, out float result) && result >= min && result <= max;
    }

}
