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

        public string? ShowFolderDialog()
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string message)
        {
            throw new NotImplementedException();
        }

        public string? ShowOpenFileDialog(string filter)
        {
            throw new NotImplementedException();
        }

        public string? ShowSaveFileDialog(string defaultFileName, string filter)
        {
            throw new NotImplementedException();
        }
    }
}
