using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UIElements;

public class EarthquakeDataFetcher : MonoBehaviour
{
    public static EarthquakeDataFetcher Instance;
    private EarthquakeManager earthquakeManager;

    public TMP_InputField alertlevel;
    public GameObject uiPanel;
    private bool isUIActive = false;
    //public TMP_InputField startTimeInput;
    //public TMP_InputField endTimeInput;
    //public TMP_InputField mMagnitudeInput;
    //public TMP_Text resultText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
        }

        earthquakeManager = GetComponent<EarthquakeManager>();
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
        // Toggle UI with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUI();
        }
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

    public void OnFetchDataButtonClick()
    {
        Debug.Log("THIS CODE IS BEING RUN");

        FetchEarthquakeData();
        Debug.Log("THIS CODE IS BEING RUN");

    }

    public IEnumerator FetchEarthquakeData()
    {
        Debug.Log("THIS CODE IS BEING RUN");
        string alertlvl = alertlevel.text;

        //TODO//
        //add in custon api requests from different timeframes instead of a set api call
        string apiUrl = $"https://earthquake.usgs.gov/fdsnws/event/1/query?" +
            $"format=geojson&" +
            $"starttime=2024-11-25T00:00:00&" +
            $"endtime=2024-11-25T23:59:59&" +
            $"orderby=time&" +
            $"alertlevel={alertlvl}";

        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching earthquake data: " + request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Earthquake Data Fetched: " + jsonResponse);

                // Parse JSON using JsonUtility
                EarthquakeData data = JsonUtility.FromJson<EarthquakeData>(jsonResponse);

                foreach (Feature feature in data.features)
                {
                    float longitude = feature.geometry.coordinates[0];
                    float latitude = feature.geometry.coordinates[1];
                    float magnitude = feature.properties.mag;
                    string place = feature.properties.place;
                    long time = feature.properties.time; // Unix timestamp in milliseconds
                    float depth = feature.geometry.coordinates[2];
                    string alert = feature.properties.alert;

                    // Convert Unix time to DateTime (in UTC i think)
                    System.DateTime dateTime = UnixTimeStampToDateTime(time);

                    //Debug.Log($"Earthquake Details: \n" +
                    //          $"  Place: {place}\n" +
                    //          $"  Latitude: {latitude}, Longitude: {longitude}, Depth: {depth} km\n" +
                    //          $"  Magnitude: {magnitude}\n" +
                    //          $"  Alert Level: {alert}\n" +
                    //          $"  Time: {dateTime}");
                    earthquakeManager.PlaceEarthquakeMarker(latitude, longitude, alert);

                    //if (earthquakeManager != null)
                    //{
                    //    earthquakeManager.PlaceEarthquakeMarker(latitude, longitude);
                    //}
                    //else
                    //{
                    //    Debug.LogError("EarthquakeManager instance is null.");
                    //}
                }
            }
        }
    }

    /// <summary>
    /// credit: https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
    /// </summary>
    /// <param name="unixTimeStamp"></param>
    /// <returns>DateTime</returns>
    private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}


//FDSN	Web	Service	Specifications (not entirely sure if its relevant or not)
//https://www.fdsn.org/webservices/FDSN-WS-Specifications-1.0.pdf

//ANSS Comprehensive Earthquake Catalog (ComCat) Event Terms Documentation
//https://earthquake.usgs.gov/data/comcat/data-eventterms.php#felt

//GeoJSON Summary Format
//https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php

//API request format
//https://earthquake.usgs.gov/fdsnws/event/1/



// Root GeoJSON structure
[System.Serializable]
public class EarthquakeData
{
    public List<Feature> features;
}

// Individual earthquake features
[System.Serializable]
public class Feature
{
    public Properties properties;
    public Geometry geometry;
}

// Earthquake metadata
[System.Serializable]
public class Properties
{
    public float mag; // Magnitude
    public string place; // Location description
    public long time; // Time in Unix epoch (milliseconds)
    public string alert; //“green”, “yellow”, “orange”, “red”.
}

// Earthquake location data
[System.Serializable]
public class Geometry
{
    public List<float> coordinates; // Longitude, Latitude, Depth

}

