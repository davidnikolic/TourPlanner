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
using TourPlanner.Logging;
using TourPlanner.Logging.Interfaces;
using TourPlanner.BL.Services;
using TourPlanner.UI.Interfaces;
using TourPlanner.UI.Views.Components;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TourPlanner.UI.ViewModels
{
    public class TourLogsViewModel : ViewModelBase
    {
        private readonly ILoggerWrapper _logger;
        private ITourLogService _tourLogService;

        private ISelectedTourService _selectedTourService;

        private ISearchService _searchService;

        private IDialogService _dialogService;
        public ObservableCollection<TourLogDTO> TourLogs { get; set; } = new();

        private TourDTO selectedTour;

        private TourLogDTO selectedLog;

        public string SearchQuery
        {
            get => _searchService.CurrentSearchTerm;
            set
            {
                if (_searchService.CurrentSearchTerm != value)
                {
                    _logger.Debug($"Search query changed to: '{value}'");
                    _searchService.CurrentSearchTerm = value;
                    OnPropertyChanged();
                    RefreshTourLogs();
                }
            }
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
            IDialogService dialogService,
            ISearchService searchService,
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TourLogsViewModel>();
            _logger.Info("TourLogsViewModel initialized");
            _tourLogService = tourLogService;
            _selectedTourService = selectedTourService;
            _dialogService = dialogService;
            _searchService = searchService;

            _selectedTourService.SelectedTourChanged += OnSelectedTourChanged;
            _searchService.SearchTermChanged += OnSearchTermChanged;
        }

        public RelayCommand AddCommand => new RelayCommand(execute => AddTourLog());

        public RelayCommand ModifyCommand => new RelayCommand(execute => ModifyTourLog());

        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteTourLog());
        private void AddTourLog()
        {
            if (SelectedTour == null || SelectedTour.Id <= 0)
            {
                _logger.Debug("Add tour log failed - no valid tour selected");
                MessageBox.Show("Please select a valid tour first", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _logger.Info($"Adding new tour log for tour: {SelectedTour.Name}");
            var tourLog = _dialogService.DisplayTourLogPopUp("Add new Tour Log");

            if (tourLog != null)
            {
                tourLog.TourId = selectedTour.Id;
                try
                {
                    _tourLogService.AddTourLog(tourLog);
                    _logger.Info("Tour log added successfully");
                }
                catch (Exception ex)
                {
                    _logger.Error("Failed to add tour log", ex);
                }
            }

            RefreshTourLogs();
        }

        public void ModifyTourLog()
        {
            if (selectedLog == null)
            {
                _logger.Debug("Modify tour log failed - no log selected");
                MessageBox.Show("Please select a tour log first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _logger.Info($"Modifying tour log ID: {selectedLog.Id}");
            var tourLog = _dialogService.DisplayTourLogPopUp("Edit Tour Log", selectedLog);

            if (tourLog != null)
            {
                tourLog.TourId = selectedTour.Id;
                tourLog.Id = selectedLog.Id;
                try
                {
                    _tourLogService.UpdateTourLog(tourLog);
                    _logger.Info($"Tour log updated successfully: ID {tourLog.Id}");
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to update tour log ID: {tourLog.Id}", ex);
                }
            }

            RefreshTourLogs();
        }

        public void DeleteTourLog()
        {
            if (selectedLog == null)
            {
                _logger.Debug("Delete tour log failed - no log selected");
                MessageBox.Show("Please select a tour log first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _logger.Info($"Deleting tour log ID: {selectedLog.Id}");
            try
            {
                _tourLogService?.DeleteTourLog(SelectedLog);
                _logger.Info($"Tour log deleted successfully: ID {selectedLog.Id}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to delete tour log ID: {selectedLog.Id}", ex);
            }
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
            RefreshTourLogs();
        }

        private void OnSearchTermChanged(object sender, EventArgs e)
        {
            RefreshTourLogs();
        }

        private void RefreshTourLogs()
        {
            _logger.Debug("Refreshing tour logs");
            List<TourLogDTO> filteredLogs;

            string searchTerm = _searchService.CurrentSearchTerm;

            if (SelectedTour == null)
            {
                // If no tour is selected, show all matching logs system-wide
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    _logger.Debug("No tour selected - showing all matching logs");
                    filteredLogs = _tourLogService.SearchTourLogs(searchTerm);
                }
                else
                {
                    // No tour selected and no search query → clear the list
                    _logger.Debug("No tour selected and no search query - clearing logs");
                    TourLogs.Clear();
                    return;
                }
            }
            else
            {
                // A tour is selected -> show all logs for this tour dont care about search term
                _logger.Debug($"Loading all logs for selected tour: {SelectedTour.Name}");
                filteredLogs = _tourLogService.GetTourLogsForTour(SelectedTour.Id);
            }

            // Clear current list of logs in the UI and add filtered logs
            TourLogs.Clear();
            foreach (var tl in filteredLogs)
                TourLogs.Add(tl);

            _logger.Info($"Tour logs refreshed. Displaying {TourLogs.Count} logs for tour: {SelectedTour?.Name ?? "None"}");
        }
    }
}
