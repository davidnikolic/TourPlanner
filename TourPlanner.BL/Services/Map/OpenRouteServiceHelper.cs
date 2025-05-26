using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TourPlanner.BL.Services.Map
{
    /// <summary>
    /// Helper class to get geographic coordinates (latitude and longitude)
    /// from a location name using the OpenRouteService API.
    /// </summary>
    public class OpenRouteServiceHelper
    {
        private readonly string ApiKey;
        private const string GeocodingUrl = "https://api.openrouteservice.org/geocode/search";

        public OpenRouteServiceHelper()
        {
            // Tries to load the API key from environment variables
            ApiKey = Environment.GetEnvironmentVariable("TOURPLANNER_APIKEY", EnvironmentVariableTarget.Process)
                       ?? throw new Exception("API-Key NOT FOUND. You need to set environment variables 'TOURPLANNER_APIKEY'");
        }

        public (double Lat, double Lng) GetCoordinates(string locationName)
        {
            // HTTP client for making requests
            using (var client = new HttpClient())
            {
                // Authentication
                client.DefaultRequestHeaders.Add("Authorization", ApiKey);

                // Build the request url with api, location name, sync
                var response = client.GetAsync($"{GeocodingUrl}?api_key={ApiKey}&text={Uri.EscapeDataString(locationName)}&size=1")
                                     .GetAwaiter().GetResult();

                // HTTP Response status check
                response.EnsureSuccessStatusCode();

                // Read the response content as a string (JSON format)
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                // Parse json response to extract coordinates
                using (JsonDocument doc = JsonDocument.Parse(content))
                {
                    var features = doc.RootElement.GetProperty("features");
                    if (features.GetArrayLength() > 0)
                    {
                        var geometry = features[0].GetProperty("geometry");
                        var coordinates = geometry.GetProperty("coordinates");

                        double lng = coordinates[0].GetDouble();
                        double lat = coordinates[1].GetDouble();

                        return (lat, lng);
                    }
                }

                throw new Exception($"No coordinates found for {locationName}");
            }
        }

        public float GetDistance(string startLocation, string endLocation)
        {
            // Hole die Koordinaten (synchron)
            var start = GetCoordinates(startLocation);
            var end = GetCoordinates(endLocation);

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
        }
    }
}
