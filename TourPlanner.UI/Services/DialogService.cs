using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
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
            var fields = FormFieldFactory.CreateForTour(tour);
            var vm = new GenericDialogViewModel(title, fields);

            var dialog = new GenericDialogWindowView { DataContext = vm };
            vm.CloseRequested += () => dialog.DialogResult = true;

            if (dialog.ShowDialog() == true)
            {
                var res = FormFieldFactory.ToTourDTO(fields);
                if (tour != null) res.RouteImagePath = tour.RouteImagePath;
                return res;

            }

            return null;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public string? ShowOpenFileDialog(string filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*")
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = filter,
                Title = "Datei auswählen"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        public string? ShowFolderDialog()
        {
            var dialog = new VistaFolderBrowserDialog
            {
                Description = "Bitte Ordner zum Exportieren wählen",
                UseDescriptionForTitle = true,
                ShowNewFolderButton = true
            };

            return dialog.ShowDialog() == true ? dialog.SelectedPath : null;
        }

        public string? ShowSaveFileDialog(string defaultFileName = "export.csv", string filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*")
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = filter,
                FileName = defaultFileName,
                Title = "Datei speichern"
            };

            bool? result = saveFileDialog.ShowDialog();

            return result == true ? saveFileDialog.FileName : null;
        }
    }
}
