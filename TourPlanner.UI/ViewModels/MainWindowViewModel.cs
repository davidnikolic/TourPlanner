using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Models;
using TourPlanner.DAL.Entities;

namespace TourPlanner.UI.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Tour> Tours { get; set; }

        public MainWindowViewModel()
        {
            Tours = new ObservableCollection<Tour>();

            Tours.Add(new Tour()
            {
                Name = "Harrachpark",
                DistanceKm = 10,
            });
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
