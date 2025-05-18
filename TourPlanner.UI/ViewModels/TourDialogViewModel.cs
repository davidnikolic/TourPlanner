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
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Services;
using TourPlanner.DAL.Entities;
using static TourPlanner.DAL.Entities.Enums;
using TourPlanner.UI;

namespace TourPlanner.UI.ViewModels
{
    public class TourDialogViewModel : ViewModelBase // interface for changing elements
    {

        public event Action? CloseRequested;

        public TourDTO tour { get; set; } = new();

        private int? id;
        public int? Id 
        { 
            get => id;
            set { id = value; OnPropertyChanged(); } 
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
        public TourDTO? Result { get; private set; }

        public List<TransportType> TransportTypes { get; set; } // List of transport types dropdown

        public string PopUpText { get; private set; } = "";

        //ICommand for the AddTour-Method.
        public ICommand AddTourCommand { get; }

        public TourDialogViewModel(string text, TourDTO? existingTour = null)
        {
            PopUpText = text;

            AddTourCommand = new RelayCommand(_ => AddTour());

            TransportTypes = Enum.GetValues(typeof(TransportType)).Cast<TransportType>().ToList();

            if (existingTour != null)
            {
                tour = existingTour;

                // Vorbelegen der Felder
                Id = tour.Id;
                Name = tour.Name;
                Description = tour.Description;
                StartLocation = tour.StartLocation;
                EndLocation = tour.EndLocation;
                SelectedTransportType = tour.TransportType;
                Distance = tour.DistanceKm;
                EstimatedTime = tour.EstimatedTimeHours;
            }
        }

        private void AddTour()
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

            Result = new TourDTO
            {
                Name = Name,
                Description = Description,
                StartLocation = StartLocation,
                EndLocation = EndLocation,
                TransportType = SelectedTransportType,
                DistanceKm = Distance,
                EstimatedTimeHours = EstimatedTime,
            };

            if (Id.HasValue)
                Result.Id = Id.Value;

            CloseRequested?.Invoke();
        }
    }
}
