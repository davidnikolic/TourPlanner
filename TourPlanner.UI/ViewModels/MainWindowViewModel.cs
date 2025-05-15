using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.Models;
using TourPlanner.BL.Services;
using TourPlanner.DAL.Entities;

namespace TourPlanner.UI.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private ITourService _tourService;

        public ObservableCollection<Tour> Tours { get; set; }

        public MainWindowViewModel(ITourService tourService)
        {
            _tourService = tourService;
            
            var tours = _tourService.GetTours();
            Tours = new ObservableCollection<Tour>(tours);
        }

        private Tour selectedTour;

        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                selectedTour = value;
                OnPropertyChanged();
            }
        }
    }
}
