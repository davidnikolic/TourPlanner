using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.UI.Interfaces;

namespace TourPlanner.UI.Services
{
    internal class MockDialogService : IDialogService
    {
        public TourLogDTO? DisplayTourLogPopUp(string title, TourLogDTO tourLog = null)
        {
            return new TourLogDTO()
            {
                Id = 1,
                Comment = "This is a sample comment",
                Difficulty = EnumsDTO.DifficultyLevel.easy,
                DurationHours = 1
            };
        }

        public TourDTO? DisplayTourPopUp(string title, TourDTO tour = null)
        {
            return new TourDTO()
            {
                Id = 1,
                Name = "Sample-Tour",
                Description = "This is a mock-tour",
                EstimatedTimeHours = 1
            };
        }
    }
}
