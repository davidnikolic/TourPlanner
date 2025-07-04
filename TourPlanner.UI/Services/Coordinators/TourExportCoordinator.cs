using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;
using TourPlanner.UI.Interfaces.Coordinators;
using IDialogService = TourPlanner.UI.Interfaces.IDialogService;

namespace TourPlanner.UI.Services.Coordinators
{
    public class TourExportCoordinator : ITourExportCoordinator
    {
        private ITourService _tourService;

        private ITourLogService _tourLogService;

        private IExportService _exportService;

        private IReportService _reportService;

        private ISelectedTourService _selectedTourService;

        private IDialogService _dialogService;

        private ITourStatisticsService _tourStatisticsService;

        public TourExportCoordinator
            (
            ITourService tourService,
            ITourLogService tourLogService,
            IExportService exportService,
            IReportService reportService,
            ISelectedTourService selectedTourService,
            IDialogService dialogService,
            ITourStatisticsService tourStatisticsService
            )
        {
            _tourService = tourService;
            _tourLogService = tourLogService;
            _exportService = exportService;
            _reportService = reportService;
            _selectedTourService = selectedTourService;
            _dialogService = dialogService;
            _tourStatisticsService = tourStatisticsService;
        }

        public void ExportAllToursAsCsv(string folderPath)
        {
            var tours = _tourService.GetTours();

            foreach (var tour in tours)
            {
                tour.TourLogs = _tourLogService.GetTourLogsForTour(tour.Id);
            }

            _exportService.ExportToursToCsv(tours, folderPath);
        }

        public void ExportAllToursAsJson(string path)
        {
            var tours = _tourService.GetTours();

            foreach (var tour in tours)
            {
                tour.TourLogs = _tourLogService.GetTourLogsForTour(tour.Id);
            }

            _exportService.ExportToursToJson(tours, path);
        }

        public void ExportAllToursAsPdf(string path)
        {
            var tours = _tourService.GetTours();

            foreach (var tour in tours)
            {
                tour.TourLogs = _tourLogService.GetTourLogsForTour(tour.Id);
            }

            _reportService.GenerateAllToursReport(tours, path);
        }

        public void ExportSelectedTourAsCsv()
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

        public void ExportSelectedTourAsJson()
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

        public void ExportSelectedTourAsPdf()
        {
            var tour = _selectedTourService.SelectedTour;

            if (tour == null)
            {
                _dialogService.ShowMessage("No Tour selected");
                return;
            }

            tour.TourLogs = _tourLogService.GetTourLogsForTour(tour.Id);

            var path = _dialogService.ShowSaveFileDialog("output.pdf", "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*");

            _reportService.GenerateTourReport(tour, path);

            _dialogService.ShowMessage("PDF successfully created on the desktop.");
        }

        public void ExportSummarizeReport()
        {
            string path = _dialogService.ShowSaveFileDialog("output.pdf", "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*");
            var tours = _tourService.GetTours();
            var logs = _tourLogService.GetAllTourLogs();

            var stats = _tourStatisticsService.CalculateAllTourStatistics(tours, logs);

            _reportService.GenerateSummarizeReport(stats, path);

            _dialogService.ShowMessage("PDF successfully created on the desktop.");
        }
    }
}
