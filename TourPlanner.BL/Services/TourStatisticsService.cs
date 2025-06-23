using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;

namespace TourPlanner.BL.Services
{
    public class TourStatisticsService : ITourStatisticsService
    {
        public List<TourStatisticsDTO> CalculateAllTourStatistics(List<TourDTO> tours, List<TourLogDTO> logs) 
        {
            List<TourStatisticsDTO> statistics = new();

            foreach (TourDTO tour in tours)
            {
                List<TourLogDTO> tourLogs = logs.Where(l => l.TourId.Equals(tour.Id)).ToList();

                statistics.Add(CalculateTourStatistic(tour, tourLogs));
            }

            return statistics;
        }

        public TourStatisticsDTO CalculateTourStatistic(TourDTO tour, List<TourLogDTO> logs)
        {
            if (logs == null || logs.Count == 0)
            {
                return new TourStatisticsDTO
                {
                    TourId = tour.Id,
                    TourName = tour.Name,
                    Popularity = 0,
                    AvgDifficulty = 0f,         
                    AvgRating = 0f,             
                    IsChildFriendly = false     
                };
            }

            return new TourStatisticsDTO
            {
                TourId = tour.Id,
                TourName = tour.Name,
                Popularity = logs.Count(),
                AvgDifficulty = (float)logs.Average(l => (int)l.Difficulty),
                AvgRating = (float)logs.Average(l => (int)l.Rating),
                IsChildFriendly = IsChildFriendly(logs)
            };
        }

        public bool IsChildFriendly(List<TourLogDTO> logs)
        {
            if (logs == null || logs.Count == 0)
                return false;

            var avgDifficulty = logs.Average(l => (int)l.Difficulty); 
            var avgDuration = logs.Average(l => l.DurationHours);
            var avgDistance = logs.Average(l => l.DistanceKm);

            return avgDifficulty <= 0.7     
                && avgDuration <= 2.0      
                && avgDistance <= 5.0;     
        }
    }

}
