using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BL.Models
{
    public class Tour
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string StartLocation { get; set; }

        public string EndLocation { get; set; }

        public TransportType TransportType { get; set; }

        public double Distance { get; set; }

        public TimeSpan EstimatedTime { get; set; }

        public string RouteImagePath { get; set; }


    }
}

public enum TransportType
{
    Car,
    Bike,
    Foot,
    Boat,
}