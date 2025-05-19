using System;
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
using TourPlanner.UI.Views.Components;

namespace TourPlanner.UI.ViewModels
{
    public class TourLogsViewModel : ViewModelBase
    {
        private ITourLogService _tourLogService;

        private ISelectedTourService _selectedTourService;
        public ObservableCollection<TourLogDTO> TourLogs { get; set; } = new();

        private TourDTO selectedTour;

        private TourLogDTO selectedLog;

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

        public TourLogsViewModel(ITourLogService tourLogService, ISelectedTourService selectedTourService) { 
            _tourLogService = tourLogService;
            _selectedTourService = selectedTourService;

            _selectedTourService.SelectedTourChanged += OnSelectedTourChanged;
        }

        public RelayCommand AddCommand => new RelayCommand(execute => AddTourLog());

        public RelayCommand ModifyCommand => new RelayCommand(execute => ModifyTourLog());

        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteTourLog());
        private void AddTourLog()
        {
            if (SelectedTour == null || SelectedTour.Id <= 0)
            {
                MessageBox.Show("Bitte zuerst eine gültige Tour auswählen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new TourLogDialogWindowView();
            var vm = new TourLogDialogViewModel("Add new TourLog");

            dialog.DataContext = vm;

            vm.CloseRequested += () => dialog.DialogResult = true;

            if (dialog.ShowDialog() == true)
            {
                if (vm.Result is TourLogDTO newTourLog)
                {
                    newTourLog.TourId = SelectedTour.Id;
                    _tourLogService.AddTourLog(newTourLog);
                }
            }

            RefreshTourLogs();
        }

        public void ModifyTourLog()
        {
            if (selectedLog != null)
            {
                var dialog = new TourLogDialogWindowView();
                var vm = new TourLogDialogViewModel("Modify Log", SelectedLog);

                dialog.DataContext = vm;

                vm.CloseRequested += () => dialog.DialogResult = true;

                if (dialog.ShowDialog() == true)
                {
                    if (vm.Result is TourLogDTO changedTourLog)
                    {
                        _tourLogService.UpdateTourLog(changedTourLog);
                    }
                }

                RefreshTourLogs();
            }
        }

        public void DeleteTourLog()
        {
            if (selectedLog != null)
            {
                _tourLogService?.DeleteTourLog(SelectedLog);

                RefreshTourLogs();
            }
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

        private void RefreshTourLogs()
        {
            var tourLogs = _tourLogService.GetTourLogsForTour(SelectedTour.Id);
            TourLogs.Clear();


            foreach (var tl in tourLogs)
                TourLogs.Add(tl);
        }
    }
}
