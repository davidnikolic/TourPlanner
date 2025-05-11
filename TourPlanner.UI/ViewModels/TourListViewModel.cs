using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.UI.ViewModels
{
    public class TourListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Tours { get; } = new ObservableCollection<string>();

        public void AddTourName(string tourName)
        {
            if (!string.IsNullOrWhiteSpace(tourName))
                Tours.Add(tourName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
