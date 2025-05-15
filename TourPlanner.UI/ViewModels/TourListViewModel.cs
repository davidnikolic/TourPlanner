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
using TourPlanner.UI.Views.Components;

namespace TourPlanner.UI.ViewModels
{
    public class TourListViewModel : ViewModelBase
    {

        private ITourService _tourService;

        public ObservableCollection<Tour> Tours { get; set; } = new();

        public event Action<Tour>? SelectedTourChanged;

        private Tour selectedTour;
        public Tour SelectedTour 
        { 
            get => selectedTour;
            set
            {
                selectedTour = value;
                OnPropertyChanged();
                SelectedTourChanged?.Invoke(value);
            }
        }

        public TourListViewModel(ITourService tourService)
        {
            _tourService = tourService;

            var tours = _tourService.GetTours();

            Tours = new ObservableCollection<Tour>(tours);
        }

        public RelayCommand AddCommand => new RelayCommand(execute => AddTour());
        public RelayCommand ModifyCommand => new RelayCommand(execute => ModifyTour());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteTour());

        private void AddTour()
        {
            var dialog = new DialogWindowView();
            var vm = new DialogViewModel("Add new Tour");

            dialog.DataContext = vm;

            vm.CloseRequested += () => dialog.DialogResult = true;

            if (dialog.ShowDialog() == true)
            {
                if (vm.Result is Tour newTour)
                {
                    _tourService.AddTour(newTour);
                }
            }

            RefreshTours();
        }

        private void ModifyTour()
        {
            var dialog = new DialogWindowView();
            var vm = new DialogViewModel("Modify Tour", SelectedTour);

            dialog.DataContext = vm;

            vm.CloseRequested += () => dialog.DialogResult = true;

            if (dialog.ShowDialog() == true)
            {
                if (vm.Result is Tour changedTour)
                {
                    _tourService.UpdateTour(changedTour);
                }
            }

            RefreshTours();
        }

        private void DeleteTour()
        {
            _tourService.DeleteTour(SelectedTour);
            RefreshTours();
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
