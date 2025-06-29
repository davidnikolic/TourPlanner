using System;
using System.Buffers.Text;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.Services;
using TourPlanner.UI.Interfaces;
using TourPlanner.UI.Views.Components;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TourPlanner.UI.ViewModels
{
    public class TourLogsViewModel : ViewModelBase
    {
        private ITourLogService _tourLogService;

        private ISelectedTourService _selectedTourService;

        private IDialogService _dialogService;
        public ObservableCollection<TourLogDTO> TourLogs { get; set; } = new();

        private TourDTO selectedTour;

        private TourLogDTO selectedLog;

        private string _searchQuery = string.Empty;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                // Only update if the value has actually changed to avoid unnecessary UI updates
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    // Notify the UI that this property has changed (data binding)
                    OnPropertyChanged();
                    // Execute the search logic and filter TourLogs based on the new query
                    ExecuteSearch(); 
                }
            }
        }

        private void ExecuteSearch()
        {
            var result = _tourLogService.SearchTourLogs(SearchQuery);
            // If SelectedTour set → show log for the TOUR
            if (SelectedTour != null)
            {
                result = result.Where(log => log.TourId == SelectedTour.Id).ToList();
            }

            // Set List
            TourLogs.Clear();
            foreach (var log in result)
                TourLogs.Add(log);
        }

        public TourDTO SelectedTour
        {
            get => selectedTour;
            set
            {
                selectedTour = value;
                OnPropertyChanged();
            }
        }

        public TourLogDTO SelectedLog
        {
            get => selectedLog;
            set
            {
                selectedLog = value;
                OnPropertyChanged();
            }
        }

        public TourLogsViewModel(
            ITourLogService tourLogService,
            ISelectedTourService selectedTourService,
            IDialogService dialogService)
        {

            _tourLogService = tourLogService;
            _selectedTourService = selectedTourService;
            _dialogService = dialogService;

            _selectedTourService.SelectedTourChanged += OnSelectedTourChanged;
        }

        public RelayCommand AddCommand => new RelayCommand(execute => AddTourLog());

        public RelayCommand ModifyCommand => new RelayCommand(execute => ModifyTourLog());

        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteTourLog());
        private void AddTourLog()
        {
            if (SelectedTour == null || SelectedTour.Id <= 0)
            {
                MessageBox.Show("Please select a valid tour first", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var tourLog = _dialogService.DisplayTourLogPopUp("Add new Tour Log");

            if (tourLog != null)
            {
                tourLog.TourId = selectedTour.Id;
                _tourLogService.AddTourLog(tourLog);
            }

            RefreshTourLogs();
        }

        public void ModifyTourLog()
        {
            if (selectedLog == null)
            {
                MessageBox.Show("Please select a tour log first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var tourLog = _dialogService.DisplayTourLogPopUp("Edit Tour Log", selectedLog);

            if (tourLog != null)
            {
                tourLog.TourId = selectedTour.Id;
                tourLog.Id = selectedLog.Id;
                _tourLogService.UpdateTourLog(tourLog);
            }

            RefreshTourLogs();
        }

        public void DeleteTourLog()
        {
            if (selectedLog == null)
            {
                MessageBox.Show("Please select a tour log first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _tourLogService?.DeleteTourLog(SelectedLog);

            RefreshTourLogs();

        }

        private void OnSelectedTourChanged(TourDTO tour)
        {
            if (tour == null)
            {
                SelectedTour = null;
                TourLogs.Clear();
                return;
            }

            SelectedTour = tour;
            _searchQuery = string.Empty;
            RefreshTourLogs();
        }

        private void RefreshTourLogs()
        {
            List<TourLogDTO> filteredLogs;

            if (SelectedTour == null)
            {
                // If no tour is selected, show all matching logs system-wide
                if (!string.IsNullOrWhiteSpace(_searchQuery))
                {
                    filteredLogs = _tourLogService.SearchTourLogs(_searchQuery);
                }
                else
                {
                    // No tour selected and no search query → clear the list
                    TourLogs.Clear(); 
                    return;
                }
            }
            else
            {
                // A tour is selected → only show logs related to this specific tour
                if (!string.IsNullOrWhiteSpace(_searchQuery))
                {
                    filteredLogs = _tourLogService.SearchTourLogs(_searchQuery)
                                                  .Where(log => log.TourId == SelectedTour.Id)
                                                  .ToList();
                }
                else
                {
                    // No active search → just load all logs for the selected tour
                    filteredLogs = _tourLogService.GetTourLogsForTour(SelectedTour.Id);
                }
            }
            // At the end clear current list of logs in the UI
            // Add each filtered log to the UI
            TourLogs.Clear();
            foreach (var tl in filteredLogs)
                TourLogs.Add(tl);
        }
    }
}
