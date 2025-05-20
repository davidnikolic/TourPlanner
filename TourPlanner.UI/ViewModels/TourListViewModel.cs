using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Interfaces;
using TourPlanner.UI.Interfaces;
using TourPlanner.BL.DTOs;
using TourPlanner.UI.Views.Components;
using TourPlanner.BL.Services;


namespace TourPlanner.UI.ViewModels
{
    public class TourListViewModel : ViewModelBase
    {

        private ITourService _tourService;

        private ISelectedTourService? _selectedTourService;

        private IDialogService _dialogService;

        public ObservableCollection<TourDTO> Tours { get; set; } = new();

        private TourDTO selectedTour;
        public TourDTO SelectedTour
        {
            get => selectedTour;
            set
            {
                selectedTour = value;
                OnPropertyChanged();
                _selectedTourService.SelectedTour = value;
            }
        }

        public TourListViewModel(
            ITourService tourService,
            ISelectedTourService selectedTourService,
            IDialogService dialogService
            )
        {
            _tourService = tourService;
            _selectedTourService = selectedTourService;
            _dialogService = dialogService;

            var tours = _tourService.GetTours();

            Tours = new ObservableCollection<TourDTO>(tours);
        }

        public RelayCommand AddCommand => new RelayCommand(execute => AddTour());
        public RelayCommand ModifyCommand => new RelayCommand(execute => ModifyTour());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteTour());

        private void AddTour()
        {
            var tour = _dialogService.DisplayTourPopUp("Add new tour");

            if (tour != null) _tourService.AddTour(tour);

            RefreshTours();
        }

        private void ModifyTour()
        {
            var tour = _dialogService.DisplayTourPopUp("Modify Tour", SelectedTour);
            if (tour != null) _tourService.UpdateTour(tour);

            RefreshTours();

        }

        private void DeleteTour()
        {
            if (SelectedTour != null)
            {
                _tourService.DeleteTour(SelectedTour);
                _selectedTourService.SelectedTour = null;
                RefreshTours();
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
