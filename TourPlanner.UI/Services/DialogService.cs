using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.UI.Interfaces;
using TourPlanner.UI.ViewModels;
using TourPlanner.UI.ViewModels.Components;
using TourPlanner.UI.Views.Components;

namespace TourPlanner.UI.Services
{
    internal class DialogService : IDialogService
    {
        public TourLogDTO? DisplayTourLogPopUp(string title, TourLogDTO tourLog = null)
        {
            var fields = FormFieldFactory.CreateForTourLog(tourLog);
            var vm = new GenericDialogViewModel(title, fields);

            var dialog = new GenericDialogWindowView { DataContext = vm };
            vm.CloseRequested += () => dialog.DialogResult = true;

            if (dialog.ShowDialog() == true)
            {
                var res = FormFieldFactory.ToTourLogDTO(fields);

                return res;

            }

            return null;
        }

        public TourDTO? DisplayTourPopUp(string title, TourDTO tour = null)
        {
            var dialog = new TourDialogWindowView();
            var vm = new TourDialogViewModel(title, tour);

            dialog.DataContext = vm;

            vm.CloseRequested += () => dialog.DialogResult = true;

            if (dialog.ShowDialog() == true)
            {
                return vm.Result;
            }

            return null;
        }
    }
}
