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
            Loaded += OnLoaded;
        }
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var mapViewService = ((App)Application.Current).Services.GetRequiredService<IMapViewService>();
            var mapService = ((App)Application.Current).Services.GetRequiredService<IMapService>();

            await mapViewService.InitializeMapAsync(MapWebView);

            // WebView an MapService weitergeben
            if (mapService is MapService concreteMapService)
            {
                concreteMapService.SetWebView(MapWebView);
            }
        }

    }
}
