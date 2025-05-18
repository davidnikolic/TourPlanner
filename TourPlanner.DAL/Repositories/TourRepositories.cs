using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DAL.Entities;
using TourPlanner.DAL.Repositories.Interfaces;

namespace TourPlanner.DAL.Repositories
{
    public class TourRepositories : ITourRepository
    {
        private readonly TourPlannerDBContext _dbContext;
        public TourRepositories(TourPlannerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddTour(TourEntity tour)
        {
            _dbContext.Tours.Add(tour);
            _dbContext.SaveChanges();
        }

        public IEnumerable<TourEntity> GetTours()
        {
            return _dbContext.Tours.ToList();
        }

        public void UpdateTour(TourEntity tour)
        {
            var existingTour = _dbContext.Tours.Find(tour.Id);
            if (existingTour == null)
                return;

            // Update properties
            _dbContext.Entry(existingTour).CurrentValues.SetValues(tour);
            _dbContext.SaveChanges();
        }

        public void DeleteTour(int id)
        {
            var tour = _dbContext.Tours.Find(id);
            if (tour == null)
                return;

            _dbContext.Tours.Remove(tour);
            _dbContext.SaveChanges();
        }

    }
}
