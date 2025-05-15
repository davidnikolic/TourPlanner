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
        public async Task AddTourAsync(TourEntity tour)
        {
            _dbContext.Tours.Add(tour);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<TourEntity>> GetAllToursAsync()
        {
            return await _dbContext.Tours.ToListAsync();
        }

        public IEnumerable<TourEntity> GetAllTours()
        {
            return _dbContext.Tours.ToList();
        }

        public async Task UpdateTourAsync(TourEntity tour)
        {
            var existingTour = await _dbContext.Tours.FindAsync(tour.Id);
            if (existingTour == null)
                return;

            // Update properties
            _dbContext.Entry(existingTour).CurrentValues.SetValues(tour);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTourAsync(int id)
        {
            var tour = await _dbContext.Tours.FindAsync(id);
            if (tour == null)
                return;

            _dbContext.Tours.Remove(tour);
            await _dbContext.SaveChangesAsync();
        }

    }
}
