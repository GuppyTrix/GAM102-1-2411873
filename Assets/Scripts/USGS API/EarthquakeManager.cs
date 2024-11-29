using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class EarthquakeManager : MonoBehaviour
{
    public GameObject earthquakeMarkerPrefab; 
    public GameObject globe;
    public static EarthquakeManager Instance;
    private EarthquakeDataFetcher earthquakeDataFetcher;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        earthquakeDataFetcher = GetComponent<EarthquakeDataFetcher>();
    }

    public IEnumerator PlaceEarthquakeMarker(float latitude, float longitude, string color)
    {
        // Convert latitude and longitude to a 3D position on the globe.
        Vector3 globePosition = LatitudeLongitudeTo3DPosition(latitude, longitude);

        // Compute the marker's world position relative to the globe.
        Vector3 markerPosition = globe.transform.position + globePosition;

        // Instantiate the marker prefab at the calculated position, parented to the globe.
        GameObject marker = Instantiate(earthquakeMarkerPrefab, markerPosition, Quaternion.identity, globe.transform);

        // Set the marker's color using the specified string.
        Renderer renderer = marker.GetComponent<MeshRenderer>();
        renderer.material.color = GetMarkerColor(color);

        // Wait for one frame (placeholder for potential future logic).
        yield return null;
    }


    private UnityEngine.Color GetMarkerColor(string color)
    {
        Debug.Log(color);

        //“green”, “yellow”, “orange”, “red”.
        switch (color.ToLower())
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
