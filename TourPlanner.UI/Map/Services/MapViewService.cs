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
using Microsoft.Win32; // Required for SaveFileDialog

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
            // Check if object is a valid WebView2 isntance
            if (webViewObj is not WebView2 webView)
                throw new ArgumentException("Invalid WebView object");

            string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Map", "MapTemplate.html");

            if (!File.Exists(htmlPath)) return;

            // Make sure WebView2 is initialized
            await webView.EnsureCoreWebView2Async();
            // Load the local HTML template
            webView.CoreWebView2.Navigate(new Uri(htmlPath).ToString());

            // When loading completes, execute JS to initialize the map and set flag
            webView.CoreWebView2.NavigationCompleted += (_, _) =>
            {
                string js = $"initMap(48.2082, 16.3738, 47.8095, 13.0550)";
                webView.CoreWebView2.ExecuteScriptAsync(js);
                _isMapInitialized = true;
            };
        }

        public async Task UpdateMapAsync(object webViewObj, string startLocation, string endLocation)
        {
            if (webViewObj is not WebView2 webView) return;


            var startCoords = _geoHelper.GetCoordinates(startLocation);
            var endCoords = _geoHelper.GetCoordinates(endLocation);

            // Helper method to format double with 6 decimal places
            string Format(double value) => value.ToString("F6", System.Globalization.CultureInfo.InvariantCulture);

            // Build the JavaScript string to call initMap with new coordinates
            string js = $"initMap({Format(startCoords.Lat)}, {Format(startCoords.Lng)}, {Format(endCoords.Lat)}, {Format(endCoords.Lng)})";

            await webView.CoreWebView2.ExecuteScriptAsync(js);
        }

        public async Task SaveMapImageAsync(object webViewObj)
        {
            if (webViewObj is not WebView2 webView) return;

            try
            {
                // WebView2 initialized and map ready with 2sec delay
                await webView.EnsureCoreWebView2Async();
                await Task.Delay(2000);

                // Open save dialog for the user to choose file location and name
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "PNG Image|*.png",// Allow only .png files
                    FileName = $"map_{DateTime.Now:yyyyMMdd_HHmmss}.png",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)// Default document folder
                };

                // If the user cancels, exit
                bool? result = saveFileDialog.ShowDialog();
                if (result != true)
                    return; 

                string chosenPath = saveFileDialog.FileName;

                // Take a screenshot of the WebView2 and save it as PNG
                using var fileStream = new FileStream(chosenPath, FileMode.Create);
                await webView.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, fileStream);

                // Update database with the path to the saved map image
                var tourService = ((App)Application.Current).Services.GetRequiredService<ITourService>();
                int lastTourId = tourService.GetLastTourId();
                tourService.UpdateTourMapImagePath(lastTourId, chosenPath);

                MessageBox.Show("Image is saved and the path is updated in the database.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during save image process: {ex.Message}");
            }
        }
    }
}
