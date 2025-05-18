using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.MockRepos;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;
using TourPlanner.DAL.Entities;
using TourPlanner.DAL.Repositories.Interfaces;
using TourPlanner.DAL.Repositories;
using System.Runtime.CompilerServices;

namespace TourPlanner.BL.Services
{
    public class TourService : ITourService
    {
        public ITourRepository _tourRepository;

        public TourService(ITourRepository TourRepo)
        {
            _tourRepository = TourRepo;
        }

        

        public void AddTour(TourDTO tour)
        {
           TourEntity entity = ToEntity(tour);
           _tourRepository.AddTour(entity);
        }

        public List<TourDTO> GetTours()
        {
            var entities = (_tourRepository.GetTours()).ToList();

            List<TourDTO> tours = entities
                .Select(entity => ToModel(entity))
                .ToList();

            return tours;
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
        }

        public static TourDTO ToModel(TourEntity entity)
        {
            if (entity == null)
                return null;

            return new TourDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                StartLocation = entity.StartLocation,
                EndLocation = entity.EndLocation,
                Description = entity.Description,
                TransportType = entity.TransportType,
                DistanceKm = entity.DistanceKm,
                EstimatedTimeHours = entity.EstimatedTimeHours,
                RouteImagePath = entity.RouteImagePath
            };
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
                TransportType = model.TransportType,
                DistanceKm = model.DistanceKm,
                EstimatedTimeHours = model.EstimatedTimeHours,
                RouteImagePath = model.RouteImagePath
            };
        }

        
    }
}
