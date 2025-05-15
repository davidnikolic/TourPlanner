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
        public Task AddTour(Tour tour);

        public List<Tour> GetTours();
    }
}
