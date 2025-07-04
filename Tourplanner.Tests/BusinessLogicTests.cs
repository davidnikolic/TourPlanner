using TourPlanner.BL.DTOs;
using TourPlanner.BL.Services;
using TourPlanner.BL.MockRepos;
using TourPlanner.BL.Interfaces;
using TourPlanner.DAL.Repositories.Interfaces;

namespace TourPlanner.Tests
{
    public class BusinessLogicTests
    {
        
        ITourRepository tourRepository;
        ITourService tourService;

        ITourLogRepository tourLogRepository;
        ITourLogService tourLogService;

        [SetUp]
        public void Setup()
        {
            tourRepository = new MockTourRepo();
            tourService = new TourService(tourRepository, null);
        }

        [Test]
        public void TestAddTour()
        {
            // Arrange
            TourDTO t = new()
            {
                Id = 1,
                Name = "Test",
                Description = "This is dummy data",
                StartLocation = "A",
                EndLocation = "B",
            };

            // Act
            tourService.AddTour(t);

            // Assert
            Assert.That(tourRepository.GetTours().Count,
                        Is.EqualTo(1));

            Assert.That(tourRepository.GetTours().First().Id, 
                Is.EqualTo(1));
        }

        [Test]
        public void TestGetTours()
        {

            // Arrange
            TourDTO t = new()
            {
                Id = 1,
                Name = "Test",
                Description = "This is dummy data",
                StartLocation = "A",
                EndLocation = "B",
            };

            TourDTO t2 = new()
            {
                Id = 2,
                Name = "Test",
                Description = "This is dummy data",
                StartLocation = "A",
                EndLocation = "B",
            };
            tourService.AddTour(t);
            tourService.AddTour(t2);

            // Act
            List<TourDTO> tours = tourService.GetTours().ToList();


            // Assert
            Assert.That(tours.Count, Is.EqualTo(2));
            Assert.That(tours.Any(t => t.Id == 1), Is.True);
            Assert.That(tours.Any(t => t.Id == 2), Is.True);
        }

        [Test]
        public void TestUpdateTour()
        {
            // Arrange
            TourDTO t = new()
            {
                Id = 2,
                Name = "Test",
                Description = "This is dummy data",
                StartLocation = "A",
                EndLocation = "B",
            };

            tourService.AddTour(t);

            // Act
            t.EndLocation = "C";
            tourService.UpdateTour(t);

            // Assert
            Assert.That(tourRepository.GetTours().Any(t => t.EndLocation == "C"), Is.True);

        }

        [Test]
        public void TestRemoveTour()
        {
            // Arrange
            TourDTO t = new()
            {
                Id = 2,
                Name = "Test",
                Description = "This is dummy data",
                StartLocation = "A",
                EndLocation = "B",
            };

            tourService.AddTour(t);

            // Act
            tourService.DeleteTour(t);

            // Assert
            Assert.That(tourRepository.GetTours().Count, Is.EqualTo(0));
        }
    }
}
