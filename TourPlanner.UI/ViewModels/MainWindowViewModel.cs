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
    /// <summary>
    /// The Main-ViewModel. This object orchastrates the SubViewModels.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The Service for the tours.
        /// </summary>
        private ITourService _tourService;

        /// <summary>
        /// The service for the tour-logs.
        /// </summary>
        private ITourLogService _tourLogService;

        /// <summary>
        /// The service for the selected tour.
        /// </summary>
        private ISelectedTourService _selectedTourService;

        /// <summary>
        /// The ViewModel for the TourList-View
        /// </summary>
        public TourListViewModel TourListViewModel { get; }

        /// <summary>
        /// The ViewModel for the TourDetail TabControl.
        /// </summary>
        public TourDetailViewModel TourDetailViewModel { get; } 

        /// <summary>
        /// The ViewModel for the Tour-Log-List.
        /// </summary>
        public TourLogsViewModel TourLogsViewModel { get; }

        /// <summary>
        /// The constructor for the Main ViewModel.
        /// </summary>
        /// <param name="tourService"></param>
        /// <param name="tourLogService"></param>
        /// <param name="selectedTourService"></param>
        /// <param name="tourListViewModel"></param>
        /// <param name="tourLogsViewModel"></param>
        /// <param name="tourDetailViewModel"></param>
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
