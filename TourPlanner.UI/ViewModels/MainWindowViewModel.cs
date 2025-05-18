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
    public class MainWindowViewModel : ViewModelBase
    {
        private ITourService _tourService;
        private ITourLogService _tourLogService;

        private ISelectedTourService _selectedTourService;

        public TourListViewModel TourListViewModel { get; }
        public TourDetailViewModel TourDetailViewModel { get; } 
        public TourLogsViewModel TourLogsViewModel { get; }

        public MainWindowViewModel(
            ITourService tourService, 
            ITourLogService tourLogService, 
            ISelectedTourService selectedTourService, 
            TourListViewModel tourListViewModel, 
            TourLogsViewModel tourLogsViewModel,
            TourDetailViewModel tourDetailViewModel
            )
        {
            _tourService = tourService;
            _tourLogService = tourLogService;
            _selectedTourService = selectedTourService;

            TourListViewModel = tourListViewModel;
            TourLogsViewModel = tourLogsViewModel;
            TourDetailViewModel = tourDetailViewModel;
        }
    }
}
