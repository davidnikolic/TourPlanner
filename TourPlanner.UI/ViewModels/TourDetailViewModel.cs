using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.Services.Map;

namespace TourPlanner.UI.ViewModels
{
    public class TourDetailViewModel : ViewModelBase
    {
        private readonly IMapService _mapService;
        private ISelectedTourService _selectedTourService;
        private readonly ITourStatisticsService _tourStatisticsService;
        private ITourLogService _tourLogService;

        private TourDTO selectedTour;

        private TourStatisticsDTO statistics;

        public TourStatisticsDTO Statistics
        {
            get => statistics;
            set
            {
                statistics = value;
                OnPropertyChanged();
            }
        }

        public TourDTO SelectedTour
        {
            get => selectedTour;
            set
            {
                if (selectedTour != value)
                {

                    selectedTour = value;
                    OnPropertyChanged();
                    ResetTabState(); // Reset map state to null
                    EvaluateLazyLoading(); // Triggers map update based on current tab selection
                }
            }
        }

        private ImageSource _mapImage;
        public ImageSource MapImage
        {
            get => _mapImage;
            set
            {
                _mapImage = value;
                OnPropertyChanged();
            }
        }

        private int _selectedTabIndex = 1;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();
                EvaluateLazyLoading();
            }
        }

        private bool _mapLoaded = false;

        public TourDetailViewModel
            (
            ISelectedTourService selectedTourService,
            IMapService mapService,
            ITourStatisticsService tourStatisticsService,
            ITourLogService tourLogService
            )
        {
            _selectedTourService = selectedTourService;
            _mapService = mapService;
            _tourStatisticsService = tourStatisticsService;
            _tourLogService = tourLogService;

            _selectedTourService.SelectedTourChanged += OnSelectedTourChanged;
        }

        private async Task EvaluateLazyLoading()
        {
            if (SelectedTabIndex == 1)
            {
                // This is the Map/Route Tab
                // Only load the static map image once
                if (!_mapLoaded)
                {
                    _mapLoaded = true;
                }
                string start = "";
                string end = "";

                if (SelectedTour != null)
                {
                    start = SelectedTour.StartLocation;
                    end = SelectedTour.EndLocation;
                }
                await _mapService.UpdateMapAsync(start, end);
                await _mapService.SaveMapImageAsync(selectedTour.RouteImagePath);
            }
            if (SelectedTabIndex == 2)
            {
                var logs = _tourLogService.GetTourLogsForTour(selectedTour.Id);

                if(logs.Count != 0 && SelectedTour != null) Statistics = _tourStatisticsService.CalculateTourStatistic(SelectedTour, logs);
            }
            else // if another tab is selcted
            {
                // Reset map
                MapImage = null; 
                _mapLoaded = false;
                statistics = null;
            } 
        }

        // Add this method to force refresh the map
        public async Task RefreshMapAsync()
        {
            if (SelectedTabIndex == 1 && SelectedTour != null)
            {
                ResetTabState();
                await EvaluateLazyLoading();
            }
        }

        private void ResetTabState()
        {
            MapImage = null;
            _mapLoaded = false;
        }

        private void OnSelectedTourChanged(TourDTO tour)
        {
            SelectedTour = tour;

            // If tour is null (deleted), reset the map completely
            if (tour == null)
            {
                ResetTabState();
                MapImage = null;
                // Force clear the map in the service as well
                _ = _mapService.UpdateMapAsync("", "");
            }
        }

    }
}
