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
                    //inputValidator.defaultValue = "1";
                    break;

                case "IntField_-100to1000":
                    inputValidator.validateFunc = input => ValidationRules.ValidateIntInRange(input, -100, 1000);
                    //inputValidator.defaultValue = "0";
                    break;

                case "StringField_NonEmpty":
                    inputValidator.validateFunc = ValidationRules.ValidateNonEmptyString;
                    //inputValidator.defaultValue = "Default";
                    break;

            }
        }
    }
}
