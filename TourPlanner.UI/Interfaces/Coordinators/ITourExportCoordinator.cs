using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.UI.Interfaces.Coordinators
{
    public interface ITourExportCoordinator
    {
        void ExportSelectedTourAsPdf();
        void ExportAllToursAsPdf();
        void ExportSelectedTourAsCsv();
        void ExportAllToursAsCsv(string folderPath);
        void ExportSelectedTourAsJson();
        void ExportAllToursAsJson();
        void ExportSummarizeReport();
    }
}
