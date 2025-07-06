using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;
using TourPlanner.DAL.Entities;
using TourPlanner.DAL.Repositories.Interfaces;
using TourPlanner.Logging;
using TourPlanner.Logging.Interfaces;

namespace TourPlanner.BL.Services
{
    public class TourService : ITourService
    {
        public ITourRepository _tourRepository;

        private readonly ILoggerWrapper? _logger;

        public TourService(ITourRepository TourRepo,  ILoggerFactory? factory)
        {
            _tourRepository = TourRepo;

            if(factory != null) _logger = factory.CreateLogger<TourService>();
        }

        public TourDTO AddTour(TourDTO tour)
        {
            TourEntity entity = ToEntity(tour);
            var tourEntity = _tourRepository.AddTour(entity);
            if(_logger != null) _logger.Info($"Tour with ID {tour.Id} added");
            return ToModel(tourEntity);
        }

        public List<TourDTO> GetTours()
        {
            var entities = (_tourRepository.GetTours()).ToList();

            List<TourDTO> tours = entities
                .Select(entity => ToModel(entity))
                .ToList();

            return tours;
        }

        public void UpdateTourMapImagePath(int tourId, string imagePath)
        {
            var entity = _tourRepository.GetTourById(tourId);
            if (entity == null) return;

            entity.RouteImagePath = imagePath;
            _tourRepository.UpdateTour(entity);
        }
        public int GetLastTourId()
        {
            return _tourRepository.GetLastTourId();
        }

        public void UpdateTour(TourDTO tour)
        {
            TourEntity entity = ToEntity(tour);
            _tourRepository.UpdateTour(entity);
        }

        public void DeleteTour(TourDTO tour)
        {
            TourEntity entity = ToEntity(tour);
            _tourRepository.DeleteTour(entity.Id);

            if (_logger != null) _logger.Info($"Tour with ID {tour.Id} deleted");
        }

        public TourDTO ToModel(TourEntity entity)
        {
            if (entity == null)
                return null;

            var model = new TourDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                StartLocation = entity.StartLocation,
                EndLocation = entity.EndLocation,
                Description = entity.Description,
                TransportType = (EnumsDTO.TransportType)entity.TransportType,
                DistanceKm = entity.DistanceKm,
                EstimatedTimeHours = entity.EstimatedTimeHours,
                RouteImagePath = entity.RouteImagePath
            };

            if (_logger != null) _logger.Debug($"Tour-Entity with ID {model.Id} successully converted to Model");

            return model;
        }

        public static TourEntity ToEntity(TourDTO model)
        {
            if (model == null)
                return null;

            return new TourEntity
            {
                Id = model.Id,
                Name = model.Name,
                StartLocation = model.StartLocation,
                EndLocation = model.EndLocation,
                Description = model.Description,
                TransportType = (Enums.TransportType)model.TransportType,
                DistanceKm = model.DistanceKm,
                EstimatedTimeHours = model.EstimatedTimeHours,
                RouteImagePath = model.RouteImagePath
            };
        }
    }
}
