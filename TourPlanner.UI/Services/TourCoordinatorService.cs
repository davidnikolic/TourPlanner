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

        public TourCoordinatorService(
            ITourService tourService,
            IDialogService dialogService,
            ISelectedTourService selectedTourService
            )
        {
            _tourService = tourService;
            _dialogService = dialogService;
            _selectedTourService = selectedTourService;
        }

        public void AddTour()
        {
            var tour = _dialogService.DisplayTourPopUp("Add new tour");

            if (tour == null) return;

            _tourService.AddTour(tour);
            
            ToursChanged?.Invoke();
        }

        public void RequestTourRefresh()
        {
            ToursChanged?.Invoke();
        }
    }
}
