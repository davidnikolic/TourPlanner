using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Moq;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.MockRepos;
using TourPlanner.BL.Services;
using TourPlanner.DAL.Repositories.Interfaces;
using TourPlanner.UI.Services;
using TourPlanner.UI.ViewModels;

namespace TourPlanner.Tests
{
    public class SearchBarTests
    {
        private Mock<ITourService> _mockTourService;
        private Mock<ITourLogService> _mockTourLogService;
        private SearchService _searchService;
        private List<TourDTO> _testTours;
        private List<TourLogDTO> _testTourLogs;
        [SetUp]
        public void Setup()
        {
            // Arrange - Setup mocks and test data
            _mockTourService = new Mock<ITourService>();
            _mockTourLogService = new Mock<ITourLogService>();
            _searchService = new SearchService(_mockTourService.Object, _mockTourLogService.Object);

            _testTours = new List<TourDTO>
            {
                new TourDTO { Id = 1, Name = "Alpine Adventure", Description = "Mountain tour", StartLocation = "Salzburg" },
                new TourDTO { Id = 2, Name = "City Bike Tour", Description = "Urban cycling", StartLocation = "Vienna" }
            };

            _testTourLogs = new List<TourLogDTO>
            {
                new TourLogDTO { Id = 1, TourId = 1, Comment = "Great mountain views", Rating = EnumsDTO.SatisfactionRating.veryDissatisfied },
                new TourLogDTO { Id = 2, TourId = 2, Comment = "Nice city ride", Rating = EnumsDTO.SatisfactionRating.satisfied }
            };

            _mockTourService.Setup(x => x.GetTours()).Returns(_testTours);
            _mockTourLogService.Setup(x => x.GetAllTourLogs()).Returns(_testTourLogs);
        }

        [Test]
        public void TestSearchFullName()
        {
            // Act - Search for complete tour name
            var result = _searchService.SearchTours("Alpine Adventure");

            // Assert - Should find exact match
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Alpine Adventure"));
        }

        [Test]
        public void TestSearchPartOfName()
        {
            // Act - Search for partial name (case insensitive)
            var result = _searchService.SearchTours("bike");

            // Assert - Should find tour containing "bike"
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("City Bike Tour"));
        }

        [Test]
        public void TestSearchLogName()
        {
            // Act - Search in tour log comments
            var result = _searchService.SearchTours("mountain views");

            // Assert - Should find tour with matching log comment
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Id, Is.EqualTo(1));
        }

        [Test]
        public void TestSearchPopularity()
        {
            // Arrange - Setup mock for popularity search
            _mockTourLogService.Setup(x => x.SearchTourLogs("popularity:high"))
                              .Returns(_testTourLogs.Where(l => l.TourId == 1).ToList());

            // Act - Search for popular tours
            var result = _searchService.SearchTours("popularity:high");

            // Assert - Should find tour with high popularity
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Id, Is.EqualTo(1));
        }

        [Test]
        public void TestSearchEmpty()
        {
            // Act - Search with empty string
            var result = _searchService.SearchTours("");

            // Assert - Should return all tours
            Assert.That(result.Count, Is.EqualTo(_testTours.Count));
        }
    }
}
