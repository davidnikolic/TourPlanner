using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;

namespace TourPlanner.BL.Services
{
    /// <summary>
    /// The Implementation of ISelectedTourService
    /// </summary>
    public class SelectedTourService : ISelectedTourService
    {
        /// <summary>
        /// The selected Tour as TourDTO.
        /// </summary>
        private TourDTO selectedTour;

        /// <summary>
        /// The Action, that informs the other services that the tour has been changed.
        /// </summary>
        public event Action<TourDTO> SelectedTourChanged;

        /// <summary>
        /// The constructor for the selected Tour.
        /// </summary>
        public TourDTO SelectedTour { 
            get => selectedTour;
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    // The Action gets invoked when the Tour changes.
                    SelectedTourChanged?.Invoke(value);
                }
            }
        }   
    }
}
