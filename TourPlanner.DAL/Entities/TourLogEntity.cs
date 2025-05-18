using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TourPlanner.DAL.Entities.Enums;

namespace TourPlanner.DAL.Entities
{
    public class TourLogEntity
    {
        public int Id { get; set; }

        public int TourId { get; set; }

        public DateTime LogDate { get; set; }
        public string? Comment { get; set; }

        public DifficultyLevel Difficulty { get; set; }

        public float DistanceKm { get; set; }

        public float DurationHours { get; set; }
        public SatisfactionRating Rating { get; set; }

        // Navitagtion Property to TourEntity
        public TourEntity? Tour { get; set; }
    }
}
