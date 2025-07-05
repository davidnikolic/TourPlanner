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
using TourPlanner.Logging;
using TourPlanner.Logging.Interfaces;

namespace TourPlanner.UI.Map.Services
{
    public class MapViewService : IMapViewService
    {
        private readonly ILoggerWrapper _logger;
        private readonly OpenRouteServiceHelper _geoHelper = new();
        private readonly string mapImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Map", "Images");
        // Flag to check if map is fully initialized
        private bool _isMapInitialized = false;

        public MapViewService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MapViewService>();
            _logger.Info("MapViewService initialized");
        }
        public async Task InitializeMapAsync(object webViewObj)
        {
            if (webViewObj is not WebView2 webView) return;

            string htmlPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Map", "MapTemplate.html");
            if (!System.IO.File.Exists(htmlPath))
            {
                _logger.Debug("Map.html not found");
                MessageBox.Show($"Map.html not found: {htmlPath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            await webView.EnsureCoreWebView2Async();
            if (webView.CoreWebView2 == null) return;

            webView.CoreWebView2.Navigate(new Uri(htmlPath).ToString());
        }

        public async Task UpdateMapAsync(object webViewObj, string startLocation, string endLocation)
        {
            if (webViewObj is not WebView2 webView || webView.CoreWebView2 == null)
            {
                _logger.Debug("UpdateMapAsync failed - WebView2 not ready");
                return;
            }
            if (string.IsNullOrWhiteSpace(startLocation) || string.IsNullOrWhiteSpace(endLocation))
            {
                _logger.Debug("UpdateMapAsync skipped - empty locations");
                return;
            }
            _logger.Info($"Updating map route from '{startLocation}' to '{endLocation}'");
            try
            {
                var (routeCoordinates, distance) = await _geoHelper.GetRouteAsync(startLocation, endLocation);
                var startCoords = await _geoHelper.GetCoordinatesAsync(startLocation);
                var endCoords = await _geoHelper.GetCoordinatesAsync(endLocation);
                _logger.Debug($"Route coordinates retrieved: {routeCoordinates?.Count ?? 0} points");

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
                _logger.Info("Map updated successfully");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to update map for route: {startLocation} to {endLocation}", ex);
                MessageBox.Show($"Route could not be loaded. Check the locations or your internet connection.\n\nERROR: {ex.Message}", "MAP ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        public async Task SaveMapImageAsync(object webViewObj, string fullImagePath)
        {
            if (webViewObj is not WebView2 webView)
            {
                _logger.Debug("SaveMapImageAsync failed - invalid WebView2 object");
                return;
            }
            _logger.Info($"Saving map image to: {fullImagePath}");
            try
            {
                // WebView2 initialized and map ready with 2sec delay
                await webView.EnsureCoreWebView2Async();
                if (webView.CoreWebView2 == null)
                {
                    _logger.Error("SaveMapImageAsync failed - CoreWebView2 is null");
                    return;
                }
                await Task.Delay(1000);

                string chosenPath = fullImagePath;

                if (File.Exists(chosenPath))
                {
                    File.Delete(chosenPath);
                    _logger.Debug($"Existing image file deleted: {chosenPath}");
                }

                // Take a screenshot of the WebView2 and save it as PNG
                using var fileStream = new FileStream(chosenPath, FileMode.Create);

                await webView.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, fileStream);


                _logger.Info($"Map image saved successfully: {chosenPath}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to save map image: {fullImagePath}", ex);
                MessageBox.Show($"Error during save image process: {ex.Message}");
            }
        }
    }
}
