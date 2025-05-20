using TourPlanner.BL.DTOs;
using TourPlanner.BL.Services;
using TourPlanner.BL.MockRepos;

namespace Tourplanner.Tests
{
    public class BusinessLogicTests
    {

        MockTourRepo mockTourRepo = new();
        TourDTO t = new();


        [SetUp]
        public void Setup()
        {
            t.Id = 1;
            t.Name = "Testwanderung";
        }

        [Test]
        public void TestAddTour()
        {
            mockTourRepo.AddTour(t);
            Assert.That(mockTourRepo.Tours.First().Id, Is.EqualTo(t.Id));
        }

        [Test]
        public void TestReadTour()
        {
            TourDTO tour = mockTourRepo.GetTour(1);
            
            Assert.That(tour.Name, Is.EqualTo("Testwanderung"));
        }

        [Test]
        public void TestUpdateTour()
        {
            TourDTO OriginalTour = new TourDTO();

            OriginalTour.Id = 2;
            OriginalTour.Name = "Testdorf";



            mockTourRepo.AddTour(OriginalTour);

            TourDTO ChangedTour = OriginalTour;

            ChangedTour.EstimatedTimeHours = 2;

            mockTourRepo.UpdateTour(ChangedTour);

            Assert.That(mockTourRepo.Tours.Where(i => i.Id == 2).First().Name, Is.EqualTo("Testdorf"));
            Assert.That(mockTourRepo.Tours.Where(i => i.Id == 2).First().EstimatedTimeHours, Is.EqualTo(2));
        }

        [Test]
        public void TestRemoveTour()
        {
            mockTourRepo.DeleteTour(t.Id);
            Assert.That(mockTourRepo.Tours, Is.Empty); 
        }
    }
}
