using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TourPlanner.UI.Map.Interface
{
    public interface IMapViewService
    {
        Task InitializeMapAsync(object webView);
        Task UpdateMapAsync(object webView, string startLocation, string endLocation);
        Task SaveMapImageAsync(object webView, string fullImagePath);
    }
}
