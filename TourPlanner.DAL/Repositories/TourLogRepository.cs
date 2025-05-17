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
    public class TourLogRepository : ITourLogRepository
    {
        private readonly TourPlannerDBContext _dbContext;

        public TourLogRepository(TourPlannerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTourLogAsync(TourLogEntity log)
        {
            _dbContext.TourLogs.Add(log);
            await _dbContext.SaveChangesAsync();
        }

        public void AddTourLog(TourLogEntity log)
        {
            _dbContext.TourLogs.Add(log);
            _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<TourLogEntity>> GetAllTourLogsForTourAsync(int tourId)
        {
            return await _dbContext.TourLogs
                .Where(log => log.TourId == tourId)
                .ToListAsync();
        }

        public IEnumerable<TourLogEntity> GetAllTourLogsForTour(int tourId)
        {
            return _dbContext.TourLogs
                .Where(log => log.TourId == tourId)
                .ToList();
        }

        public async Task UpdateTourLogAsync(TourLogEntity log)
        {
            var existingLog = await _dbContext.TourLogs.FindAsync(log.Id);
            if (existingLog == null) return;

            _dbContext.Entry(existingLog).CurrentValues.SetValues(log);
            await _dbContext.SaveChangesAsync();
        }

        public void UpdateTourLog(TourLogEntity log)
        {
            var existingLog = _dbContext.TourLogs.Find(log.Id);
            if (existingLog == null) return;

            _dbContext.Entry(existingLog).CurrentValues.SetValues(log);
            _dbContext.SaveChanges();
        }

        public async Task DeleteTourLogAsync(int logId)
        {
            var log = await _dbContext.TourLogs.FindAsync(logId);
            if (log == null) return;

            _dbContext.TourLogs.Remove(log);
            await _dbContext.SaveChangesAsync();
        }

        public void DeleteTourLog(int logId)
        {
            var log = _dbContext.TourLogs.Find(logId);
            if (log == null) return;

            _dbContext.TourLogs.Remove(log);
            _dbContext.SaveChanges();
        }
    }
}
