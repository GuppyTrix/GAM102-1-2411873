using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public InputValidator[] inputValidators; // Array of input validators to manage.

    private void Start()
    {
        // Iterate through each input validator and assign validation logic.
        foreach (var inputValidator in inputValidators)
        {
            switch (inputValidator.GetValidationRule())
            {
                case "IntField_1to20000":
                    // Validate integer within 1 to 20,000.
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, 1, 20000);
                    break;

                case "FloatField_0to20001.6":
                    // Validate float within 0.0 to 20,001.6.
                    inputValidator.validateFunc = input => ValidationRules.ValidateFloatInRange(input, 0.0f, 20001.6f);
                    break;

                case "IntField_-100to1000":
                    // Validate integer within -100 to 1,000.
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, -100, 1000);
                    break;

                case "IntField_-90to90":
                    // Validate integer within -90 to 90.
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, -90, 90);
                    break;

                case "IntField_-360to360":
                    // Validate integer within -360 to 360.
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, -360, 360);
                    break;

                case "IntField_-180to180":
                    // Validate integer within -180 to 180.
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, -180, 180);
                    break;

                case "IntField_0to180":
                    // Validate integer within 0 to 180.
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, 0, 180);
                    break;

                case "IntField_0to20":
                    // Validate integer within 0 to 20.
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, 0, 20);
                    break;

                case "IntField":
                    // Validate if input is an integer.
                    inputValidator.validateFunc = input => ValidationRules.ValidateIsInt(input);
                    break;

                case "StringField_NonEmpty":
                    // Validate if input is a non-empty string.
                    inputValidator.validateFunc = ValidationRules.ValidateNonEmptyString;
                    break;
            }
        }
    }
}