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

namespace TourPlanner.BL.Services
{
    public class TourService : ITourService
    {
        public ITourRepository _tourRepository;

        public TourService(ITourRepository TourRepo)
        {
            _tourRepository = TourRepo;
        }

        public async List<Tour> GetTours()
        {
            List<TourEntity> t = (List<TourEntity>) await _tourRepository.GetAllToursAsync();

            List<Tour> res = t.ForEach(t => ToModel(t));
        }

        public async Task AddTour(TourEntity tour)
        {
           await _tourRepository.AddTourAsync(tour);
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
                DistanceKm = entity.DistanceKm
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
                DistanceKm = model.DistanceKm
            };
        }
    }
}
