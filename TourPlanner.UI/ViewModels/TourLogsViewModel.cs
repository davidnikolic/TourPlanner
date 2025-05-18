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

        public TourDTO SelectedTour
        {
            get => selectedTour;
            set
            {
                selectedTour = value;
                OnPropertyChanged();
            }
        }

        public TourLogsViewModel(ITourLogService tourLogService, ISelectedTourService selectedTourService) { 
            _tourLogService = tourLogService;
            _selectedTourService = selectedTourService;

            _selectedTourService.SelectedTourChanged += OnSelectedTourChanged;
        }

        public RelayCommand AddCommand => new RelayCommand(execute => AddTourLog());

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

        private void OnSelectedTourChanged(TourDTO tour)
        {
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
