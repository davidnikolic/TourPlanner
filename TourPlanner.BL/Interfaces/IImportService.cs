using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using static TourPlanner.BL.Services.ImportService;

namespace TourPlanner.BL.Interfaces
{
    public interface IImportService
    {
        CsvContentType DetectCsvType(string filePath);
        List<TourDTO> ImportToursFromCSV(string filepath);
        List<TourLogDTO> ImportTourLogsFromCSV(string filepath);
    }
}
