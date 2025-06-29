using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;

namespace TourPlanner.UI.Interfaces
{
    public interface IDialogService
    {
        public TourDTO? DisplayTourPopUp(string title, TourDTO tour = null);

        public TourLogDTO? DisplayTourLogPopUp(string title, TourLogDTO tourLog = null);

        string? ShowOpenFileDialog(string filter);

        string? ShowSaveFileDialog(string defaultFileName, string filter);

        void ShowMessage(string message);

    }
}
