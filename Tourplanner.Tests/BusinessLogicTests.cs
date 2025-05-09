using TourPlanner.BL.Models;

namespace Tourplanner.Tests
{
    public class BusinessLogicTests
    {

        Tour t = new();

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            Console.WriteLine(t);
        }
    }
}
