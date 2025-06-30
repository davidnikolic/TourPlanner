using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DAL.Entities;

namespace TourPlanner.DAL.Repositories.Interfaces
{
    public interface ITourRepository
    {
        TourEntity AddTour(TourEntity tour);
        IEnumerable<TourEntity> GetTours();

        void UpdateTour(TourEntity tour);
        void DeleteTour(int id);
        TourEntity GetTourById(int id);
        int GetLastTourId();
        IEnumerable<TourLogEntity> GetAllTourLogs();
    }
}
