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
                LogDate = safeLogDate,
                Comment = model.Comment,
                DistanceKm = model.DistanceKm,
                DurationHours = model.DurationHours,
                Difficulty = (Enums.DifficultyLevel)model.Difficulty,
                Rating = (Enums.SatisfactionRating)model.Rating
            };
        }
    }
}
