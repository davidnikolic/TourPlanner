using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.MockRepos;
using TourPlanner.BL.Models;

namespace TourPlanner.BL.Services
{
    internal class TourService
    {
        public MockTourRepo MockTourRepo { get; set; } = new MockTourRepo();

        public TourService(MockTourRepo mockTourRepo)
        {
            this.MockTourRepo = mockTourRepo;
        }

        public List<Tour> GetTours()
        {
            return MockTourRepo.GetTours();
        }

        public void AddTour(Tour tour)
        {
            MockTourRepo.AddTour(tour);
        }
    }
}
