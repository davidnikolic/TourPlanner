using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.UI.Map.Interface;
using TourPlanner.BL.Interfaces;
using Microsoft.Web.WebView2.Wpf;

namespace TourPlanner.BL.Services.Map
{
    public class MapService : IMapService
    {
        private readonly IMapViewService _mapViewService;
        private object? _currentWebView;

        public MapService(IMapViewService mapViewService)
        {
            _mapViewService = mapViewService;
        }

        public void SetWebView(object webView)
        {
            _currentWebView = webView;
        }

        public async Task UpdateMapAsync(string startLocation, string endLocation)
        {
            if (_currentWebView != null)
            {
                await _mapViewService.UpdateMapAsync(_currentWebView, startLocation, endLocation);
            }
        }

        public async Task SaveMapImageAsync(string fullImagePath)
        {
            if (_currentWebView != null)
            {
                await _mapViewService.SaveMapImageAsync(_currentWebView, fullImagePath);
            }
        }
        public async Task ClearMapAsync()
        {
            if (_currentWebView is WebView2 webView && webView.CoreWebView2 != null)
            {
                await webView.CoreWebView2.ExecuteScriptAsync("clearMap()");
            }
        }
    }
}
