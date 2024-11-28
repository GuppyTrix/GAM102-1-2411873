using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public InputValidator[] inputValidators;

    private void Start()
    {
        foreach (var inputValidator in inputValidators)
        {
            switch (inputValidator.GetValidationRule())
            {
                case "IntField_1to20000":
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, 1, 20000);
                    break;

                case "FloatField_0to20001.6":
                    inputValidator.validateFunc = input => ValidationRules.ValidateFloatInRange(input, 0.0f, 20001.6f);
                    break;

                case "IntField_-100to1000":
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, -100, 1000);
                    break;

                case "IntField_-90to90":
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, -90, 90);
                    break;

                case "IntField_-360to360":
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, -360, 360);
                    break;

                case "IntField_-180to180":
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, -180, 180);
                    break;

                case "IntField_0to180":
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, 0, 180);
                    break;

                case "IntField_0to20":
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, 0, 20);
                    break;

                case "IntField":
                    inputValidator.validateFunc = input => ValidationRules.ValidateIsInt(input);
                    break;

                case "StringField_NonEmpty":
                    inputValidator.validateFunc = ValidationRules.ValidateNonEmptyString;
                    break;

            }
        }
    }
}
