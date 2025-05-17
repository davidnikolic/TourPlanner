using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;

namespace TourPlanner.BL.MockRepos
{
    public class MockTourRepo
    {
        public List<TourDTO> Tours { get; set; } = new List<TourDTO>();

        public void AddTour(TourDTO tour)
        {
            Tours.Add(tour);
        }

        public List<TourDTO> GetTours() { 
            return Tours; 
        }

        public TourDTO GetTour(int id)
        {
            return Tours.FirstOrDefault(i => i.Id == id);
        }

        public void UpdateTour(TourDTO tour)
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
