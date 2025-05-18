using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            }
        }


        public TourDetailViewModel(ISelectedTourService selectedTourService)
        {
            _selectedTourService = selectedTourService;

            _selectedTourService.SelectedTourChanged += OnSelectedTourChanged;
        }

        private void OnSelectedTourChanged(TourDTO tour)
        {
            SelectedTour = tour;
        }
    }
}
