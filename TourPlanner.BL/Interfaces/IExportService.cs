using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;

namespace TourPlanner.BL.Interfaces
{
    public interface IExportService
    {
        void ExportToursToCsv(List<TourDTO> tours, string folderPath);
        void ExportToursToJson(List<TourDTO> tours, string filePath);
    }
}
