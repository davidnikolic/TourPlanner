using static TourPlanner.BL.DTOs.EnumsDTO;
using System.Text.Json.Serialization;

namespace TourPlanner.BL.DTOs
{
    public class TourLogDTO
    {
        [JsonIgnore]
        [CsvHelper.Configuration.Attributes.Ignore]
        public int Id { get; set; }

        [JsonIgnore]
        [CsvHelper.Configuration.Attributes.Ignore]
        public int TourId { get; set; }

        public DateTime LogDate { get; set; } = DateTime.Now;
        public string? Comment { get; set; } = "";

        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.easy;

        public float DistanceKm { get; set; } = 0;

        public float DurationHours { get; set; } = 0;
        public SatisfactionRating Rating { get; set; } = SatisfactionRating.neutral;
    }
}
