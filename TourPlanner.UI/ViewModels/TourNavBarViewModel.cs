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

        private ITourLogService _tourLogService;

        private ISelectedTourService _selectedTourService;

        private ITourStatisticsService _tourStatisticsService;

        private IReportService _reportService;

        public TourNavBarViewModel
            (
            ITourService tourService,
            ITourLogService tourLogService,
            ISelectedTourService selectedTourService,
            ITourStatisticsService ourStatisticsService,
            IReportService reportService
            ) 
        { 
            _tourService = tourService;
            _tourLogService = tourLogService;
            _selectedTourService = selectedTourService;
            _tourStatisticsService = ourStatisticsService;
            _reportService = reportService;
        }


        public RelayCommand ExportSelectedTourCommand => new RelayCommand(execute => ExportSelectedTour());

        public RelayCommand ExportAllToursCommand => new RelayCommand(execute => ExportAllTours());
        public RelayCommand SummarizeReportCommand => new RelayCommand(execute => ExportSummarizeReport());

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

            tour.TourLogs = _tourLogService.GetTourLogsForTour(tour.Id);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TourReport.pdf");

            _reportService.GenerateTourReport(tour, path);

            MessageBox.Show("PDF erfolgreich erstellt auf dem Desktop.");
        }

        private void ExportAllTours()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TourReport.pdf");
            var tours = _tourService.GetTours();

            foreach (var tour in tours)
            {
                tour.TourLogs = _tourLogService.GetTourLogsForTour(tour.Id);
            }

            _reportService.GenerateAllToursReport(tours, path);

            MessageBox.Show("PDF erfolgreich erstellt auf dem Desktop.");
        }

        public void ExportSummarizeReport()
        {

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SummarizeReport.pdf");
            var tours = _tourService.GetTours();
            var logs = _tourLogService.GetAllTourLogs();

            var stats = _tourStatisticsService.CalculateAllTourStatistics(tours, logs);

            _reportService.GenerateSummarizeReport(stats, path);

            MessageBox.Show("PDF erfolgreich erstellt auf dem Desktop.");
        }

        public void ImportFromCsv()
        {

        }

        public void ImportFromJSON()
        {

        }
    }
}
