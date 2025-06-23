using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BL.DTOs
{
    public class TourStatisticsDTO
    {
        public string TourName { get; set; }
        public int TourId { get; set; }
        public float AvgDifficulty { get; set; }
        public float AvgRating { get; set; }
        public int Popularity { get; set; }
        public bool IsChildFriendly { get; set; }
    }
}
