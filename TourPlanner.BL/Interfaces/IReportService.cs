using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;

namespace TourPlanner.BL.Interfaces
{
    public interface IReportService
    {
        void GenerateTourReport(TourDTO tour, string filePath);
        
        void GenerateAllToursReport(List<TourDTO> tours, string filePath);

        void GenerateSummarizeReport(List<TourStatisticsDTO> stats, string filePath);
    }
}
