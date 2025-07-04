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
using System.Windows.Input;
using TourPlanner.BL.Services.Map;


namespace TourPlanner.UI.ViewModels
{
    public class TourListViewModel : ViewModelBase
    {
        private ITourService _tourService;

        private ISelectedTourService? _selectedTourService;

        private IDialogService _dialogService;

        private ITourCoordinatorService _tourCoordinatorService;

        private ISearchService _searchService;

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
            ITourCoordinatorService tourCoordinatorService,
            ISearchService searchService
            )
        {
            _tourService = tourService;
            _selectedTourService = selectedTourService;
            _dialogService = dialogService;
            _tourCoordinatorService = tourCoordinatorService;
            _searchService = searchService;

            _tourCoordinatorService.ToursChanged += RefreshTours;
            _searchService.SearchTermChanged += (s, e) => RefreshTours();

            var tours = _tourService.GetTours();
            Tours = new ObservableCollection<TourDTO>(tours);
            RefreshTours();
        }

        public RelayCommand AddCommand => new RelayCommand(execute => AddTour());
        public RelayCommand ModifyCommand => new RelayCommand(execute => ModifyTour());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteTour());

        private void AddTour()
        {
            _tourCoordinatorService.AddTour();
        }

        private void ModifyTour()
        {
            if (SelectedTour == null || SelectedTour.Id <= 0)
            {
                MessageBox.Show("Please select a valid tour first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var tour = _dialogService.DisplayTourPopUp("Modify Tour", SelectedTour);

            if (tour != null)
            {
                tour.Id = SelectedTour.Id;
                _tourService.UpdateTour(tour);
                
            }

            RefreshTours();

        }

        private void DeleteTour()
        {
            if (SelectedTour == null || SelectedTour.Id <= 0)
            {
                MessageBox.Show("Please select a valid tour first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show($"Do you really want to delete '{SelectedTour.Name}' tour", "Delete Tour", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _tourService.DeleteTour(SelectedTour);
                _selectedTourService.SelectedTour = null; // Reset selection, as the tour has been deleted
                RefreshTours();
            }
        }

        public void RefreshTours()
        {
            int id = -1;
            if (selectedTour != null)
            {
                id = selectedTour.Id;
            }


            List<TourDTO> toursToDisplay;
            // Get the current search term from the SearchService
            string currentSearchTerm = _searchService.CurrentSearchTerm;

            if (!string.IsNullOrWhiteSpace(currentSearchTerm))
            {
                id = -1;
                // If a search term is provided, filter tours using SearchTours()
                toursToDisplay = _searchService.SearchTours(currentSearchTerm);
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
            if (id != -1 && Tours.Any(t => t.Id == id))
            {
                // Set SelectedTour to the one that was previously selected and is still available / first or exception
                SelectedTour = Tours.First(t => t.Id == id);
            }
            else
            {
                // Otherwise, select the first item in the list or set to null if none exist  / First Tour or "null" no tour displayed
                SelectedTour = null; 
            }
        }
    }
}
