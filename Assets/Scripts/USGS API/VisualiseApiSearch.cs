using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VisualiseApiSearch : MonoBehaviour
{
    public TMP_InputField[] RectSearchItems; // Input fields for rectangular search.
    public TMP_InputField[] CircleSearchItems; // Input fields for circular search.

    public GameObject Sphere; // Reference to the sphere GameObject.
    private Material sphereMaterial; // Material used to modify sphere visuals.

    private void Start()
    {
        // Initialize the sphere material if the sphere exists.
        if (Sphere != null)
        {
            sphereMaterial = Sphere.GetComponent<Renderer>().material;
        }
    }

    public void OnPressClear()
    {
        ClearSphere(); // Reset sphere visuals.
    }

    public void OnPressRectPreview()
    {
        ClearSphere(); // Reset sphere visuals.

        // If valid input fields exist, visualize rectangular search.
        if (RectSearchItems != null && RectSearchItems.Length == 4)
        {
            VisualizeRectSearch();
        }
    }

    public void OnPressCirclePreview()
    {
        ClearSphere(); // Reset sphere visuals.

        // If valid input fields exist, visualize circular search.
        if (CircleSearchItems != null && CircleSearchItems.Length == 4)
        {
            VisualizeCircleSearch();
        }
    }

    void VisualizeRectSearch()
    {
        // Parse inputs and clamp latitude/longitude to valid ranges.
        float minLat = Mathf.Clamp(ParseInput(RectSearchItems[0], -90f), -90f, 90f);
        float maxLat = Mathf.Clamp(ParseInput(RectSearchItems[1], 90f), -90f, 90f);
        float minLon = Mathf.Clamp(ParseInput(RectSearchItems[2], -180f), -180f, 180f);
        float maxLon = Mathf.Clamp(ParseInput(RectSearchItems[3], 180f), -180f, 180f);

        // Highlight the spherical area.
        HighlightSphere(minLat, maxLat, minLon, maxLon);
    }

    void VisualizeCircleSearch()
    {
        // Parse inputs for circle center and radius.
        float centerLat = ParseInput(CircleSearchItems[0], 0f);
        float centerLon = ParseInput(CircleSearchItems[1], 0f);
        float maxRadiusDegrees = ParseInput(CircleSearchItems[2], -1f);
        float maxRadiusKm = ParseInput(CircleSearchItems[3], -1f);

        // Ensure a radius value is provided.
        if (maxRadiusDegrees < 0 && maxRadiusKm < 0)
        {
            Debug.LogError("CircleSearch requires a radius value!");
            return;
        }

        // Convert radius from km to degrees if necessary.
        float radiusDegrees = maxRadiusDegrees > 0 ? maxRadiusDegrees : maxRadiusKm / 111f;

        // Highlight the spherical area based on circle parameters.
        HighlightSphere(centerLat - radiusDegrees, centerLat + radiusDegrees, centerLon - radiusDegrees, centerLon + radiusDegrees);
    }

    void HighlightSphere(float minLat, float maxLat, float minLon, float maxLon)
    {
        if (sphereMaterial == null) return;

        // Update shader properties to highlight the specified region.
        sphereMaterial.SetFloat("_MinLat", minLat);
        sphereMaterial.SetFloat("_MaxLat", maxLat);
        sphereMaterial.SetFloat("_MinLon", minLon);
        sphereMaterial.SetFloat("_MaxLon", maxLon);

        Debug.Log($"Highlighting region: MinLat={minLat}, MaxLat={maxLat}, MinLon={minLon}, MaxLon={maxLon}");
    }

    void ClearSphere()
    {
        if (sphereMaterial != null)
        {
            // Reset the sphere's highlighted region.
            sphereMaterial.SetFloat("_MinLat", 90f);
            sphereMaterial.SetFloat("_MaxLat", 90f);
            sphereMaterial.SetFloat("_MinLon", 180f);
            sphereMaterial.SetFloat("_MaxLon", 180f);
        }
    }

    float ParseInput(TMP_InputField inputField, float defaultValue)
    {
        // Parse input field value or return the default value if invalid.
        if (inputField == null || string.IsNullOrEmpty(inputField.text))
            return defaultValue;

        float value;
        return float.TryParse(inputField.text, out value) ? value : defaultValue;
    }
}
