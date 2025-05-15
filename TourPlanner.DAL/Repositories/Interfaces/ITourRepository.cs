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
        Task AddTourAsync(TourEntity tour);
        Task<IEnumerable<TourEntity>> GetAllToursAsync();

        void AddTour(TourEntity tour);
        IEnumerable<TourEntity> GetTours();

        Task UpdateTourAsync(TourEntity tour);
        Task DeleteTourAsync(int id);
    }
}
