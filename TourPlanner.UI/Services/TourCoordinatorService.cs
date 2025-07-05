using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;
using TourPlanner.UI.Interfaces;

namespace TourPlanner.UI.Services
{
    public class TourCoordinatorService : ITourCoordinatorService
    {
        private readonly ITourService _tourService;
        private readonly IDialogService _dialogService;
        private readonly ISelectedTourService _selectedTourService;

        public event Action? ToursChanged; 
        private readonly IMapService _mapService;


        public TourCoordinatorService(
            ITourService tourService,
            IDialogService dialogService,
            ISelectedTourService selectedTourService,
            IMapService mapService
            )
        {
            _mapService = mapService;
            _tourService = tourService;
            _dialogService = dialogService;
            _selectedTourService = selectedTourService;
        }

        public async void AddTour()
        {
            var tour = _dialogService.DisplayTourPopUp("Add new tour");
            if (tour == null) return;
            _tourService.AddTour(tour);
            ToursChanged?.Invoke();
            await HandleMap(tour);
        }

        private async Task HandleMap(TourDTO tour)
        {
            await _mapService.UpdateMapAsync(tour.StartLocation, tour.EndLocation);
            await _mapService.SaveMapImageAsync(tour.RouteImagePath);
        }

        public void RequestTourRefresh()
        {
            ToursChanged?.Invoke();
        }
    }
}
