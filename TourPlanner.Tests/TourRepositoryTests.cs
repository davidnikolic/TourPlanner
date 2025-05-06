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
        public async Task AddTourAsync_ShouldAddTourToDatabase()
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
            await _dbContext.Tours.AddAsync(tour);
            await _dbContext.SaveChangesAsync();

            // Assert: Retrieve the saved tour and verify the values
            var storedTour = await _dbContext.Tours.FindAsync(tour.Id);
            Assert.NotNull(storedTour);
            Assert.AreEqual(tour.Name, storedTour.Name); 
            Assert.AreEqual(tour.StartLocation, storedTour.StartLocation);
            Assert.AreEqual(tour.TransportType, storedTour.TransportType);
        }

        [TearDown]
        public void Cleanup()
        {
            _dbContext.Dispose(); 
        }
    }
}
