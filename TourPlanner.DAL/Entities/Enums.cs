using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.DAL.Entities
{
    public class Enums
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
