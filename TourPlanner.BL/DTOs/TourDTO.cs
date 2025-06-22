using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using static TourPlanner.BL.DTOs.EnumsDTO;

namespace TourPlanner.BL.DTOs
{
    public class TourDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string? Description { get; set; } = "";

        public string StartLocation { get; set; } = "";

        public string EndLocation { get; set; } = "";

        public TransportType TransportType { get; set; } = TransportType.foot;

        public float DistanceKm { get; set; } = 0;

        public float EstimatedTimeHours { get; set; } = 0;

        public string? RouteImagePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "res", "dummy-map.jpg");

        public ICollection<TourLogDTO> TourLogs { get; set; } = new List<TourLogDTO>();
    }
}
