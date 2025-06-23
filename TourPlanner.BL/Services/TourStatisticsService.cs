using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;

namespace TourPlanner.BL.Services
{
    public class TourStatisticsService
    {
        public TourStatisticsDTO CalculateTourStatistic(TourDTO tour, IEnumerable<TourLogDTO> logs)
        {
            return new TourStatisticsDTO
            {
                TourId = tour.Id,
                TourName = tour.Name,
                Popularity = logs.Count(),
                AvgDifficulty = (float)logs.Average(l => (int)l.Difficulty),
                AvgRating = (float)logs.Average(l => (int)l.Rating),
                IsChildFriendly = logs.Average(l => (int)l.Difficulty) <= 1.5 && logs.Average(l => (int)l.Rating) >= 1.5
            };
        }
    }
}
