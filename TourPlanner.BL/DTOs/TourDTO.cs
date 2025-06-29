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

        public string? RouteImagePath { get; set; }

        public ICollection<TourLogDTO> TourLogs { get; set; } = new List<TourLogDTO>();

        public TourDTO()
        {
            string outputDir = Path.Combine(AppContext.BaseDirectory, "GeneratedImages");
            Directory.CreateDirectory(outputDir);
            string fileName = Guid.NewGuid() + ".png";
            RouteImagePath = Path.Combine(outputDir, fileName);
        }
    }
}
