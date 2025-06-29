using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Core;
using TourPlanner.BL.Services;
using TourPlanner.BL.Services.Map;
using TourPlanner.DAL.Repositories;
using TourPlanner.DAL;
using TourPlanner.UI.Map;
using TourPlanner.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.BL.Interfaces;
using TourPlanner.UI.Map.Interface;

namespace TourPlanner.UI.Views.Components
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        private readonly IMapViewService _mapService;

        public MapView()
        {
            InitializeComponent();
            _mapService = ((App)Application.Current).Services.GetRequiredService<IMapViewService>();

            MapWebView.Loaded += async (sender, e) => await _mapService.InitializeMapAsync(MapWebView);
            MapEventService.UpdateMapRequested += async (start, end) => await _mapService.UpdateMapAsync(MapWebView, start, end);
            MapEventService.SaveMapImageRequested += async (fullImagePath) => await _mapService.SaveMapImageAsync(MapWebView, fullImagePath);
        }

    }
}
