using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.Services.Map;
using TourPlanner.UI.Map.Interface;
using Microsoft.Win32;
using System.Globalization; // Required for SaveFileDialog

namespace TourPlanner.UI.Map.Services
{
    public class MapViewService : IMapViewService
    {
        private readonly OpenRouteServiceHelper _geoHelper = new();
        private readonly string mapImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Map", "Images");
        // Flag to check if map is fully initialized
        private bool _isMapInitialized = false;

        public async Task InitializeMapAsync(object webViewObj)
        {
            if (webViewObj is not WebView2 webView) return;

            string htmlPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Map", "MapTemplate.html");
            if (!System.IO.File.Exists(htmlPath))
            {
                MessageBox.Show($"Map.html not found: {htmlPath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            await webView.EnsureCoreWebView2Async();
            if (webView.CoreWebView2 == null) return;

            webView.CoreWebView2.Navigate(new Uri(htmlPath).ToString());
        }

        public async Task UpdateMapAsync(object webViewObj, string startLocation, string endLocation)
        {
            if (webViewObj is not WebView2 webView || webView.CoreWebView2 == null) return;
            if (string.IsNullOrWhiteSpace(startLocation) || string.IsNullOrWhiteSpace(endLocation)) return;

            try
            {
                var (routeCoordinates, distance) = await _geoHelper.GetRouteAsync(startLocation, endLocation);
                var startCoords = await _geoHelper.GetCoordinatesAsync(startLocation);
                var endCoords = await _geoHelper.GetCoordinatesAsync(endLocation);

                // Local helper function to format double values with 6 decimal places wit decimal points
                string Format(double value) => value.ToString("F6", CultureInfo.InvariantCulture);

                string routeJsArray;
                if (routeCoordinates != null && routeCoordinates.Count > 0)
                {
                    // Represents exact driving paths with turns on street leve, each coordinate pair turn into js array 
                    var coordinatePairs = routeCoordinates.Select(coord => $"[{Format(coord[0])}, {Format(coord[1])}]");
                    routeJsArray = "[" + string.Join(",", coordinatePairs) + "]";
                }
                else
                {
                    routeJsArray = "[]";
                }
                // Build js function as string and displays driving route, if nothing found empty array and default start/end line
                string jsCommand = $"initMap({Format(startCoords.Lat)}, {Format(startCoords.Lng)}, {Format(endCoords.Lat)}, {Format(endCoords.Lng)}, {routeJsArray})";
                await webView.CoreWebView2.ExecuteScriptAsync(jsCommand);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Route could not be loaded. Check the locations or your internet connection.\n\nERROR: {ex.Message}", "MAP ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        public async Task SaveMapImageAsync(object webViewObj, string fullImagePath)
        {
            if (webViewObj is not WebView2 webView) return;

            try
            {
                // WebView2 initialized and map ready with 2sec delay
                await webView.EnsureCoreWebView2Async();
                if (webView.CoreWebView2 == null) return;
                await Task.Delay(1000);

                string chosenPath = fullImagePath;

                if (File.Exists(chosenPath))
                {
                    File.Delete(chosenPath);
                }

                // Take a screenshot of the WebView2 and save it as PNG
                using var fileStream = new FileStream(chosenPath, FileMode.Create);

                await webView.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, fileStream);


                // LOG, dass das Bild unter Pfad hinterlegt wurde.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during save image process: {ex.Message}");
            }
        }
    }
}
