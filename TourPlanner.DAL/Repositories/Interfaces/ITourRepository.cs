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

        Task UpdateTourAsync(TourEntity tour);

        Task DeleteTourAsync(int id); 
        void AddTour(TourEntity tour);
        IEnumerable<TourEntity> GetTours();

        void UpdateTour(TourEntity tour);
        void DeleteTour(int id);   
    }
}
