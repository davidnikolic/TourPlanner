using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Interfaces;
using TourPlanner.UI.Interfaces.Coordinators;

namespace TourPlanner.UI.Services.Coordinators
{
    public class TourExportCoordinator : ITourExportCoordinator
    {
        private ITourService _tourService;

        private ITourLogService _tourLogService;

        private IExportService _exportService;

        public TourExportCoordinator
            (
            ITourService tourService,
            ITourLogService tourLogService,
            IExportService exportService
            )
        {
            _tourService = tourService;
            _tourLogService = tourLogService;
            _exportService = exportService;
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

        public void ExportAllToursAsJson()
        {
            throw new NotImplementedException();
        }

        public void ExportAllToursAsPdf()
        {
            throw new NotImplementedException();
        }

        public void ExportSelectedTourAsCsv()
        {
            throw new NotImplementedException();
        }

        public void ExportSelectedTourAsJson()
        {
            throw new NotImplementedException();
        }

        public void ExportSelectedTourAsPdf()
        {
            throw new NotImplementedException();
        }

        public void ExportSummarizeReport()
        {
            throw new NotImplementedException();
        }
    }
}
