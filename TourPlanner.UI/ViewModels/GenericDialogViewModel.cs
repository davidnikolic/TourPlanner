using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.UI.ViewModels.Components;

namespace TourPlanner.UI.ViewModels
{
    public class GenericDialogViewModel : ViewModelBase
    {
        public event Action? CloseRequested;

        public ObservableCollection<FormField> Fields { get; } = new()
        {
            new() { Label = "Name", Type = "TextBox", Value = "David"},
            new() { Label = "Typ", Type = "ComboBox", Value = "Wandern", Options = new[] { "Wandern", "Rad", "Auto" } }
        };

        private void ConfirmActivity()
        {
            // Optional: Validierung hier
            CloseRequested?.Invoke();
        }
    }
}
