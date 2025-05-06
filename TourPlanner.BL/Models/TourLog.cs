using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BL.Models
{
    public class TourLog
    {
        public Guid Id { get; set; }

        public Guid TourId { get; set; }


        public DateTime TimeStamp { get; set; }

        public string Comment { get; set; }

        public Difficulty Difficulty { get; set; }

        public double Distance { get; set; }

        public TimeSpan Duration { get; set; }

        public SatisfactionRating SatisfactionRating { get; set; }
    }
}

public enum Difficulty
{
    VeryEasy,
    Easy,
    Medium,
    Hard,
    VeryHard,
    Impossible
}

public enum SatisfactionRating
{
    VeryDissatisfied,
    Dissatisfied,
    Neutral,
    Satisfied,
    VerySatisfied
}