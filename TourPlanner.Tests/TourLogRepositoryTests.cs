using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using TourPlanner.DAL;
using TourPlanner.DAL.Entities;
using TourPlanner.DAL.Repositories;

namespace TourPlanner.Tests
{
    public class TourLogRepositoryTests
    {
        private TourPlannerDBContext _dbContext;
        private TourLogRepository _repository;

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

            _repository = new TourLogRepository(_dbContext);
        }

        [Test]
        public void AddTourLog_ShouldAddTourLogToDatabase()
        {
            // Arrange
            var log = new TourLogEntity
            {
                // existing Tour 
                TourId = 18,                        
                LogDate = DateTime.UtcNow,          
                Comment = "Test Log für Unit Test",
                Difficulty = Enums.DifficultyLevel.medium,
                DistanceKm = 12.3f,
                DurationHours = 2.5f,
                Rating = Enums.SatisfactionRating.satisfied
            };

            // Act
            _repository.AddTourLog(log);

            // Assert
            var storedLog = _dbContext.TourLogs.FirstOrDefault(l => l.Comment == "Test Log für Unit Test");
            Assert.NotNull(storedLog);
            Assert.AreEqual(log.TourId, storedLog.TourId);
            Assert.AreEqual(log.Difficulty, storedLog.Difficulty);
            Assert.AreEqual(log.DistanceKm, storedLog.DistanceKm);
            Assert.AreEqual(log.DurationHours, storedLog.DurationHours);
            Assert.AreEqual(log.Rating, storedLog.Rating);
        }

        [Test]
        public void GetAllTourLogsForTour_ShouldReturnAllLogsForGivenTour()
        {
            // Arrange
            int tourId = 1;

            // Act
            var logs = _repository.GetAllTourLogsForTour(tourId);

            // Assert
            Assert.IsNotNull(logs);
            Assert.IsTrue(logs.All(l => l.TourId == tourId));
        }

        [Test]
        public void UpdateTourLog_ShouldUpdateExistingLog()
        {
            // Arrange
            var log = new TourLogEntity
            {
                TourId = 18,
                LogDate = DateTime.UtcNow,
                Comment = "Original Comment",
                Difficulty = Enums.DifficultyLevel.medium,
                DistanceKm = 5.0f,
                DurationHours = 1.0f,
                Rating = Enums.SatisfactionRating.neutral
            };
            _repository.AddTourLog(log);

            var savedLog = _dbContext.TourLogs.FirstOrDefault(l => l.Comment == "Original Comment");
            Assert.NotNull(savedLog);

            // Update 
            savedLog.Comment = "Updated Comment";
            savedLog.Rating = Enums.SatisfactionRating.verySatisfied;

            // Act
            _repository.UpdateTourLog(savedLog);

            // Assert
            var updatedLog = _dbContext.TourLogs.Find(savedLog.Id);
            Assert.AreEqual("Updated Comment", updatedLog.Comment);
            Assert.AreEqual(Enums.SatisfactionRating.verySatisfied, updatedLog.Rating);
        }

        [Test]
        public void DeleteTourLog_ShouldRemoveLogFromDatabase()
        {
            // Arrange
            var log = new TourLogEntity
            {
                TourId = 18,
                LogDate = DateTime.UtcNow,
                Comment = "Log to Delete",
                Difficulty = Enums.DifficultyLevel.easy,
                DistanceKm = 3.2f,
                DurationHours = 0.5f,
                Rating = Enums.SatisfactionRating.dissatisfied
            };
            _repository.AddTourLog(log);

            var savedLog = _dbContext.TourLogs.FirstOrDefault(l => l.Comment == "Log to Delete");
            Assert.NotNull(savedLog);

            // Act
            _repository.DeleteTourLog(savedLog.Id);

            // Assert
            var deletedLog = _dbContext.TourLogs.Find(savedLog.Id);
            Assert.IsNull(deletedLog);
        }


        [TearDown]
        public void Cleanup()
        {
            _dbContext.Dispose();
        }
    }
}
