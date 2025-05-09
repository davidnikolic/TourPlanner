using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DAL.Entities;
using TourPlanner.BL.MockRepos;

namespace TourPlanner.BL.Services
{
    internal class TourService
    {
        public MockTourRepo MockTourRepo { get; set; } = new MockTourRepo();
    }
}
