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
        Task<TourEntity> AddTourAsync(TourEntity tour);
    }
}
