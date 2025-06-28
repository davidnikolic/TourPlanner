using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetEnv;
using Newtonsoft.Json.Linq;
using System.Runtime.Intrinsics.X86;
using static System.Net.WebRequestMethods;
using Org.BouncyCastle.Utilities;

namespace TourPlanner.BL.Services.Map
{
    /// <summary>
    /// Helper class to get geographic coordinates (latitude and longitude)
    /// from a location name using the OpenRouteService API.
    /// </summary>
    public class OpenRouteServiceHelper
    {
        private readonly string ApiKey;
        private readonly HttpClient _httpClient;
        private const string GeocodingUrl = "https://api.openrouteservice.org/geocode/search";
        private const string DrivingURL = "https://api.openrouteservice.org/v2/directions/driving-car";

        public OpenRouteServiceHelper()
        {
            var envPath = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".env");
            Env.Load(envPath);

            ApiKey = Environment.GetEnvironmentVariable("TOURPLANNER_APIKEY")
                     ?? throw new InvalidOperationException("API-Key 'TOURPLANNER_APIKEY' NOT FOUND");

            // HTTP request
            _httpClient = new HttpClient();
            // Authorization header for all requests and check api, permission check
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ApiKey);

            // Clear headers, expect json responses and geojson for geographic data
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/geo+json"));
        }

        public async Task<(double Lat, double Lng)> GetCoordinatesAsync(string locationName)
        {
            var requestUrl = $"{GeocodingUrl}?api_key={ApiKey}&text={Uri.EscapeDataString(locationName)}&size=1";
            var response = await _httpClient.GetAsync(requestUrl); // send get request to openrouteserivce

            response.EnsureSuccessStatusCode();

            // Read response
            var content = await response.Content.ReadAsStringAsync();
            using (JsonDocument doc = JsonDocument.Parse(content))
            {
                var features = doc.RootElement.GetProperty("features");
                if (features.GetArrayLength() > 0)
                {
                    var coordinates = features[0].GetProperty("geometry").GetProperty("coordinates");
                    double lng = coordinates[0].GetDouble();
                    double lat = coordinates[1].GetDouble();
                    return (lat, lng);
                }
            }
            throw new Exception($"No coordinates found for {locationName}");
        }

        public async Task<(List<double[]> RouteCoordinates, double Distance)> GetRouteAsync(string startLocation, string endLocation)
        {
            var startCoords = await GetCoordinatesAsync(startLocation);
            var endCoords = await GetCoordinatesAsync(endLocation);

            // Local function to format coordinates, ensures decimal poiints instead of ","
            string FormatCoord(double lng, double lat) => $"{lng.ToString(System.Globalization.CultureInfo.InvariantCulture)},{lat.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

            var startParam = FormatCoord(startCoords.Lng, startCoords.Lat);
            var endParam = FormatCoord(endCoords.Lng, endCoords.Lat);

            // Build the request URL for directions API with start and end parameters
            var requestUrl = $"{DrivingURL}?api_key={ApiKey}&start={startParam}&end={endParam}";

            // Send GET request to directions endpoint
            var response = await _httpClient.GetAsync(requestUrl);

            // Check if the response was successful(HTTP status 2xx)
            if (!response.IsSuccessStatusCode)
            {
                // If not successful, read error content and throw an exception with details
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Fehler bei der Routenabfrage: {response.ReasonPhrase}\n{errorContent}");
            }

            // Read the JSON response content as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();
            // Parse the JSON string into a JObject for easy access to properties
            var parsedJson = JObject.Parse(jsonResponse);

            // Extract the "features" array from the JSON which contains route data
            var features = parsedJson["features"];
            // If no route features are found, return empty route and zero distance
            if (features == null || !features.Any())
            {
                return (new List<double[]>(), 0);
            }

            // Main Route calculations, extract geometry contains list of coordinate pairs
            var route = features[0];
            var geometry = route["geometry"]?["coordinates"];
            var summary = route["properties"]?["summary"];
            double distanceInMeters = summary?["distance"]?.Value<double>() ?? 0;

            // Prepare a list to hold all route points as arrays of[longitude, latitude]
            var routeCoordinates = new List<double[]>();
            if (geometry != null)
            {
                foreach (var coordPair in geometry)
                {
                    routeCoordinates.Add(new[] { coordPair[0].Value<double>(), coordPair[1].Value<double>() });
                }
            }
            // Return the route coordinates and the distance converted to kilometers
            return (routeCoordinates, distanceInMeters / 1000);
        }
        /*
        public float GetDistance(string startLocation, string endLocation)
        {
            // Hole die Koordinaten (synchron)
            var start = GetCoordinatesAsync(startLocation);
            var end = GetCoordinatesAsync(endLocation);

            // Erdradius in Kilometern
            const double R = 6371;

            // Radiant umrechnen
            double dLat = (end.Lat - start.Lat) * Math.PI / 180;
            double dLon = (end.Lng - start.Lng) * Math.PI / 180;

            // Haversine-Formel, kürzeste Entfernung zwischen zwei Punkten auf einer Kugel
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(start.Lat * Math.PI / 180) * Math.Cos(end.Lat * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distanceKm = R * c;

            return (float)distanceKm;
        }*/
    }
}
