using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;
using TourPlanner.UI.Interfaces;
using TourPlanner.UI.Interfaces.Coordinators;
using static TourPlanner.BL.Services.ImportService;

namespace TourPlanner.UI.Services.Coordinators
{
    public class TourImportCoordinator : ITourImportCoordinator
    {
        private ITourService _tourService;

        private ITourLogService _tourLogService;

        private IImportService _importService;

        private IReportService _reportService;

        private ISelectedTourService _selectedTourService;

        private IDialogService _dialogService;

        private IMapService _mapService;


        public TourImportCoordinator(
            ITourService tourService, 
            ITourLogService tourLogService, 
            IImportService importService, 
            IReportService reportService, 
            ISelectedTourService selectedTourService, 
            IDialogService dialogService,
            IMapService mapService
            )
        {
            _tourService = tourService;
            _tourLogService = tourLogService;
            _importService = importService;
            _reportService = reportService;
            _selectedTourService = selectedTourService;
            _dialogService = dialogService;
            _mapService = mapService;
        }

        public void ImportFromCsv(string path)
        {
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
        }

        public async Task ImportFromJson(string path)
        {
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
                        await _mapService.UpdateMapAsync(t.StartLocation, t.EndLocation);
                        await _mapService.SaveMapImageAsync(t.RouteImagePath);
                    }
                    break;

                case ContentType.TourLog:
                    var currentTour = _selectedTourService.SelectedTour;

                    if (currentTour == null)
                    {
                        _dialogService.ShowMessage("No Tour selected");
                        return;
                    }
                    var logs = _importService.ImportTourLogsFromJson(path);
                    foreach (var l in logs)
                    {
                        l.TourId = currentTour.Id;
                        _tourLogService.AddTourLog(l);
                    }
                    break;

                default:
                    _dialogService.ShowMessage("Unbekanntes JSON-Format.");
                    break;
            }
        }
    }
}
