using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UIElements;
using System.Drawing;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class EarthquakeDataFetcher : MonoBehaviour
{
    public static EarthquakeDataFetcher Instance;
    private EarthquakeManager earthquakeManager;
    private EarthquakeData data;


    //API call parameters
    string apiUrl = "https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&";


    //Time
    //All times use ISO8601 Date/Time format. Unless a timezone is specified, UTC is assumed
    private string? endtime = "2024-11-25T23:59:59"; // default:present time
    private string? starttime = "2024 - 11 - 25T00:00:00"; // NOW-30 days
    private string? updatedafter; //Limit to events updated after the specified time

    //Other
    private int? limit = 100; //1, 20k
    private int? maxdepth; //-100, 1k
    private int? mindepth; //-100, 1k
    private int? maxmagnitude;
    private int? minmagnitude;
    private string? orderby = "time";


    //Location
    //Requests that use both rectangle and circle will return the intersection, which may be empty, use with caution.

    //Rectangle
    //Requests may use any combination of these parameters
        private int? minlatitude; //-90, 90
        private int? maxlatitude; //-90, 90
        private int? minlongitude; //-360, 360
        private int? maxlongitude; //-360, 360

        //Circle
        //Requests must include all of latitude, longitude, and maxradius to perform a circle search
        private int? latitude;  //-90, 90
        private int? longitude; //-180, 180
        private int? maxradius; //0, 180  //in degrees
        private int? maxradiuskm; //0, 20001.6 // in km

    private void Awake()
    {
        //Intsntising this script as i only want one ever in a scene
        if (Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
        }

        earthquakeManager = GetComponent<EarthquakeManager>();
    }
    


    public IEnumerator FetchEarthquakeData(string color)
    {

        //TODO//
        //add in custon api requests from different timeframes instead of a set api call

        var queryParams = new List<string>();


        //adding all of the api params that are not null
        if (!string.IsNullOrEmpty(endtime)) queryParams.Add($"endtime={endtime}");
        if (!string.IsNullOrEmpty(starttime)) queryParams.Add($"starttime={starttime}");
        if (!string.IsNullOrEmpty(updatedafter)) queryParams.Add($"updatedafter={updatedafter}");
        if (!string.IsNullOrEmpty(orderby)) queryParams.Add($"orderby={orderby}");
        if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
        if (maxdepth.HasValue) queryParams.Add($"maxdepth={maxdepth.Value}");
        if (mindepth.HasValue) queryParams.Add($"mindepth={mindepth.Value}");
        if (maxmagnitude.HasValue) queryParams.Add($"maxmagnitude={maxmagnitude.Value}");
        if (minmagnitude.HasValue) queryParams.Add($"minmagnitude={minmagnitude.Value}");
        if (minlatitude.HasValue) queryParams.Add($"minlatitude={minlatitude.Value}");
        if (maxlatitude.HasValue) queryParams.Add($"maxlatitude={maxlatitude.Value}");
        if (minlongitude.HasValue) queryParams.Add($"minlongitude={minlongitude.Value}");
        if (maxlongitude.HasValue) queryParams.Add($"maxlongitude={maxlongitude.Value}");
        if (latitude.HasValue) queryParams.Add($"latitude={latitude.Value}");
        if (longitude.HasValue) queryParams.Add($"longitude={longitude.Value}");
        if (maxradius.HasValue) queryParams.Add($"maxradius={maxradius.Value}");
        if (maxradiuskm.HasValue) queryParams.Add($"maxradiuskm={maxradiuskm.Value}");

        string fullUrl = apiUrl + string.Join("&", queryParams);
        Debug.Log($"Fetching data from: {fullUrl}");


        //string apiUrl = $"https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&" +
        //    $"starttime=2024-11-25T00:00:00&" +
        //    $"endtime=2024-11-25T23:59:59&" +
        //    $"orderby={orderby}";

        using (UnityWebRequest request = UnityWebRequest.Get(fullUrl))
        {
            yield return request.SendWebRequest();

            //Checking if the web request failed
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching earthquake data: " + request.error);
            }
            else
            {
                //storing the downloaded text
                string jsonResponse = request.downloadHandler.text; 
                Debug.Log("Earthquake Data Fetched: " + jsonResponse);

                // Parse JSON using JsonUtility
                data = JsonUtility.FromJson<EarthquakeData>(jsonResponse);

                //loop through each Earthquake
                foreach (Feature feature in data.features)
                {

                    //data needed from the earthquake api to make a marker and debug
                    float longitude = feature.geometry.coordinates[0];
                    float latitude = feature.geometry.coordinates[1];
                    float magnitude = feature.properties.mag;
                    string place = feature.properties.place;
                    long time = feature.properties.time; // Unix timestamp in milliseconds
                    float depth = feature.geometry.coordinates[2];

                    // Convert Unix time to DateTime in UTC
                    System.DateTime dateTime = UnixTimeStampToDateTime(time);

                    //Creating the marker for the Earthquake
                    StartCoroutine(EarthquakeManager.Instance.PlaceEarthquakeMarker(latitude, longitude, color));

                    //SetParameters params to null to reset the search on the next call
                    SetParameters();
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


    //Sets all of the data that the Api needs if they are not null except limit and orderby
    public void SetParameters(
        string endtime = null,
        string starttime = null,
        string updatedafter = null,
        string orderby = null,
        int? limit = null,
        int? maxdepth = null,
        int? mindepth = null,
        int? maxmagnitude = null,
        int? minmagnitude = null,
        int? minlatitude = null,
        int? maxlatitude = null,
        int? minlongitude = null,
        int? maxlongitude = null,
        int? latitude = null,
        int? longitude = null,
        int? maxradius = null,
        int? maxradiuskm = null)
    {
        this.endtime = endtime;
        this.starttime = starttime;
        this.updatedafter = updatedafter;

        this.orderby = orderby ?? this.orderby;
        this.limit = limit ?? this.limit;
        this.maxdepth = maxdepth;
        this.mindepth = mindepth;
        this.maxmagnitude = maxmagnitude;
        this.minmagnitude = minmagnitude;
        this.minlatitude = minlatitude;
        this.maxlatitude = maxlatitude;
        this.minlongitude = minlongitude;
        this.maxlongitude = maxlongitude;
        this.latitude = latitude;
        this.longitude = longitude;
        this.maxradius = maxradius;
        this.maxradiuskm = maxradiuskm;
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
}

// Earthquake location data
[System.Serializable]
public class Geometry
{
    public List<float> coordinates; // Longitude, Latitude, Depth

}

