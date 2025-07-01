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
        public void TestSearchFullName()
        {
   
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
