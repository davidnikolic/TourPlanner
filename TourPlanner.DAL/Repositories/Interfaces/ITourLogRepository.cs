using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DAL.Entities;

namespace TourPlanner.DAL.Repositories.Interfaces
{
    public interface ITourLogRepository
    {
        Task AddTourLogAsync(TourLogEntity log);
        void AddTourLog(TourLogEntity log);

        Task<IEnumerable<TourLogEntity>> GetAllTourLogsForTourAsync(int tourId);
        IEnumerable<TourLogEntity> GetAllTourLogsForTour(int tourId);

        Task UpdateTourLogAsync(TourLogEntity log);
        void UpdateTourLog(TourLogEntity log);

        Task DeleteTourLogAsync(int logId);
        void DeleteTourLog(int logId);
    }
}
