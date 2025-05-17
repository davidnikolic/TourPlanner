using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TourPlanner.DAL.Entities.Enums;

namespace TourPlanner.BL.DTOs
{
    public class TourLogDTO
    {
        
        public int Id { get; set; }

        public int TourId { get; set; }

        public DateTime LogDate { get; set; } = DateTime.Now;
        public string? Comment { get; set; } = "";

        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.easy;

        public float DistanceKm { get; set; } = 0;

        public float DurationHours { get; set; } = 0;
        public SatisfactionRating Rating { get; set; } = SatisfactionRating.neutral;
    }
}
