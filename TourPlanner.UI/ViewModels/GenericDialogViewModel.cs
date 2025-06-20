using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.UI.ViewModels.Components;
using static TourPlanner.BL.DTOs.EnumsDTO;

namespace TourPlanner.UI.ViewModels
{
    public class GenericDialogViewModel : ViewModelBase
    {
        public string PopUpText { get; private set; } = "Sample Text";

        public event Action? CloseRequested;

        public ObservableCollection<FormField> Fields { get; } = new()
        {
            new() { Label = "Datum", Type = "DatePicker"},
            new() { Label = "Kommentar", Type = "TextBox"},
            new() { Label = "Schwierigkeit", Type = "ComboBox", Options = Enum.GetValues(typeof(DifficultyLevel)).Cast<object>()},
            new() { Label = "Distanz (in km)", Type = "TextBox"},
            new() { Label = "Dauer (in h)", Type = "TextBox"},
            new() { Label = "Bewertung", Type = "ComboBox", Options = Enum.GetValues(typeof(SatisfactionRating)).Cast<object>()},
            //new() { Label = "Typ", Type = "ComboBox", Value = "Wandern", Options = new[] { "Wandern", "Rad", "Auto" } }
        };

        private void ConfirmActivity()
        {
            // Optional: Validierung hier
            CloseRequested?.Invoke();
        }
    }
}
