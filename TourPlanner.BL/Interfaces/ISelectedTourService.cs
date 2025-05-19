using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;

namespace TourPlanner.BL.Interfaces
{
    /// <summary>
    /// This service handles the selected tour. This services provides the selected tour for every viewmodel
    /// </summary>
    public interface ISelectedTourService
    {
        /// <summary>
        /// The SelectedTour as a TourDTO.
        /// </summary>
        TourDTO SelectedTour { get; set; }

        /// <summary>
        /// An event, that informs the subscribers that the selectedTour has been changed.
        /// </summary>
        event Action<TourDTO> SelectedTourChanged;
    }
}
