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
using TourPlanner.UI.Map;

namespace TourPlanner.UI.ViewModels
{
    public class TourDetailViewModel : ViewModelBase
    {
        private ISelectedTourService _selectedTourService;

        private TourDTO selectedTour;

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

        private void EvaluateLazyLoading()
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
                // Request a map update via event service using start and end locations
                MapEventService.RequestMapUpdate(start, end);
            }
            else // if another tab is selcted
            {
                // Reset map
                MapImage = null; 
                _mapLoaded = false;
            }
        }

        public TourDetailViewModel(ISelectedTourService selectedTourService)
        {
            _selectedTourService = selectedTourService;

            _selectedTourService.SelectedTourChanged += OnSelectedTourChanged;
        }

        private void ResetTabState()
        {
            MapImage = null;
            _mapLoaded = false;
        }

        private void OnSelectedTourChanged(TourDTO tour)
        {
            SelectedTour = tour;
        }
    }
}
