using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BL.Models
{
    public class TourLog
    {
        public int Id { get; set; }

        public int TourId { get; set; }

        public Tour Tour { get; set; }

        public DateTime LogDate { get; set; }
        public string? Comment { get; set; }

        public string Difficulty { get; set; } = "easy";

        public float DistanceKm { get; set; }

        public float DurationHours { get; set; }
        public string Rating { get; set; }
    }
}
