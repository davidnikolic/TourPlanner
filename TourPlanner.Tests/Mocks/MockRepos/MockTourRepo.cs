using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.DAL.Entities;
using TourPlanner.DAL.Repositories.Interfaces;

namespace TourPlanner.BL.MockRepos
{
    public class MockTourRepo : ITourRepository
    {
        public List<TourEntity> Tours { get; set; } = new List<TourEntity>();

        public TourEntity AddTour(TourEntity tour)
        {
            Tours.Add(tour);

            return tour;
        }

        public IEnumerable<TourEntity> GetTours()
        {
            return Tours;
        }

        public void UpdateTour(TourEntity tour)
        {
            if (Tours.FindIndex(t => t.Id == tour.Id) >= 0)
            {
                DeleteTour(tour.Id);
                AddTour(tour);
            }

        }

        public TourEntity GetTourById(int id)
        {
            return Tours.FirstOrDefault(t => t.Id == id);
        }

        public int GetLastTourId()
        {
            return Tours.Max(t => t.Id);
        }
        public void DeleteTour(int id)
        {
            Tours.RemoveAll(i => i.Id == id);
        }

        public IEnumerable<TourLogEntity> GetAllTourLogs()
        {
            throw new NotImplementedException();
        }
    }
}
