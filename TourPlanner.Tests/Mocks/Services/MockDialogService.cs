using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.UI.Interfaces;

namespace TourPlanner.UI.Services
{
    internal class MockDialogService : IDialogService
    {
        public TourDTO? sampleTour { get; set; }

        public TourLogDTO? sampleTourLog { get; set; }


        public TourLogDTO? DisplayTourLogPopUp(string title, TourLogDTO tourLog = null)
        { 
            return tourLog ?? sampleTourLog;
        }

        public TourDTO? DisplayTourPopUp(string title, TourDTO tour = null)
        {
            return tour ?? sampleTour;
        }
    }
}
