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
        private readonly ISearchService _searchService;

        public event Action? ToursChanged; 
        private readonly IMapService _mapService;


        public TourCoordinatorService(
            ITourService tourService,
            IDialogService dialogService,
            ISelectedTourService selectedTourService,
            IMapService mapService,
            ISearchService searchService
            )
        {
            _mapService = mapService;
            _tourService = tourService;
            _dialogService = dialogService;
            _selectedTourService = selectedTourService;
            _searchService = searchService;
        }

        public async void AddTour()
        {
            _searchService.CurrentSearchTerm = string.Empty;
            var tour = _dialogService.DisplayTourPopUp("Add new tour");
            if (tour == null) return;
            _tourService.AddTour(tour);
            ToursChanged?.Invoke();
            await HandleMap(tour);
        }

        public async Task HandleMap(TourDTO tour)
        {
            await _mapService.UpdateMapAsync(tour.StartLocation, tour.EndLocation);
            if (tour.RouteImagePath != null) await _mapService.SaveMapImageAsync(tour.RouteImagePath);
        }

        public void RequestTourRefresh()
        {
            ToursChanged?.Invoke();
        }
    }
}
