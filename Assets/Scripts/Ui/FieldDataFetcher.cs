using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FieldDataFetcher : MonoBehaviour
{
    public GameObject globe; // Reference to the globe GameObject
    public Canvas ESCMenu; // Pause menu
    public Canvas APIMenu; // API menu
    public GameObject uiPanel; // UI panel to contain input fields
    public Transform inputFieldsParent; // Parent object that holds the input fields
    private Dictionary<string, TMP_InputField> inputFields = new Dictionary<string, TMP_InputField>(); // Dictionary to store input fields by name
    private Dictionary<string, TMP_Dropdown> dropdownFields = new Dictionary<string, TMP_Dropdown>(); // Dictionary for dropdown fields

    // Initializes the input fields and dropdowns
    private void Awake()
    {
        foreach (var inputField in inputFieldsParent.GetComponentsInChildren<TMP_InputField>())
        {
            inputFields[inputField.name] = inputField; // Add input fields to dictionary by name
        }

        foreach (var dropdown in inputFieldsParent.GetComponentsInChildren<TMP_Dropdown>())
        {
            dropdownFields[dropdown.name] = dropdown; // Add dropdowns to dictionary by name
        }
    }

    // Starts with menus hidden and locks the cursor for better control
    private void Start()
    {
        SetCanvasState(ESCMenu, false); // Hide ESC menu
        SetCanvasState(APIMenu, false); // Hide API menu
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        CameraController.Instance.SetCameraMove(true); // Enable camera movement
    }

    // Exits the ESC menu and restores cursor and camera control
    public void leaveESCMenu()
    {
        SetCanvasState(ESCMenu, false); // Hide ESC menu
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        CameraController.Instance.SetCameraMove(true); // Re-enable camera movement
    }

    // Quits the game
    public void doExitGame()
    {
        Application.Quit(); // Exit the application
    }

    // Handles key presses to toggle ESC menu or API menu visibility
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Check if ESC is pressed
        {
            if (ESCMenu.isActiveAndEnabled)
            {
                CameraController.Instance.SetCameraMove(true);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                SetCanvasState(APIMenu, false); // Close API menu
                ToggleCanvas(ESCMenu); // Toggle ESC menu visibility
            }
            else
            {
                CameraController.Instance.SetCameraMove(false);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                SetCanvasState(APIMenu, false); // Close API menu
                ToggleCanvas(ESCMenu); // Open ESC menu
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && !ESCMenu.isActiveAndEnabled)
        {
            CameraController.Instance.ToggleCameraMove();
            ToggleCanvas(APIMenu); // Toggle API menu visibility
        }
    }

    // Deletes all markers in the globe (children of globe)
    public void OnPressDeleteMarkers(string fieldName)
    {
        foreach (Transform child in globe.transform)
        {
            Destroy(child.gameObject); // Destroy each child GameObject (marker)
        }
    }

    // Gets an input field by its name from the dictionary
    public TMP_InputField GetInputFieldByName(string fieldName)
    {
        if (inputFields.TryGetValue(fieldName, out TMP_InputField inputField))
        {
            return inputField;// return the inputfield for other uses
        }
        else
        {
            Debug.LogError($"Input field with name '{fieldName}' not found in the dictionary.");
            return null;
        }
    }

    // Gets the text value from an input field by name
    private string GetInputValue(string fieldName)
    {
        if (inputFields.TryGetValue(fieldName, out TMP_InputField inputField))
        {
            return inputField.text; // Return the text from the input field
        }
        else
        {
            Debug.LogError($"Input field with name '{fieldName}' not found!");
            return null;
        }
    }

    // Gets the selected value from a dropdown field
    private string GetDropdownValue(string fieldName)
    {
        if (dropdownFields.TryGetValue(fieldName, out TMP_Dropdown dropdown))
        {
            int selectedIndex = dropdown.value;
            return dropdown.options[selectedIndex].text.ToLower(); // Return selected option in lowercase
        }
        else
        {
            Debug.LogError($"Dropdown field with name '{fieldName}' not found!");
            return null;
        }
    }

    // Gets an integer value from an input field
    private int? GetInputIntValue(string fieldName)
    {
        string value = GetInputValue(fieldName);
        if (int.TryParse(value, out int result))
        {
            return result; // Return the integer value if parsing is successful
        }
        return null; // Return null if parsing fails
    }

    // Displays a temporary message in an input field
    private IEnumerator SetMessage(TMP_InputField inputField, string message = "*Required*")
    {
        inputField.text = message; // Set the message
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        inputField.text = ""; // Clear the message
    }

    // Toggles visibility of a canvas and adjusts cursor state
    private void ToggleCanvas(Canvas canvas)
    {
        canvas.enabled = !canvas.enabled; // Toggle canvas visibility

        // Adjust cursor state based on the canvas visibility
        if (canvas.enabled)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }

    // Sets the state of a canvas (enabled or disabled) and adjusts cursor state
    private void SetCanvasState(Canvas canvas, bool state)
    {
        canvas.enabled = state; // Set canvas visibility

        // Adjust cursor state based on the canvas state
        if (canvas.enabled)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }

    // Fetches data for a regular search based on input values
    public void FetchDataRegularSearch()
    {
        EarthquakeDataFetcher.Instance.SetParameters(
            GetInputValue("endtime"),
            GetInputValue("starttime"),
            GetInputValue("updatedafter"),
            GetDropdownValue("orderby"),
            GetInputIntValue("limit"),
            GetInputIntValue("maxdepth"),
            GetInputIntValue("mindepth"),
            GetInputIntValue("maxmagnitude"),
            GetInputIntValue("minmagnitude")
        );

        StartCoroutine(EarthquakeDataFetcher.Instance.FetchEarthquakeData(GetDropdownValue("color"))); // Fetch data with color option
    }

    // Fetches data for a rectangular search based on input values
    public void FetchDataRectSearch()
    {
        //Sends the data ready for the api request
        EarthquakeDataFetcher.Instance.SetParameters(
            GetInputValue("endtime"),
            GetInputValue("starttime"),
            GetInputValue("updatedafter"),
            GetDropdownValue("orderby"),
            GetInputIntValue("limit"),
            GetInputIntValue("maxdepth"),
            GetInputIntValue("mindepth"),
            GetInputIntValue("maxmagnitude"),
            GetInputIntValue("minmagnitude"),
            GetInputIntValue("minlatitude"),
            GetInputIntValue("maxlatitude"),
            GetInputIntValue("minlongitude"),
            GetInputIntValue("maxlongitude")
        );

        StartCoroutine(EarthquakeDataFetcher.Instance.FetchEarthquakeData(GetDropdownValue("color"))); // Fetch data with color option
    }

    // Fetches data for a circular search based on input values
    public void FetchDataCircleSearch()
    {
        if (
            !((GetInputIntValue("latitude").HasValue && GetInputIntValue("longitude").HasValue)
            && (GetInputIntValue("maxradius").HasValue ^ GetInputIntValue("maxradiuskm").HasValue))
            )
        {
            Debug.LogError("Missing Required Fields");
            return; // Exit if required fields are missing
        }

        //Sends the data ready for the api request
        EarthquakeDataFetcher.Instance.SetParameters(
            GetInputValue("endtime"),
            GetInputValue("starttime"),
            GetInputValue("updatedafter"),
            GetDropdownValue("orderby"),
            GetInputIntValue("limit"),
            GetInputIntValue("maxdepth"),
            GetInputIntValue("mindepth"),
            GetInputIntValue("maxmagnitude"),
            GetInputIntValue("minmagnitude"),
            null, null, null, null,
            GetInputIntValue("latitude"),
            GetInputIntValue("longitude"),
            GetInputIntValue("maxradius"),
            GetInputIntValue("maxradiuskm")
        );

        StartCoroutine(EarthquakeDataFetcher.Instance.FetchEarthquakeData(GetDropdownValue("color"))); // Fetch data with color option
    }

    // Fetches data for a search combining circular and rectangular api search
    public void FetchDataCircleandRectSearch()
    {
        if (!((GetInputIntValue("latitude").HasValue && GetInputIntValue("longitude").HasValue)
            && (GetInputIntValue("maxradius").HasValue ^ GetInputIntValue("maxradiuskm").HasValue)))
        {
            Debug.LogError("Missing Required Fields");
            return; // Exit if required fields are missing
        }
        //Sends the data ready for the api request
        EarthquakeDataFetcher.Instance.SetParameters(
            GetInputValue("endtime"),
            GetInputValue("starttime"),
            GetInputValue("updatedafter"),
            GetDropdownValue("orderby"),
            GetInputIntValue("limit"),
            GetInputIntValue("maxdepth"),
            GetInputIntValue("mindepth"),
            GetInputIntValue("maxmagnitude"),
            GetInputIntValue("minmagnitude"),
            GetInputIntValue("minlatitude"),
            GetInputIntValue("maxlatitude"),
            GetInputIntValue("minlongitude"),
            GetInputIntValue("maxlongitude"),
            GetInputIntValue("latitude"),
            GetInputIntValue("longitude"),
            GetInputIntValue("maxradius"),
            GetInputIntValue("maxradiuskm")
        );

        StartCoroutine(EarthquakeDataFetcher.Instance.FetchEarthquakeData(GetDropdownValue("color"))); // Fetch data with color option
    }
}
