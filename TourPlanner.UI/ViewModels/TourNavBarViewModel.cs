using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.Services;
using TourPlanner.UI.Interfaces;
using TourPlanner.UI.Interfaces.Coordinators;
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

        private IExportService _exportService;

        private ITourCoordinatorService _tourCoordinatorService;

        private ITourExportCoordinator _tourExportCoordinator;

        public TourNavBarViewModel
            (
            ITourService tourService,
            ITourLogService tourLogService,
            ISelectedTourService selectedTourService,
            ITourStatisticsService ourStatisticsService,
            IDialogService dialogService,
            IImportService importService,
            IExportService exportService,
            IReportService reportService,
            ITourCoordinatorService tourCoordinatorService,
            ITourExportCoordinator tourExportCoordinator
            ) 
        { 
            _tourService = tourService;
            _tourLogService = tourLogService;
            _selectedTourService = selectedTourService;
            _tourStatisticsService = ourStatisticsService;
            _dialogService = dialogService;
            _importService = importService;
            _exportService = exportService;
            _reportService = reportService;
            _tourCoordinatorService = tourCoordinatorService;
            _tourExportCoordinator = tourExportCoordinator;
        }


        public RelayCommand AddTourCommand => new RelayCommand(execute => AddTour());
        
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

        private void ImportFromCsv()
        {
            var path = _dialogService.ShowOpenFileDialog("CSV Files (*.csv)|*.csv|All Files (*.*)|*.*");

            if (path == null) return;

            var type = _importService.DetectCsvType(path);

            switch (type)
            {
                case ContentType.Tour:
                    var tours = _importService.ImportToursFromCSV(path);
                    foreach (var t in tours)
                    {
                        _tourService.AddTour(t);
                    }
                    break;

                case ContentType.TourLog:
                    var tour = _selectedTourService.SelectedTour;

                    if (tour == null)
                    {
                        _dialogService.ShowMessage("No Tour selected");
                        return;
                    }

                    var logs = _importService.ImportTourLogsFromCSV(path);
                    foreach (var l in logs)
                    {
                        l.TourId = tour.Id;
                        _tourLogService.AddTourLog(l);
                    }
                    break;
                case ContentType.Error:
                    _dialogService.ShowMessage("Die Datei ist bereits geöffnet (z. B. in Excel) und kann nicht gelesen werden. Bitte schließe sie zuerst.");
                    break;
                default:
                    _dialogService.ShowMessage("Unbekanntes CSV-Format.");
                    break;
            }
            _tourCoordinatorService.RequestTourRefresh();
        }

        private void ImportFromJson()
        {
            var path = _dialogService.ShowOpenFileDialog("JSON Files (*.json)|*.json|All Files (*.*)|*.*");

            if (path == null) return;

            var type = _importService.DetectJsonType(path);

            switch (type)
            {
                case ContentType.Tour:
                    var tours = _importService.ImportToursFromJson(path);
                    foreach (var t in tours)
                    {
                        var tour = _tourService.AddTour(t);
                        if (t.TourLogs != null)
                        {
                            
                            foreach (var log in t.TourLogs)
                            {
                                log.TourId = tour.Id;
                                _tourLogService.AddTourLog(log);
                            }
                        }
                    }
                    break;

                case ContentType.TourLog:
                    var logs = _importService.ImportTourLogsFromJson(path);
                    foreach (var l in logs)
                        _tourLogService.AddTourLog(l);
                    break;

                default:
                    _dialogService.ShowMessage("Unbekanntes JSON-Format.");
                    break;
            }
            _tourCoordinatorService.RequestTourRefresh();
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
            var tour = _selectedTourService.SelectedTour;

            if (tour == null)
            {
                _dialogService.ShowMessage("No Tour selected");
                return;
            }

            tour.TourLogs = _tourLogService.GetTourLogsForTour(tour.Id);

            var folderPath = _dialogService.ShowFolderDialog();

            if (folderPath == null) return;

            _exportService.ExportToursToCsv(new List<TourDTO> { tour }, folderPath);
        }

        private void ExportSelectedTourAsJson()
        {
            var tour = _selectedTourService.SelectedTour;

            if (tour == null)
            {
                _dialogService.ShowMessage("No Tour selected");
                return;
            }

            var path = _dialogService.ShowSaveFileDialog("output.json", "JSON Files (*.json)|*.json|All Files (*.*)|*.*");

            if (path == null) return;

            var logs = _tourLogService.GetTourLogsForTour(tour.Id);

            tour.TourLogs = logs;

            _exportService.ExportToursToJson(new List<TourDTO> { tour }, path);
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
            var folderPath = _dialogService.ShowFolderDialog();

            if (folderPath == null) return;

            _tourExportCoordinator.ExportAllToursAsCsv(folderPath); 
        }

        private void ExportAllToursAsJson()
        {
            var path = _dialogService.ShowSaveFileDialog("output.json", "JSON Files (*.json)|*.json|All Files (*.*)|*.*");

            if (path == null) return;

            var tours = _tourService.GetTours();

            foreach (var tour in tours)
            {
                tour.TourLogs = _tourLogService.GetTourLogsForTour(tour.Id);
            }

            _exportService.ExportToursToJson(tours, path);
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
