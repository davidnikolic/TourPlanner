using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.MockRepos;
using TourPlanner.BL.Models;
using TourPlanner.BL.Interfaces;
using TourPlanner.DAL.Entities;
using TourPlanner.DAL.Repositories.Interfaces;

namespace TourPlanner.BL.Services
{
    public class TourService : ITourService
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
        private readonly ITourRepository _tourRepository;

        public TourService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }
        public async Task AddTour(TourEntity tour)
        {
           await _tourRepository.AddTourAsync(tour);
        }
    }
}
