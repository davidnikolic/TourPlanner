using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.Services;
using TourPlanner.UI.Interfaces;

namespace TourPlanner.UI.ViewModels
{
    public class TourNavBarViewModel : ViewModelBase
    {
        private ITourService _tourService;

        private ISelectedTourService _selectedTourService;

        private IReportService _reportService;

        public TourNavBarViewModel
            (
            ITourService tourService,
            ISelectedTourService selectedTourService,
            IReportService reportService
            ) 
        { 
            _tourService = tourService;
            _selectedTourService = selectedTourService;
            _reportService = reportService;
        }


        public RelayCommand ExportSelectedTourCommand => new RelayCommand(execute => ExportSelectedTour());
        public RelayCommand ExportAllToursCommand => new RelayCommand(execute => ExportSummarizeReport());

        public RelayCommand ImportFromCsvCommand => new RelayCommand(execute => ImportFromCsv());
        public RelayCommand ImportFromJSONCommand => new RelayCommand(execute => ImportFromJSON());

        public RelayCommand ExitCommand => new RelayCommand(execute => Environment.Exit(0));

        private void ExportSelectedTour()
        {
            var tour = _selectedTourService.SelectedTour;

            if (tour == null)
            {
                MessageBox.Show("Keine Tour ausgewählt.");
                return;
            }

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TourReport.pdf");

            _reportService.GenerateTourReport(tour, path);
            MessageBox.Show("PDF erfolgreich erstellt auf dem Desktop.");
        }

        private void ExportSummarizeReport()
        {

        }

        public void ImportFromCsv()
        {

        }

        public void ImportFromJSON()
        {

        }
    }
}
