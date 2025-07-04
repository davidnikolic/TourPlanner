using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DAL.Entities;
using TourPlanner.DAL.Repositories.Interfaces;

namespace TourPlanner.BL.MockRepos
{
    public class MockTourLogsRepo : ITourLogRepository
    {
        public void AddTourLog(TourLogEntity log)
        {
            throw new NotImplementedException();
        }

        public Task AddTourLogAsync(TourLogEntity log)
        {
            throw new NotImplementedException();
        }

        public void DeleteTourLog(int logId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTourLogAsync(int logId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TourLogEntity> FindTourLogsByFullText(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TourLogEntity> GetAllTourLogs()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TourLogEntity> GetAllTourLogsForTour(int tourId)
        {
            throw new NotImplementedException();
        }

        public TourLogEntity GetTourLogById(int tourLogId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTourLog(TourLogEntity log)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTourLogAsync(TourLogEntity log)
        {
            throw new NotImplementedException();
        }
    }
}
