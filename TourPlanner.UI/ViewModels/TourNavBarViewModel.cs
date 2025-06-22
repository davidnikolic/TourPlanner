using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.BL.Interfaces;
using TourPlanner.UI.Interfaces;

namespace TourPlanner.UI.ViewModels
{
    public class TourNavBarViewModel : ViewModelBase
    {
        private IDialogService _dialogService;

        private ITourService _tourService;

        private ISelectedTourService _selectedTourService;

        public TourNavBarViewModel
            (
            IDialogService dialogService,
            ITourService tourService,
            ISelectedTourService selectedTourService
            ) 
        { 
            _dialogService = dialogService;
            _tourService = tourService;
            _selectedTourService = selectedTourService;
        }


        public RelayCommand ExportSelectedTourCommand => new RelayCommand(execute => ExportSelectedTour());
        public RelayCommand ExportAllToursCommand => new RelayCommand(execute => ExportSummarizeReport());

        public RelayCommand ImportFromCsvCommand => new RelayCommand(execute => ImportFromCsv());
        public RelayCommand ImportFromJSONCommand => new RelayCommand(execute => ImportFromJSON());

        public RelayCommand ExitCommand => new RelayCommand(execute => Environment.Exit(0));

        public void ImportFromCsv()
        {

        }

        public void ImportFromJSON()
        {

        }


        private void ExportSelectedTour()
        {
            
        }

        private void ExportSummarizeReport()
        {

        }
    }
}
