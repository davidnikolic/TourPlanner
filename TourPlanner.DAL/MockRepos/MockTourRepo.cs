using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DAL.Entities;

namespace TourPlanner.DAL.MockRepos
{
    public class MockTourRepo
    {
        public List<TourEntity> Tours { get; set; } = new List<TourEntity>();

        public void AddTour(TourEntity tour)
        {
            Tours.Add(tour);
        }

        public TourEntity GetTour(int id)
        {
            return Tours.FirstOrDefault(i => i.Id == id);
        }

        public void UpdateTour(TourEntity tour)
        {
            if (Tours.Where(i => i.Id ==  tour.Id).FirstOrDefault() != null)
            {
                DeleteTour(tour.Id);
                AddTour(tour);
            }

        }

        public void DeleteTour(int id)
        {
            Tours.RemoveAll(i => i.Id == id);
        }
    }
}
