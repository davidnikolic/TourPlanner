using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.DTOs;
using TourPlanner.DAL.Entities;
using TourPlanner.DAL.Repositories.Interfaces;

namespace TourPlanner.BL.Services
{
    public class TourLogService : ITourLogService
    {
        private readonly ITourLogRepository _tourLogRepository;

        public TourLogService(ITourLogRepository tourLogRepository)
        {
            _tourLogRepository = tourLogRepository;
        }

        public void AddTourLog(TourLogDTO log)
        {
            var entity = ToEntity(log);
            _tourLogRepository.AddTourLog(entity);
        }

        public List<TourLogDTO> GetTourLogsForTour(int tourId)
        {
            var entities = _tourLogRepository.GetAllTourLogsForTour(tourId);
            return entities.Select(e => ToModel(e)).ToList();
        }

        public List<TourLogDTO> GetAllTourLogs()
        {
            var entities = _tourLogRepository.GetAllTourLogs();
            return entities.Select(e => ToModel(e)).ToList();
        }

        public void UpdateTourLog(TourLogDTO log)
        {
            var entity = ToEntity(log);
            _tourLogRepository.UpdateTourLog(entity);
        }

        public void DeleteTourLog(TourLogDTO log)
        {
            _tourLogRepository.DeleteTourLog(log.Id);
        }

        public static TourLogDTO ToModel(TourLogEntity entity)
        {
            if (entity == null) return null;

            return new TourLogDTO
            {
                Id = entity.Id,
                TourId = entity.TourId,
                LogDate = entity.LogDate,
                Comment = entity.Comment,
                DistanceKm = entity.DistanceKm,
                DurationHours = entity.DurationHours,
                Difficulty = (EnumsDTO.DifficultyLevel)entity.Difficulty,
                Rating = (EnumsDTO.SatisfactionRating)entity.Rating
            };
        }
        public static TourLogEntity ToEntity(TourLogDTO model)
        {
            if (model == null) return null;

            var safeLogDate = DateTime.SpecifyKind(model.LogDate, DateTimeKind.Unspecified);

            return new TourLogEntity
            {
                Id = model.Id,
                TourId = model.TourId,
                LogDate = safeLogDate.Date,
                Comment = model.Comment,
                DistanceKm = model.DistanceKm,
                DurationHours = model.DurationHours,
                Difficulty = (Enums.DifficultyLevel)model.Difficulty,
                Rating = (Enums.SatisfactionRating)model.Rating
            };
        }

        public static int CalculateChildFriendliness(IEnumerable<TourLogEntity> logs)
        {
            // No logs
            if (!logs.Any()) return 0;

            // 1. Calculate average values for difficulty, duration, and distance
            var avgDifficulty = logs.Average(l => (int)l.Difficulty);
            var avgDuration = logs.Average(l => l.DurationHours);
            var avgDistance = logs.Average(l => l.DistanceKm);

            // 2. Apply factors to each metric:
            // - Difficulty: 30% weight
            // - Duration: 40% weight (longer tours less child-friendly)
            // - Distance: 30% weight (longer distance less child-friendly)

            // Total sum scaled to a range from 0 to 5
            // Subtract sum 5 and with combined value 
            return 5 - (int)(avgDifficulty * 0.3 + avgDuration * 0.4 + avgDistance * 0.3);
        }

        public List<TourLogDTO> SearchTourLogs(string searchTerm)
        {
            // Load all tour logs from the database
            var allTourLogEntities = _tourLogRepository.GetAllTourLogs();

            // If the search term is empty or null, return all tour logs as DTOs
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return allTourLogEntities.Select(ToModel).ToList();
            }

            // Lowercase and space 
            var lowerSearchTerm = searchTerm.ToLower().Trim();

            // For specific numeric searches with exact matches 

            // Popularity
            if (lowerSearchTerm.StartsWith("popularity:"))
            {
                // Extract the value after the word(should be a number)
                string valuePart = lowerSearchTerm.Substring("popularity:".Length).Trim();

                // Try to parse the extracted part into an integer and check if its an int
                if (int.TryParse(valuePart, out int popValue))
                {
                    // Filter logs where the tour has the exact number
                    // Nullpointer check
                    var filtered = allTourLogEntities.Where(l =>
                        l.Tour != null && l.Tour.TourLogs != null && l.Tour.TourLogs.Count() == popValue
                    ).ToList();
                    return filtered.Select(ToModel).ToList();
                }
            }
            // ChildFriendliness
            else if (lowerSearchTerm.StartsWith("childfriendliness:"))
            {
                string valuePart = lowerSearchTerm.Substring("childfriendliness:".Length).Trim();
                if (int.TryParse(valuePart, out int childValue))
                {
                    var filtered = allTourLogEntities.Where(l =>
                        l.Tour != null && l.Tour.TourLogs != null && CalculateChildFriendliness(l.Tour.TourLogs) == childValue
                    ).ToList();
                    return filtered.Select(ToModel).ToList();
                }
            }
            // DistanceKm
            else if (lowerSearchTerm.StartsWith("distance:"))
            {
                string valuePart = lowerSearchTerm.Substring("distance:".Length).Trim();
                if (float.TryParse(valuePart, out float distValue))
                {
                    var filtered = allTourLogEntities.Where(l =>
                        l.DistanceKm == distValue 
                    ).ToList();
                    return filtered.Select(ToModel).ToList();
                }
            }
            // DurationHours
            else if (lowerSearchTerm.StartsWith("duration:"))
            {
                string valuePart = lowerSearchTerm.Substring("duration:".Length).Trim();
                if (float.TryParse(valuePart, out float durValue)) 
                {
                    var filtered = allTourLogEntities.Where(l =>
                        l.DurationHours == durValue
                    ).ToList();
                    return filtered.Select(ToModel).ToList();
                }
            }

            // General full-text contains search
            // Performs search across multiple fields

            // Filter all logs by search term
            var filteredEntities = allTourLogEntities.Where(l =>
            {
                // Search in each relevant field for the search term
                return
                    (l.Comment != null && l.Comment.ToLower().Contains(lowerSearchTerm)) ||
                    l.Difficulty.ToString().ToLower().Contains(lowerSearchTerm) ||
                    l.Rating.ToString().ToLower().Contains(lowerSearchTerm) ||
                    l.DistanceKm.ToString().ToLower().Contains(lowerSearchTerm) ||
                    l.DurationHours.ToString().ToLower().Contains(lowerSearchTerm);
            }).ToList();

            // Convert the filtered entities to DTOs and return
            return filteredEntities.Select(ToModel).ToList();
        }
    }
}
