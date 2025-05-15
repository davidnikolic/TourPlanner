using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.Services;
using TourPlanner.DAL.Entities;
using static TourPlanner.DAL.Entities.Enums;

namespace TourPlanner.UI.ViewModels
{
    public class AddTourViewModel : INotifyPropertyChanged // interface for changing elements
    {
        public event EventHandler RequestClose;

        public ICommand AddTourCommand { get; } // Button click

        public event PropertyChangedEventHandler PropertyChanged;
        public List<TransportType> TransportTypes { get; set; } // List of transport types dropdown
        private readonly ITourService _tourService;
        private readonly TourListViewModel _tourListViewModel;

        public AddTourViewModel(ITourService tourService, TourListViewModel tourListViewModel)
        {
            _tourService = tourService;
            _tourListViewModel = tourListViewModel;

            // Initialize the command and assign the async method to be executed
            //AddTourCommand = new RelayCommand(async () => await AddTourAsync());

            TransportTypes = Enum.GetValues(typeof(TransportType)).Cast<TransportType>().ToList();
        }

        // Form input fields as properties, all notify UI when changed 
        private string name;
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        private string description;
        public string Description
        {
            get => description;
            set { description = value; OnPropertyChanged(); }
        }

        private string startLocation;
        public string StartLocation
        {
            get => startLocation;
            set { startLocation = value; OnPropertyChanged(); }
        }

        private string endLocation;
        public string EndLocation
        {
            get => endLocation;
            set { endLocation = value; OnPropertyChanged(); }
        }

        private TransportType selectedTransportType;
        public TransportType SelectedTransportType
        {
            get => selectedTransportType;
            set { selectedTransportType = value; OnPropertyChanged(); }
        }

        private float distance;
        public float Distance
        {
            get => distance;
            set { distance = value; OnPropertyChanged(); }
        }

        private float estimatedTime;
        public float EstimatedTime
        {
            get => estimatedTime;
            set { estimatedTime = value; OnPropertyChanged(); }
        }

        private async Task AddTourAsync()
        {
            // Validation check
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Description) ||
                string.IsNullOrWhiteSpace(StartLocation) ||
                string.IsNullOrWhiteSpace(EndLocation) ||
                !Enum.IsDefined(typeof(TransportType), SelectedTransportType) ||
                Distance <= 0 ||
                EstimatedTime <= 0)
                {
                    MessageBox.Show("Please fill out all fields correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

            //await _tourService.AddTour(new TourEntity
            //{
            //    Name = Name,
            //    Description = Description,
            //    StartLocation = StartLocation,
            //    EndLocation = EndLocation,
            //    TransportType = SelectedTransportType,
            //    DistanceKm = Distance,
            //    EstimatedTimeHours = EstimatedTime,
            //});

            //_tourListViewModel.AddTourName(Name);
            // Notify close window, actually closed in addtourdialaog.xaml.cs 
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        // Notify the UI that a property has changed (used for data binding updates)
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
