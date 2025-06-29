using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using static TourPlanner.BL.DTOs.EnumsDTO;

namespace TourPlanner.UI.ViewModels.Components
{
    public class FormFieldFactory
    {
        public static ObservableCollection<FormField> CreateForTourLog(TourLogDTO? dto)
        {
            var fields = new ObservableCollection<FormField>
        {
            new() { Label = "Date", Type = "DatePicker", Value = dto?.LogDate ?? DateTime.Now },
            new() { Label = "Comment", Type = "TextBox", Value = dto?.Comment ?? "" },
            new() { Label = "Difficulty", Type = "ComboBox", Value = dto?.Difficulty ?? DifficultyLevel.medium, Options = Enum.GetValues(typeof(DifficultyLevel)).Cast<object>() },
            new() { Label = "Distance (in km)", Type = "TextBox", Value = dto?.DistanceKm ?? 0 },
            new() { Label = "Duration (in h)", Type = "TextBox", Value = dto?.DurationHours ?? 0 },
            new() { Label = "Rating", Type = "ComboBox", Value = dto?.Rating ?? SatisfactionRating.neutral, Options = Enum.GetValues(typeof(SatisfactionRating)).Cast<object>() }
        };

            return fields;
        }

        public static ObservableCollection<FormField> CreateForTour(TourDTO? dto)
        {
            return new ObservableCollection<FormField>
            {
                new() { Label = "Name", Type = "TextBox", Value = dto?.Name ?? "" },
                new() { Label = "Comment", Type = "TextBox", Value = dto?.Description ?? "" },
                new() { Label = "StartLocation", Type = "TextBox", Value = dto?.StartLocation ?? "" },
                new() { Label = "EndLocation", Type = "TextBox", Value = dto?.EndLocation ?? "" },
                new() { Label = "Transporttype", Type = "ComboBox", Value = dto?.TransportType ?? TransportType.foot, Options = Enum.GetValues(typeof(TransportType)).Cast<object>() },
                new() { Label = "Distance (in km)", Type = "TextBox", Value = dto?.DistanceKm ?? 0f },
                new() { Label = "Duration (in h)", Type = "TextBox", Value = dto?.EstimatedTimeHours ?? 0f }
            };
        }

        public static TourLogDTO ToTourLogDTO(ObservableCollection<FormField> fields)
        {
            var dict = fields.ToDictionary(f => f.Label, f => f.Value);

            return new TourLogDTO
            {
                LogDate = (DateTime)(dict["Date"] ?? DateTime.Today),
                Comment = dict["Comment"]?.ToString() ?? "",
                Difficulty = (DifficultyLevel)(dict["Difficulty"] ?? DifficultyLevel.medium),
                DistanceKm = float.TryParse(dict["Distance (in km)"]?.ToString(), out var km) ? km : 0f,
                DurationHours = float.TryParse(dict["Duration (in h)"]?.ToString(), out var h) ? h : 0f,
                Rating = (SatisfactionRating)(dict["Rating"] ?? SatisfactionRating.neutral)
            };
        }

        public static TourDTO ToTourDTO(ObservableCollection<FormField> fields)
        {
            var dict = fields.ToDictionary(f => f.Label, f => f.Value);

            return new TourDTO
            {
                Name = dict["Name"]?.ToString() ?? "",
                Description = dict["Comment"]?.ToString() ?? "",
                StartLocation = dict["StartLocation"]?.ToString() ?? "",
                EndLocation = dict["EndLocation"]?.ToString() ?? "",
                TransportType = (TransportType)(dict["Transporttype"] ?? TransportType.foot),
                DistanceKm = float.TryParse(dict["Distance (in km)"]?.ToString(), out var km) ? km : 0f,
                EstimatedTimeHours = float.TryParse(dict["Duration (in h)"]?.ToString(), out var hours) ? hours : 0f
            };
        }
    }
}
