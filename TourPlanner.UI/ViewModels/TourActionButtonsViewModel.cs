using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.UI.Views.Components;

namespace TourPlanner.UI.ViewModels
{
    public class TourActionButtonsViewModel
    {
        public ICommand AddTourCommand { get; set; }

        public TourActionButtonsViewModel()
        {
            AddTourCommand = new RelayCommand(OpenAddTourWindow);
        }

        private void OpenAddTourWindow()
        {
            var dialog = new AddTourDialogView();
            dialog.ShowDialog(); 
        }
    }
}
