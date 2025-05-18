using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BL.DTOs
{
    public class EnumsDTO
    {
        public enum TransportType
        {
            bike,
            hike,
            car,
            foot
        }

        public enum DifficultyLevel
        {
            easy,
            medium,
            hard,
            expert
        }

        public enum SatisfactionRating
        {
            veryDissatisfied,
            dissatisfied,
            neutral,
            satisfied,
            verySatisfied
        }
    }
}
