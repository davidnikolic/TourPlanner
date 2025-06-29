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
using static TourPlanner.BL.Services.ImportService;

namespace TourPlanner.UI.ViewModels
{
    public class TourNavBarViewModel : ViewModelBase
    {
        private ITourService _tourService;

        private ITourLogService _tourLogService;

        private ISelectedTourService _selectedTourService;

        private ITourStatisticsService _tourStatisticsService;

        private IReportService _reportService;

        private IDialogService _dialogService;

        private IImportService _importService;

        private ITourCoordinatorService _tourCoordinatorService;

        public TourNavBarViewModel
            (
            ITourService tourService,
            ITourLogService tourLogService,
            ISelectedTourService selectedTourService,
            ITourStatisticsService ourStatisticsService,
            IDialogService dialogService,
            IImportService importService,
            IReportService reportService,
            ITourCoordinatorService tourCoordinatorService
            ) 
        { 
            _tourService = tourService;
            _tourLogService = tourLogService;
            _selectedTourService = selectedTourService;
            _tourStatisticsService = ourStatisticsService;
            _dialogService = dialogService;
            _importService = importService;
            _reportService = reportService;
            _tourCoordinatorService = tourCoordinatorService;
        }


        public RelayCommand AddTourCommand => new RelayCommand(execute => AddTour());
        public RelayCommand AddTourLogCommand => new RelayCommand(execute => AddTourLog());

        public RelayCommand ImportFromCsvCommand => new RelayCommand(execute => ImportFromCsv());
        public RelayCommand ImportFromJSONCommand => new RelayCommand(execute => ImportFromJson());
        
        public RelayCommand SelectedTourPdfCommand => new RelayCommand(execute => ExportSelectedTourAsPdf());
        public RelayCommand SelectedTourCsvCommand => new RelayCommand(execute => ExportSelectedTourAsCsv());
        public RelayCommand SelectedTourJsonCommand => new RelayCommand(execute => ExportSelectedTourAsJson());
        public RelayCommand AllToursPdfCommand => new RelayCommand(execute => ExportAllToursAsPdf());
        public RelayCommand AllToursCsvCommand => new RelayCommand(execute => ExportAllToursAsCsv());
        public RelayCommand AllToursJsonCommand => new RelayCommand(execute => ExportAllToursAsJson());
        public RelayCommand SummarizeReportCommand => new RelayCommand(execute => ExportSummarizeReportAsPdf());

        public RelayCommand ExitCommand => new RelayCommand(execute => Environment.Exit(0));

        private void AddTour()
        {
            _tourCoordinatorService.AddTour();
        }

        private void AddTourLog()
        {
            MessageBox.Show("Diese Funktion wird noch implementiert.", "WIP", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ImportFromCsv()
        {
            var path = _dialogService.ShowOpenFileDialog("CSV Files (*.csv)|*.csv|All Files (*.*)|*.*");

            var type = _importService.DetectCsvType(path);

            switch (type)
            {
                case CsvContentType.Tour:
                    var tours = _importService.ImportToursFromCSV(path);
                    foreach (var t in tours)
                    {
                        _tourService.AddTour(t);
                    }
                    break;

                case CsvContentType.TourLog:
                    var logs = _importService.ImportTourLogsFromCSV(path);
                    foreach (var l in logs)
                        _tourLogService.AddTourLog(l);
                    break;

                default:
                    _dialogService.ShowMessage("Unbekanntes CSV-Format.");
                    break;
            }

           

            _tourCoordinatorService.RequestTourRefresh();
        }

        private void ImportFromJson()
        {
            MessageBox.Show("Diese Funktion wird noch implementiert.", "WIP", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportSelectedTourAsPdf()
        {
            var tour = _selectedTourService.SelectedTour;

            if (tour == null)
            {
                _dialogService.ShowMessage("No Tour selected");
                return;
            }

            tour.TourLogs = _tourLogService.GetTourLogsForTour(tour.Id);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TourReport.pdf");

            _reportService.GenerateTourReport(tour, path);

            MessageBox.Show("PDF successfully created on the desktop.");
        }

        private void ExportSelectedTourAsCsv()
        {
            MessageBox.Show("Diese Funktion wird noch implementiert.", "WIP", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportSelectedTourAsJson()
        {
            MessageBox.Show("Diese Funktion wird noch implementiert.", "WIP", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportAllToursAsPdf()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TourReport.pdf");
            var tours = _tourService.GetTours();

            foreach (var tour in tours)
            {
                tour.TourLogs = _tourLogService.GetTourLogsForTour(tour.Id);
            }

            _reportService.GenerateAllToursReport(tours, path);

            MessageBox.Show("PDF successfully created on the desktop.");
        }

        private void ExportAllToursAsCsv()
        {
            MessageBox.Show("Diese Funktion wird noch implementiert.", "WIP", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportAllToursAsJson()
        {
            MessageBox.Show("Diese Funktion wird noch implementiert.", "WIP", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ExportSummarizeReportAsPdf()
        {

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SummarizeReport.pdf");
            var tours = _tourService.GetTours();
            var logs = _tourLogService.GetAllTourLogs();

            var stats = _tourStatisticsService.CalculateAllTourStatistics(tours, logs);

            _reportService.GenerateSummarizeReport(stats, path);

            MessageBox.Show("PDF successfully created on the desktop.");
        }
    }
}
