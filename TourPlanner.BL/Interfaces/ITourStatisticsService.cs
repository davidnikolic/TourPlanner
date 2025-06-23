using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;

namespace TourPlanner.BL.Interfaces
{
    public interface ITourStatisticsService
    {
        public List<TourStatisticsDTO> CalculateAllTourStatistics(List<TourDTO> tours, List<TourLogDTO> logs);

        public TourStatisticsDTO CalculateTourStatistic(TourDTO tour, List<TourLogDTO> logs);

        public bool IsChildFriendly(List<TourLogDTO> logs);
    }
}
