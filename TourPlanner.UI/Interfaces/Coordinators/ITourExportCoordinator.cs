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
        void ExportAllToursAsPdf(string path);
        void ExportSelectedTourAsCsv();
        void ExportAllToursAsCsv(string folderPath);
        void ExportSelectedTourAsJson();
        void ExportAllToursAsJson(string path);
        void ExportSummarizeReport();
    }
}
