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
using System.Windows;


namespace TourPlanner.UI.ViewModels
{
    public class TourListViewModel : ViewModelBase
    {

        private ITourService _tourService;

        private ISelectedTourService? _selectedTourService;

        private IDialogService _dialogService;

        private TourLogsViewModel _tourLogsViewModel;

        public ObservableCollection<TourDTO> Tours { get; set; } = new();

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
                    _selectedTourService.SelectedTour = value; 
                }
            }
        }

        public TourListViewModel(
            ITourService tourService,
            ISelectedTourService selectedTourService,
            IDialogService dialogService,
            TourLogsViewModel tourLogsViewModel
            )
        {
            _tourService = tourService;
            _selectedTourService = selectedTourService;
            _dialogService = dialogService;
            _tourLogsViewModel = tourLogsViewModel;
            _tourLogsViewModel.PropertyChanged += OnTourLogsViewModelPropertyChanged;
            var tours = _tourService.GetTours();

            Tours = new ObservableCollection<TourDTO>(tours);
            RefreshTours();
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
            _tourLogsViewModel.PropertyChanged += OnTourLogsViewModelPropertyChanged;
            var tours = _tourService.GetTours();

            Tours = new ObservableCollection<TourDTO>(tours);
            RefreshTours();
        }

        private void OnTourLogsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Check if the property that changed is "SearchQuery"
            // This means the user typed something into the TourLog search field
            if (e.PropertyName == nameof(TourLogsViewModel.SearchQuery))
            {
                // When the search query changes, we need to update the tour list accordingly
                RefreshTours();
            }
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
            if (SelectedTour == null || SelectedTour.Id <= 0)
            {
                MessageBox.Show("Bitte zuerst eine gültige Tour auswählen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var tour = _dialogService.DisplayTourPopUp("Modify Tour", SelectedTour);
            if (tour != null) _tourService.UpdateTour(tour);

            RefreshTours();

        }

        private void DeleteTour()
        {
            if (SelectedTour == null || SelectedTour.Id <= 0)
            {
                MessageBox.Show("Bitte zuerst eine gültige Tour auswählen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show($"Möchtest du Tour '{SelectedTour.Name}' wirklich löschen?", "Tour löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _tourService.DeleteTour(SelectedTour);
                _selectedTourService.SelectedTour = null; // Auswahl zurücksetzen, da die Tour gelöscht wurde
                RefreshTours();
            }
        }

        public void RefreshTours()
        {
            List<TourDTO> toursToDisplay;
            // Get the current search term from the TourLogsViewModel
            string currentSearchTerm = _tourLogsViewModel.SearchQuery; 

            if (!string.IsNullOrWhiteSpace(currentSearchTerm))
            {
                // If a search term is provided, filter tours using SearchTours()
                toursToDisplay = _tourService.SearchTours(currentSearchTerm);
            }
            else
            {
                // If no search term is given, load all tours
                toursToDisplay = _tourService.GetTours();
            }

            Tours.Clear();
            // Add each filtered or unfiltered tour to the list
            foreach (var tour in toursToDisplay)
                Tours.Add(tour);

            // Try to keep the previously selected tour if it's still in the new list, current selected tour in tourslist
            if (_selectedTourService.SelectedTour != null && Tours.Any(t => t.Id == _selectedTourService.SelectedTour.Id))
            {
                // Set SelectedTour to the one that was previously selected and is still available / first or exception
                SelectedTour = Tours.First(t => t.Id == _selectedTourService.SelectedTour.Id);
            }
            else
            {
                // Otherwise, select the first item in the list or set to null if none exist  / First Tour or "null" no tour displayed
                SelectedTour = Tours.FirstOrDefault(); 
            }
        }

    }
}
