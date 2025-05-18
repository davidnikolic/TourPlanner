using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;

namespace TourPlanner.BL.Services
{
    public class SelectedTourService : ISelectedTourService
    {

        private TourDTO selectedTour;

        public event Action<TourDTO> SelectedTourChanged;

        public TourDTO SelectedTour { 
            get => selectedTour;
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    SelectedTourChanged.Invoke(value);
                }
            }
        }   
    }
}
