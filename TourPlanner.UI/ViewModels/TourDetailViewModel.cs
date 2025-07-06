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
using TourPlanner.Logging;
using TourPlanner.Logging.Interfaces;
using TourPlanner.UI.Interfaces;

namespace TourPlanner.UI.ViewModels
{
    public class TourDetailViewModel : ViewModelBase
    {
        private readonly ILoggerWrapper _logger;
        private readonly IMapService _mapService;
        private ISelectedTourService _selectedTourService;
        private readonly ITourStatisticsService _tourStatisticsService;
        private ITourLogService _tourLogService;
        private readonly IUITabNavigationService _tabNavigationService;

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
                    _logger.Debug($"Selected tour changed to: {value?.Name ?? "null"}");
                    selectedTour = value;
                    OnPropertyChanged();
                    ResetTabState(); // Reset map state to null
                    if (SelectedTabIndex == 1 || SelectedTabIndex == 2)
                    {
                        _logger.Debug("Executing Lazy Loading");
                        _ = EvaluateLazyLoading();
                    }
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
                if (_selectedTabIndex != value)
                {
                    _selectedTabIndex = value;
                    _tabNavigationService.SelectedTabIndex = value;
                    OnPropertyChanged();
                    _ = EvaluateLazyLoading();
                }
            }
        }


        private bool _mapLoaded = false;

        public TourDetailViewModel(
            ISelectedTourService selectedTourService, 
            IMapService mapService, 
            ITourStatisticsService tourStatisticsService, 
            ITourLogService tourLogService,
            IUITabNavigationService tabNavigationService,
            ILoggerFactory loggerFactory
            )
        {
            _logger = loggerFactory.CreateLogger<TourDetailViewModel>();
            _logger.Info("TourDetailViewModel initialized");

            _selectedTourService = selectedTourService;
            _tourStatisticsService = tourStatisticsService;
            _tourLogService = tourLogService;
            _mapService = mapService;
            _tabNavigationService = tabNavigationService;

            _selectedTourService.SelectedTourChanged += OnSelectedTourChanged;
            _selectedTabIndex = _tabNavigationService.SelectedTabIndex;

            _tabNavigationService.SelectedTabIndexChanged += (index) =>
            {
                SelectedTabIndex = index; 
            };
        }

        private async Task EvaluateLazyLoading()
        {
            _logger.Debug($"Evaluating lazy loading for tab index: {SelectedTabIndex}");
            // This is the Map/Route Tab
            if (SelectedTabIndex == 1)
            {
                // Check if we have a valid tour selected
                if (SelectedTour == null)
                {
                    _logger.Debug("No tour selected - clearing map");
                    MapImage = null;
                    _mapLoaded = false;
                    // Clear the map service as well
                    try
                    {
                        await _mapService.ClearMapAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Failed to clear map", ex);
                    }
                    return;
                }
                if (!_mapLoaded)
                {
                    _logger.Debug("Loading map for selected tour");
                    string start = SelectedTour.StartLocation;
                    string end = SelectedTour.EndLocation;

                    try
                    {
                        await _mapService.UpdateMapAsync(start, end);
                        _mapLoaded = true;
                        _logger.Info($"Map updated successfully for route: {start} to {end}");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Failed to update map for route: {start} to {end}", ex);
                        _mapLoaded = false;
                    }
                }
            }
            else // if another tab is selcted
            {
                _logger.Debug("Resetting map - different tab selected");
                // Reset map
                MapImage = null;
                _mapLoaded = false;
                try
                {
                    await _mapService.ClearMapAsync();
                }
                catch (Exception ex)
                {
                    _logger.Error("Failed to clear map on tab change", ex);
                }
            }

            if(SelectedTabIndex == 2)
            {
                if (SelectedTour == null) return;

                var logs = _tourLogService.GetTourLogsForTour(SelectedTour.Id);
                Statistics = _tourStatisticsService.CalculateTourStatistic(SelectedTour, logs);
            }
        }

        private void ResetTabState()
        {
            _logger.Debug("Resetting tab state");
            MapImage = null;
            _mapLoaded = false;
            Statistics = null;
        }

        private async void OnSelectedTourChanged(TourDTO tour)
        {
            SelectedTour = tour;

            // If tour is null (deleted), immediately clear the map if we're on the route tab
            if (tour == null)
            {
                _logger.Debug("Tour deleted - clearing map immediately");
                ResetTabState();

                // If we're currently on the route tab, clear the map immediately
                if (SelectedTabIndex == 1)
                {
                    try
                    {
                        await _mapService.ClearMapAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Failed to clear map after tour deletion", ex);
                    }
                }
            }
        }

    }
}
