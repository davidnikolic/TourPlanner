using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using static TourPlanner.DAL.Entities.Enums;

namespace TourPlanner.BL.Models
{
    public class Tour
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string? Description { get; set; } = "";

        public string StartLocation { get; set; } = "";

        public string EndLocation { get; set; } = "";

        public TransportType TransportType { get; set; } = TransportType.foot;

        public float? DistanceKm { get; set; } = 0;

        public float? EstimatedTimeHours { get; set; } = 0;

        public string? RouteImagePath { get; set; } = "";

        public ICollection<TourLog> TourLogs { get; set; } = new List<TourLog>();
    }
}
