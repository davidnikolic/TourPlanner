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
                selectedTour = value;
                OnPropertyChanged();

                ResetTabState();  
                EvaluateLazyLoading();
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

        private int _selectedTabIndex = 0;
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
            if (SelectedTour == null) return;


            if (SelectedTabIndex == 1 && !_mapLoaded)
            {
                LoadMapImage();
                _mapLoaded = true;
            }

            if (SelectedTabIndex != 1)
            {
                MapImage = null;
                _mapLoaded = false;
            }
        }

        public TourDetailViewModel(ISelectedTourService selectedTourService)
        {
            _selectedTourService = selectedTourService;

            _selectedTourService.SelectedTourChanged += OnSelectedTourChanged;
        }


        private void LoadMapImage()
        {
            string imagePath = SelectedTour?.RouteImagePath ?? @"C:\Users\David\Downloads\political-world-map-1.jpg";

            if (File.Exists(imagePath))
            {
                MapImage = new BitmapImage(new Uri(imagePath));
            }
            else
            {
                MapImage = null;
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
        }
    }
}
