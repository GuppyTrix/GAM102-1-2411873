using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;


public class UiManager : MonoBehaviour
{
    private bool isUIActive = false;

    public GameObject globe;   

    public GameObject uiPanel;
    public Transform inputFieldsParent;
    private Dictionary<string, TMP_InputField> inputFields = new Dictionary<string, TMP_InputField>();
    private Dictionary<string, TMP_Dropdown> dropdownFields = new Dictionary<string, TMP_Dropdown>();

    private void Awake()
    {
        foreach (var inputField in inputFieldsParent.GetComponentsInChildren<TMP_InputField>())
        {
            inputFields[inputField.name] = inputField;
        }

        foreach (var dropdown in inputFieldsParent.GetComponentsInChildren<TMP_Dropdown>())
        {
            dropdownFields[dropdown.name] = dropdown;
        }
    }

    private void Start()
    {
        // Make sure the UI panel is hidden at the start
        uiPanel.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUI();
        }
    }

    public void OnPressDeleteMarkers(string fieldName)
    {
        foreach (Transform child in globe.transform)
        {
            // Destroy the child GameObject
            Destroy(child.gameObject);
        }
    
    }

    private string GetInputValue(string fieldName)
    {
        if (inputFields.TryGetValue(fieldName, out TMP_InputField inputField))
        {
            return inputField.text;
        }
        else
        {
            Debug.LogError($"Input field with name '{fieldName}' not found!");
            return null;
        }
    }
    private string GetDropdownValue(string fieldName)
    {
        if (dropdownFields.TryGetValue(fieldName, out TMP_Dropdown dropdown))
        {
            int selectedIndex = dropdown.value; // Get selected index
            return dropdown.options[selectedIndex].text.ToLower(); // Get selected option's text
        }
        else
        {
            Debug.LogError($"Dropdown field with name '{fieldName}' not found!");
            return null;
        }
    }


    private int? GetInputIntValue(string fieldName)
    {
        string value = GetInputValue(fieldName);
        if (int.TryParse(value, out int result))
        {
            return result;
        }
        return null;
    }

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

        StartCoroutine(EarthquakeDataFetcher.Instance.FetchEarthquakeData(GetDropdownValue("color")));
    }

    public void FetchDataRectSearch()
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
            GetInputIntValue("minmagnitude"),
            GetInputIntValue("minlatitude"),
            GetInputIntValue("maxlatitude"),
            GetInputIntValue("minlongitude"),
            GetInputIntValue("maxlongitude")
        );

        StartCoroutine(EarthquakeDataFetcher.Instance.FetchEarthquakeData(GetDropdownValue("color")));
    }

    public void FetchDataCircleSearch()
    {
        EarthquakeDataFetcher.Instance.SetParameters(
            GetInputValue("endtime"),
            GetInputValue("starttime"),
            GetDropdownValue("orderby"),
            GetInputValue("updatedafter"),
            GetInputIntValue("limit"),
            GetInputIntValue("maxdepth"),
            GetInputIntValue("mindepth"),
            GetInputIntValue("maxmagnitude"),
            GetInputIntValue("minmagnitude"),
            GetInputIntValue("latitude"),
            GetInputIntValue("longitude"),
            GetInputIntValue("maxradius"),
            GetInputIntValue("maxradiuskm")
        );

        StartCoroutine(EarthquakeDataFetcher.Instance.FetchEarthquakeData(GetDropdownValue("color")));
    }

    private void ToggleUI()
    {
        isUIActive = !isUIActive;
        uiPanel.SetActive(isUIActive);

        if (isUIActive)
        {
            // Unlock cursor and make it visible
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else
        {
            // Lock cursor back to the game screen
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }
}
