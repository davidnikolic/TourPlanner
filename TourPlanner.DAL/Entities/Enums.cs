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
            Easy,
            Medium,
            Hard,
            Expert
        }

        public enum SatisfactionRating
        {
            VeryDissatisfied,
            Dissatisfied,
            Neutral,
            Satisfied,
            VerySatisfied
        }
    }
}
