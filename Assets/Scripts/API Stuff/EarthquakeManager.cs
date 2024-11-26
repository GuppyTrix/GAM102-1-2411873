using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class EarthquakeManager : MonoBehaviour
{
    public GameObject earthquakeMarkerPrefab;  // Reference to your marker prefab
    public GameObject globe;  // Reference to your globe (the sphere in your scene)


    void Start()
    {
        //StartCoroutine(EarthquakeDataFetcher.Instance.FetchEarthquakeData());
        //PlaceEarthquakeMarker(-30.5092f, -72.0447f);
        //PlaceEarthquakeMarker(38.83767f, -122.7813f);
        //PlaceEarthquakeMarker(19.39083f, -155.28f);
    }



    public void PlaceEarthquakeMarker(float latitude, float longitude, string color)
    {
        Vector3 globePosition = LatitudeLongitudeTo3DPosition(latitude, longitude);

        Vector3 markerPosition = globe.transform.position + globePosition;

        GameObject marker = Instantiate(earthquakeMarkerPrefab, markerPosition, Quaternion.identity, globe.transform);

                // maybe change color based on magnitude
        Renderer renderer = marker.GetComponent<MeshRenderer>();
        renderer.material.color = GetMarkerColor(color);

    }

    private UnityEngine.Color GetMarkerColor(string color)
    {
        //“green”, “yellow”, “orange”, “red”.
        switch (color)
        {
            case "green":
                return UnityEngine.Color.green;
            case "yellow":
                return UnityEngine.Color.yellow;
            case "orange":
                return new UnityEngine.Color(1.0f, 0.5f, 0.0f); 
            case "red":
                return UnityEngine.Color.red;
            default:
                return UnityEngine.Color.white;
        }

    }

    // Convert latitude/longitude to 3D position on the globe
    private Vector3 LatitudeLongitudeTo3DPosition(float latitude, float longitude)
    {
        // https://www.reddit.com/r/askmath/comments/ybbn3b/how_to_transform_latlong_coordinates_on_a_sphere/

        // Convert degrees to radians
        float latRad = Mathf.Deg2Rad * latitude;
        float lonRad = Mathf.Deg2Rad * longitude;

         //Spherical to Cartesian conversion
        float z = 1 * Mathf.Cos(latRad) * Mathf.Sin(lonRad);
        float y = 1 * Mathf.Sin(latRad);
        float x = 1 * Mathf.Cos(latRad) * Mathf.Cos(lonRad);

        // Return as a Vector3 position
        return new Vector3(x, y, z);
    }
}
