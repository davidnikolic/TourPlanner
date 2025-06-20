using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.UI.ViewModels
{
    public class GenericDialogViewModel : ViewModelBase
    {
        public event Action? CloseRequested;

        public ObservableCollection<string> Namen { get; } = new()
        {
            "Alice", "Bob", "Charlie"
        };

        private void ConfirmActivity()
        {
            // Optional: Validierung hier
            CloseRequested?.Invoke();
        }
    }
}
