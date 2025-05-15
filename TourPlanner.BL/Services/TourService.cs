using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.MockRepos;
using TourPlanner.BL.Models;
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

        

        public void AddTour(Tour tour)
        {
           TourEntity entity = ToEntity(tour);
           _tourRepository.AddTour(entity);
        }

        public List<Tour> GetTours()
        {
            var entities = (_tourRepository.GetTours()).ToList();

            List<Tour> tours = entities
                .Select(entity => ToModel(entity))
                .ToList();

            return tours;
        }

        public void UpdateTour(Tour tour)
        {
            TourEntity entity = ToEntity(tour);
            _tourRepository.UpdateTour(entity);
        }

        public void DeleteTour(Tour tour)
        {
            TourEntity entity = ToEntity(tour);
            _tourRepository.DeleteTour(entity.Id);
        }

        public static Tour ToModel(TourEntity entity)
        {
            if (entity == null)
                return null;

            return new Tour
            {
                Id = entity.Id,
                Name = entity.Name,
                StartLocation = entity.StartLocation,
                EndLocation = entity.EndLocation,
                Description = entity.Description,
                DistanceKm = entity.DistanceKm,
                EstimatedTimeHours = entity.EstimatedTimeHours
            };
        }

        public static TourEntity ToEntity(Tour model)
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
                DistanceKm = model.DistanceKm,
                EstimatedTimeHours = model.EstimatedTimeHours
            };
        }

        
    }
}
