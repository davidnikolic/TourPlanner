using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.Models;

namespace TourPlanner.UI.ViewModels
{
    public class TourListViewModel : ViewModelBase
    {

        private ITourService _tourService;

        public ObservableCollection<Tour> Tours { get; set; } = new();

        public TourListViewModel(ITourService tourService)
        {
            _tourService = tourService;

            var tours = _tourService.GetTours();

            Tours = new ObservableCollection<Tour>(tours);
        }

        public RelayCommand AddCommand => new RelayCommand(execute => AddTour());

        private void AddTour()
        {
            _tourService.AddTour(new Tour() { Name = "Test" });
            RefreshTours();
        }

        private Tour selectedTour;

        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                selectedTour = value;
                OnPropertyChanged();
            }
        }

        private void RefreshTours()
        {
            var tours = _tourService.GetTours();
            Tours.Clear();
            foreach (var tour in tours)
                Tours.Add(tour);
        }

    }
}
