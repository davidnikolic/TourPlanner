using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        ITourRepository tourRepository;
        ITourLogRepository tourLogRepository;

        ITourService tourService;
        MockDialogService dialogService;
        ISelectedTourService selectedTourService;

        TourListViewModel tourListViewModel;
        TourLogsViewModel tourLogsViewModel;


        [SetUp]
        public void Setup()
        {
            tourRepository = new MockTourRepo();
            tourLogRepository = new MockTourLogsRepo();

            tourService = new TourService(tourRepository);
            dialogService = new MockDialogService();
            selectedTourService = new SelectedTourService();
            tourLogsViewModel = new TourLogsViewModel(, selectedTourService, dialogService);

            tourListViewModel = new(tourService, selectedTourService, dialogService, tourLogsViewModel);
        }

        [Test]
        public void TestSearchFullName()
        {
            //Arrange
            TourDTO tour1 = new TourDTO()
            {
                Name = "Tour 1",
                DistanceKm = 10,
                EstimatedTimeHours = 2
            };

            TourDTO tour2 = new TourDTO()
            {
                Name = "Tour 2",
                DistanceKm = 5,
                EstimatedTimeHours = 1
            };

            TourDTO tour3 = new TourDTO()
            {
                Name = "Tour 3",
                DistanceKm = 5,
                EstimatedTimeHours = 1
            };

            //Act
            tourLogsViewModel.SearchQuery = "Tour 1";

            //Assert
            Assert.That(tourListViewModel.Tours.Count, Is.EqualTo(1));
            Assert.That(tourListViewModel.Tours.FirstOrDefault().Name, Is.EqualTo("Tour 1"));

        }

        [Test]
        public void TestSearchPartOfName()
        {

        }

        [Test]
        public void TestSearchLogName()
        {

        }

        [Test]
        public void TestSearchPopularity()
        {

        }
    }
}
