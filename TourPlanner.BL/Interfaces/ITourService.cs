using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Models;
using TourPlanner.DAL.Entities;

namespace TourPlanner.BL.Interfaces
{
    public interface ITourService
    {
        Task AddTour(TourEntity tour);

        Task <IEnumerable<TourEntity>> GetTours();
    }
}
