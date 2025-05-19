using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BL.DTOs
{
    /// <summary>
    /// The class for the enums, that will be used for the DTOs.
    /// </summary>
    public class EnumsDTO
    {
        /// <summary>
        /// The enum for the transport-type.
        /// </summary>
        public enum TransportType
        {
            bike,
            hike,
            car,
            foot
        }


        /// <summary>
        /// The enum for the diffficulty level.
        /// </summary>
        public enum DifficultyLevel
        {
            easy,
            medium,
            hard,
            expert
        }

        /// <summary>
        /// The enum for the Satisfaction-Rating.
        /// </summary>
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
