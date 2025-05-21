using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DAL;
using TourPlanner.DAL.Entities;

namespace TourPlanner.Tests
{
    public class TourRepositoryTests
    {
        private TourPlannerDBContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env");
            Env.Load(envPath);

            string host = Env.GetString("DB_HOST");
            string port = Env.GetString("DB_PORT");
            string dbName = Env.GetString("DB_NAME");
            string user = Env.GetString("DB_USER");
            string pass = Env.GetString("DB_PASSWORD");

            string connectionString = $"Host={host};Port={port};Database={dbName};Username={user};Password={pass}";

            var options = new DbContextOptionsBuilder<TourPlannerDBContext>()
                .UseNpgsql(connectionString)
                .Options;

            _dbContext = new TourPlannerDBContext(options);
        }

        // Real integration test with the db
        // Test: Adds a tour and checks if it is stored in the database
        [Test]
        public void AddTour_ShouldAddTourToDatabase()
        {
            // Arrange: Create a new TourEntity object
            var tour = new TourEntity
            {
                Name = "Test Tour",
                Description = "description",
                StartLocation = "Location A",
                EndLocation = "Location B",
                TransportType = Enums.TransportType.car,
                DistanceKm = 12.5f,
                EstimatedTimeHours = 2.5f,
                RouteImagePath = "/path/to/image"
            };

            // Act: Add the tour to the database and save the changes
            _dbContext.Tours.Add(tour);
            _dbContext.SaveChanges();

            // Assert: Retrieve the saved tour and verify the values
            var storedTour = _dbContext.Tours.Find(tour.Id);
            Assert.NotNull(storedTour);
            Assert.AreEqual(tour.Name, storedTour.Name);
            Assert.AreEqual(tour.StartLocation, storedTour.StartLocation);
            Assert.AreEqual(tour.TransportType, storedTour.TransportType);
        }

        [Test]
        public void GetAllTours_ShouldReturnListOfTours()
        {
            // Arrange: Add a test tour
            var tour = new TourEntity
            {
                Name = "List Tour",
                Description = "description",
                StartLocation = "Loc1",
                EndLocation = "Loc2",
                TransportType = Enums.TransportType.bike,
                DistanceKm = 8.0f,
                EstimatedTimeHours = 1.5f,
                RouteImagePath = "/path/list"
            };

            _dbContext.Tours.Add(tour);
            _dbContext.SaveChanges();

            // Act: Get all tours
            var tours = _dbContext.Tours.ToList();

            // Assert: Check if at least one tour is returned
            Assert.IsNotEmpty(tours);
            Assert.IsTrue(tours.Any(t => t.Id == tour.Id));
        }

        [Test]
        public void UpdateTour_ShouldUpdateTourDetails()
        {
            // Arrange: Add a new tour
            var tour = new TourEntity
            {
                Name = "Old Name",
                Description = "Old desc",
                StartLocation = "Old Start",
                EndLocation = "Old End",
                TransportType = Enums.TransportType.car,
                DistanceKm = 5,
                EstimatedTimeHours = 1,
                RouteImagePath = "/old/path"
            };

            _dbContext.Tours.Add(tour);
            _dbContext.SaveChanges();

            // Act: Update the name and save
            tour.Name = "Updated Name";
            tour.Description = "Updated desc";
            _dbContext.Tours.Update(tour);
            _dbContext.SaveChanges();

            // Assert: Retrieve and verify
            var updatedTour = _dbContext.Tours.Find(tour.Id);
            Assert.NotNull(updatedTour);
            Assert.AreEqual("Updated Name", updatedTour.Name);
            Assert.AreEqual("Updated desc", updatedTour.Description);
        }

        [Test]
        public void DeleteTour_ShouldRemoveTourFromDatabase()
        {
            // Arrange: Add a tour
            var tour = new TourEntity
            {
                Name = "Delete Me",
                Description = "desc",
                StartLocation = "X",
                EndLocation = "Y",
                TransportType = Enums.TransportType.foot,
                DistanceKm = 3,
                EstimatedTimeHours = 0.5f,
                RouteImagePath = "/delete/path"
            };

            _dbContext.Tours.Add(tour);
            _dbContext.SaveChanges();

            // Act: Remove the tour
            _dbContext.Tours.Remove(tour);
            _dbContext.SaveChanges();

            // Assert: Try to find it again
            var deletedTour = _dbContext.Tours.Find(tour.Id);
            Assert.IsNull(deletedTour);
        }

        [TearDown]
        public void Cleanup()
        {
            // Alle Test-Touren, die bestimmte Namen haben, entfernen
            var testTourNames = new[] { "List Tour", "Updated Name", "Old Name", "Delete Me", "Test Tour" };

            var toursToDelete = _dbContext.Tours
                .Where(t => testTourNames.Contains(t.Name))
                .ToList();

            if (toursToDelete.Any())
            {
                _dbContext.Tours.RemoveRange(toursToDelete);
                _dbContext.SaveChanges();
            }

            _dbContext.Dispose();
        }
    }
}
