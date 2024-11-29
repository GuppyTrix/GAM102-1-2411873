using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;


public class UiManager : MonoBehaviour
{
    //private bool isUIActive = false;

    public GameObject globe;

    public Canvas ESCMenu;
    public Canvas APIMenu;

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
        SetCanvasState(ESCMenu, false);
        SetCanvasState(APIMenu, false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        CameraController.Instance.SetCameraMove(true);
    }

    public void leaveESCMenu()
    {
        SetCanvasState(ESCMenu, false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        CameraController.Instance.SetCameraMove(true);
    }

    public void doExitGame()
    {
        Application.Quit();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ESCMenu.isActiveAndEnabled)
            {
                CameraController.Instance.SetCameraMove(true);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                SetCanvasState(APIMenu, false);
                ToggleCanvas(ESCMenu);
            }
            else
            {
                CameraController.Instance.SetCameraMove(false);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                SetCanvasState(APIMenu, false);
                ToggleCanvas(ESCMenu);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && !ESCMenu.isActiveAndEnabled)
        {
            CameraController.Instance.ToggleCameraMove();
            ToggleCanvas(APIMenu);
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

    public TMP_InputField GetInputFieldByName(string fieldName)
    {
        if (inputFields.TryGetValue(fieldName, out TMP_InputField inputField))
        {
            return inputField;
        }
        else
        {
            Debug.LogError($"Input field with name '{fieldName}' not found in the dictionary.");
            return null; 
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
            int selectedIndex = dropdown.value;
            return dropdown.options[selectedIndex].text.ToLower();
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

    private IEnumerator SetMessage(TMP_InputField inputField, string messsage = "*Required*")
    {
        inputField.text = messsage;
        yield return new WaitForSeconds(2f);
        inputField.text = "";
    }

    private void ToggleCanvas(Canvas canvas)
    {
        canvas.enabled = !canvas.enabled;   

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

    private void SetCanvasState(Canvas canvas, bool state)
    {
        canvas.enabled = state;

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
        if (
            !((GetInputIntValue("latitude").HasValue && GetInputIntValue("longitude").HasValue)
            &&(GetInputIntValue("maxradius").HasValue ^ GetInputIntValue("maxradiuskm").HasValue))
            )
        {
            Debug.LogError("Missing Reuired Fields");
            return;
        }


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

        StartCoroutine(EarthquakeDataFetcher.Instance.FetchEarthquakeData(GetDropdownValue("color")));
    }
    public void FetchDataCircleandRectSearch()
    {
        if (!((GetInputIntValue("latitude").HasValue && GetInputIntValue("longitude").HasValue)
            &&(GetInputIntValue("maxradius").HasValue ^ GetInputIntValue("maxradiuskm").HasValue)))
        {
            Debug.LogError("Missing Reuired Fields");
            return;
        }
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

        StartCoroutine(EarthquakeDataFetcher.Instance.FetchEarthquakeData(GetDropdownValue("color")));
    }
}
