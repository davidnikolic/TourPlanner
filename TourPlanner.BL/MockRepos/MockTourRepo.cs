using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Models;

namespace TourPlanner.BL.MockRepos
{
    public class MockTourRepo
    {
        public List<Tour> Tours { get; set; } = new List<Tour>();

        public void AddTour(Tour tour)
        {
            Tours.Add(tour);
        }

        public List<Tour> GetTours() { 
            return Tours; 
        }

        public Tour GetTour(int id)
        {
            return Tours.FirstOrDefault(i => i.Id == id);
        }

        public void UpdateTour(Tour tour)
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
