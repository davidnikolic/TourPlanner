using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;

namespace TourPlanner.Tests.Mocks.Services
{
    public class MockSelectedTourService : ISelectedTourService
    {
        public TourDTO SelectedTour { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event Action<TourDTO> SelectedTourChanged;
    }
}
