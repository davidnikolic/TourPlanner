using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TourPlanner.UI.ViewModels
{
    public class TourNavBarViewModel : ViewModelBase
    {
        public RelayCommand FileCommand => new RelayCommand(execute => FileAction());

        private void FileAction()
        {
            MessageBox.Show("Das hat soweit geklappt", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
