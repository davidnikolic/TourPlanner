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
            new() { Label = "Datum", Type = "DatePicker", Value = dto?.LogDate ?? DateTime.Now },
            new() { Label = "Kommentar", Type = "TextBox", Value = dto?.Comment ?? "" },
            new() { Label = "Schwierigkeit", Type = "ComboBox", Value = dto?.Difficulty ?? DifficultyLevel.medium, Options = Enum.GetValues(typeof(DifficultyLevel)).Cast<object>() },
            new() { Label = "Distanz (in km)", Type = "TextBox", Value = dto?.DistanceKm ?? 0 },
            new() { Label = "Dauer (in h)", Type = "TextBox", Value = dto?.DurationHours ?? 0 },
            new() { Label = "Bewertung", Type = "ComboBox", Value = dto?.Rating ?? SatisfactionRating.neutral, Options = Enum.GetValues(typeof(SatisfactionRating)).Cast<object>() }
        };

            return fields;
        }

        public static ObservableCollection<FormField> CreateForTour(TourDTO? dto)
        {
            return new ObservableCollection<FormField>
            {
                new() { Label = "Name", Type = "TextBox", Value = dto?.Name ?? "" },
                new() { Label = "Beschreibung", Type = "TextBox", Value = dto?.Description ?? "" },
                new() { Label = "Startort", Type = "TextBox", Value = dto?.StartLocation ?? "" },
                new() { Label = "Zielort", Type = "TextBox", Value = dto?.EndLocation ?? "" },
                new() { Label = "Transportmittel", Type = "ComboBox", Value = dto?.TransportType ?? TransportType.foot, Options = Enum.GetValues(typeof(TransportType)).Cast<object>() },
                new() { Label = "Distanz (in km)", Type = "TextBox", Value = dto?.DistanceKm ?? 0f },
                new() { Label = "Dauer (in h)", Type = "TextBox", Value = dto?.EstimatedTimeHours ?? 0f }
            };
        }

        public static TourLogDTO ToTourLogDTO(ObservableCollection<FormField> fields)
        {
            var dict = fields.ToDictionary(f => f.Label, f => f.Value);

            return new TourLogDTO
            {
                LogDate = (DateTime)(dict["Datum"] ?? DateTime.Today),
                Comment = dict["Kommentar"]?.ToString() ?? "",
                Difficulty = (DifficultyLevel)(dict["Schwierigkeit"] ?? DifficultyLevel.medium),
                DistanceKm = float.TryParse(dict["Distanz (in km)"]?.ToString(), out var km) ? km : 0f,
                DurationHours = float.TryParse(dict["Dauer (in h)"]?.ToString(), out var h) ? h : 0f,
                Rating = (SatisfactionRating)(dict["Bewertung"] ?? SatisfactionRating.neutral)
            };
        }

        public static TourDTO ToTourDTO(ObservableCollection<FormField> fields)
        {
            var dict = fields.ToDictionary(f => f.Label, f => f.Value);

            return new TourDTO
            {
                Name = dict["Name"]?.ToString() ?? "",
                Description = dict["Beschreibung"]?.ToString() ?? "",
                StartLocation = dict["Startort"]?.ToString() ?? "",
                EndLocation = dict["Zielort"]?.ToString() ?? "",
                TransportType = (TransportType)(dict["Transportmittel"] ?? TransportType.foot),
                DistanceKm = float.TryParse(dict["Distanz (in km)"]?.ToString(), out var km) ? km : 0f,
                EstimatedTimeHours = float.TryParse(dict["Dauer (in h)"]?.ToString(), out var hours) ? hours : 0f
            };
        }
    }
}
