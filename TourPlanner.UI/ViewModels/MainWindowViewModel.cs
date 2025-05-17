using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Services;
using TourPlanner.DAL.Entities;

namespace TourPlanner.UI.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private ITourService _tourService;


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

        public TourListViewModel TourListViewModel { get; }

        public MainWindowViewModel(ITourService tourService, TourListViewModel tourListViewModel)
        {
            _tourService = tourService;
            TourListViewModel = tourListViewModel;

            TourListViewModel.SelectedTourChanged += tour =>
            {
                SelectedTour = tour;
            };
        }
    }
}
