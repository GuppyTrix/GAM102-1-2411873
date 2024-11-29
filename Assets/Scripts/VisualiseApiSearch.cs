using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VisualiseApiSearch : MonoBehaviour
{
    public TMP_InputField[] RectSearchItems;
    public TMP_InputField[] CircleSearchItems;

    public GameObject Sphere; 
    private Material sphereMaterial;

    private void Start()
    {
        if (Sphere != null)
        {
            sphereMaterial = Sphere.GetComponent<Renderer>().material;
        }
    }

    public void OnPressRectPreview()
    {
        ClearSphere();

        if (RectSearchItems != null && RectSearchItems.Length == 4)
        {
            VisualizeRectSearch();
        }
    }

    public void OnPressCirclePreview()
    {
        ClearSphere();

        if (CircleSearchItems != null && CircleSearchItems.Length == 4)
        {
            VisualizeCircleSearch();
        }
    }

    void VisualizeRectSearch()
    {
        float minLat = ParseInput(RectSearchItems[0], -90f);
        float maxLat = ParseInput(RectSearchItems[1], 90f);
        float minLon = ParseInput(RectSearchItems[2], -180f);
        float maxLon = ParseInput(RectSearchItems[3], 180f);

        // Clamp values to valid ranges
        minLat = Mathf.Clamp(minLat, -90f, 90f);
        maxLat = Mathf.Clamp(maxLat, -90f, 90f);
        minLon = Mathf.Clamp(minLon, -180f, 180f);
        maxLon = Mathf.Clamp(maxLon, -180f, 180f);

        // Highlight the spherical area
        HighlightSphere(minLat, maxLat, minLon, maxLon);
    }

    void VisualizeCircleSearch()
    {
        float centerLat = ParseInput(CircleSearchItems[0], 0f);
        float centerLon = ParseInput(CircleSearchItems[1], 0f);
        float maxRadiusDegrees = ParseInput(CircleSearchItems[2], -1f);
        float maxRadiusKm = ParseInput(CircleSearchItems[3], -1f);

        if (maxRadiusDegrees < 0 && maxRadiusKm < 0)
        {
            Debug.LogError("CircleSearch requires a radius value!");
            return;
        }

        float radiusDegrees = maxRadiusDegrees > 0 ? maxRadiusDegrees : maxRadiusKm / 111f; // 1 degree ~ 111 km

        HighlightSphere(centerLat - radiusDegrees, centerLat + radiusDegrees, centerLon - radiusDegrees, centerLon + radiusDegrees);
    }

    void HighlightSphere(float minLat, float maxLat, float minLon, float maxLon)
    {
        if (sphereMaterial == null) return;

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
            sphereMaterial.SetFloat("_MinLat", -90f);
            sphereMaterial.SetFloat("_MaxLat", 90f);
            sphereMaterial.SetFloat("_MinLon", -180f);
            sphereMaterial.SetFloat("_MaxLon", 180f);
        }
    }

    float ParseInput(TMP_InputField inputField, float defaultValue)
    {
        if (inputField == null || string.IsNullOrEmpty(inputField.text))
            return defaultValue;

        float value;
        if (float.TryParse(inputField.text, out value))
            return value;

        return defaultValue;
    }
}
