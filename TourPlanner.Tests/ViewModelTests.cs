﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.UI.ViewModels;
using TourPlanner.UI.Services;
using TourPlanner.UI.Interfaces;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.MockRepos;
using TourPlanner.DAL.Repositories.Interfaces;
using TourPlanner.BL.Services;
using TourPlanner.BL.DTOs;

namespace TourPlanner.Tests
{
    public class ViewModelTests
    {
        ITourRepository tourRepository;


        ITourService tourService;
        MockDialogService dialogService;
        ISelectedTourService selectedTourService;

        TourListViewModel tourListViewModel;

        [SetUp]
        public void Setup()
        {
            tourRepository = new MockTourRepo();

            tourService = new TourService(tourRepository);
            dialogService = new MockDialogService();
            selectedTourService = new SelectedTourService();

            tourListViewModel = new(tourService, selectedTourService, dialogService);
        }

        [Test]
        public void TestSelectedTourPropagation()
        {
            // Arrange
            TourDTO tour = new TourDTO()
            {
                Id = 1,
                Name = "Sample-Tour",
                Description = "This is a mock-tour",
                EstimatedTimeHours = 1
            };

            // Act
            tourListViewModel.SelectedTour = tour;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(selectedTourService.SelectedTour, Is.Not.Null);
                Assert.That(selectedTourService.SelectedTour.Id, Is.EqualTo(tour.Id));
                Assert.That(selectedTourService.SelectedTour.Name, Is.EqualTo("Sample-Tour"));
            });
        }

        [Test]
        public void AddCommand_ShouldAddTour_WhenDialogReturnsValidTour()
        {
            // Arrange
            TourDTO tour = new TourDTO()
            {
                Id = 1,
                Name = "Sample-Tour",
                Description = "This is a mock-tour",
                EstimatedTimeHours = 1
            };

            dialogService.sampleTour = tour;

            // Act
            tourListViewModel.AddCommand.Execute(null);

            // Assert
            var addedTour = tourRepository.GetTours().First();

            Assert.Multiple(() =>
            {
                Assert.That(addedTour.Id, Is.EqualTo(1));
                Assert.That(addedTour.Name, Is.EqualTo("Sample-Tour"));
                Assert.That(addedTour.Description, Is.EqualTo("This is a mock-tour"));
                Assert.That(addedTour.EstimatedTimeHours, Is.EqualTo(1));
            });
        }

        [Test]
        public void ModifyCommand_UpdatesExistingTour_WhenDialogReturnsNewValues()
        {
            // Arrange
            TourDTO tour = new TourDTO()
            {
                Id = 1,
                Name = "Sample-Tour",
                Description = "This is a mock-tour",
                EstimatedTimeHours = 1
            };

            dialogService.sampleTour = tour;

            // Act
            tourListViewModel.AddCommand.Execute(null);

            tour.EstimatedTimeHours = 2;

            tourListViewModel.SelectedTour = tour;

            tourListViewModel.ModifyCommand.Execute(null);

            // Assert
            var addedTour = tourRepository.GetTours().First();

            Assert.Multiple(() =>
            {
                Assert.That(addedTour.Id, Is.EqualTo(1));
                Assert.That(addedTour.Name, Is.EqualTo("Sample-Tour"));
                Assert.That(addedTour.Description, Is.EqualTo("This is a mock-tour"));
                Assert.That(addedTour.EstimatedTimeHours, Is.EqualTo(2));
            });
        }

        [Test]
        public void DeleteCommand_ShouldDeleteTour_WhenTourIsSelected()
        {
            // Arrange
            TourDTO tour = new TourDTO()
            {
                Id = 1,
                Name = "Sample-Tour",
                Description = "This is a mock-tour",
                EstimatedTimeHours = 1
            };

            dialogService.sampleTour = tour;

            // Act
            tourListViewModel.AddCommand.Execute(null);

            tourListViewModel.SelectedTour = tour;

            tourListViewModel.DeleteCommand.Execute(null);

            // Assert
            Assert.That(tourRepository.GetTours().Count, Is.EqualTo(0));
        }
    }
}
