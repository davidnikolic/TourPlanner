using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.DTOs;
using TourPlanner.UI.Views.Components;
using TourPlanner.BL.Services;

namespace TourPlanner.UI.ViewModels
{
    public class TourListViewModel : ViewModelBase
    {

        private ITourService _tourService;

        private ISelectedTourService? _selectedTourService;

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

        public TourListViewModel(ITourService tourService, ISelectedTourService selectedTourService)
        {
            _tourService = tourService;
            _selectedTourService = selectedTourService;

            var tours = _tourService.GetTours();

            Tours = new ObservableCollection<TourDTO>(tours);
        }

        public RelayCommand AddCommand => new RelayCommand(execute => AddTour());
        public RelayCommand ModifyCommand => new RelayCommand(execute => ModifyTour());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteTour());

        private void AddTour()
        {
            var dialog = new TourDialogWindowView();
            var vm = new TourDialogViewModel("Add new Tour");

            dialog.DataContext = vm;

            vm.CloseRequested += () => dialog.DialogResult = true;

            if (dialog.ShowDialog() == true)
            {
                if (vm.Result is TourDTO newTour)
                {
                    _tourService.AddTour(newTour);
                }
            }

            RefreshTours();
        }

        private void ModifyTour()
        {
            if (SelectedTour != null)
            {
                var dialog = new TourDialogWindowView();
                var vm = new TourDialogViewModel("Modify Tour", SelectedTour);

                dialog.DataContext = vm;

                vm.CloseRequested += () => dialog.DialogResult = true;

                if (dialog.ShowDialog() == true)
                {
                    if (vm.Result is TourDTO changedTour)
                    {
                        _tourService.UpdateTour(changedTour);
                    }
                }

                RefreshTours();
            }
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
