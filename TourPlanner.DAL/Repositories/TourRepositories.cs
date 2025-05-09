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
        public async Task<TourEntity> AddTourAsync(TourEntity tour)
        {
            _dbContext.Tours.Add(tour);
            await _dbContext.SaveChangesAsync();
            return tour;
        }
    }
}
