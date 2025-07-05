using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using TourPlanner.Logging;
using TourPlanner.BL.DTOs;
using TourPlanner.UI.Interfaces;
using TourPlanner.UI.ViewModels;
using TourPlanner.UI.ViewModels.Components;
using TourPlanner.UI.Views.Components;

namespace TourPlanner.UI.Services
{
    internal class DialogService : IDialogService
    {
        private readonly ILoggerWrapper _logger;
        public DialogService(ILoggerWrapper logger)
        {
            _logger = logger;
        }
        public TourLogDTO? DisplayTourLogPopUp(string title, TourLogDTO tourLog = null)
        {
            _logger.Info($"Displaying TourLog popup with title: {title}");
            try
            {
                var fields = FormFieldFactory.CreateForTourLog(tourLog);
                var vm = new GenericDialogViewModel(title, fields, _logger);
                var dialog = new GenericDialogWindowView { DataContext = vm };
                vm.CloseRequested += () => dialog.DialogResult = true;

                if (dialog.ShowDialog() == true)
                {
                    var res = FormFieldFactory.ToTourLogDTO(fields);
                    _logger.Info("TourLog popup completed successfully");
                    return res;
                }

                _logger.Info("TourLog popup was cancelled");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Error displaying TourLog popup", ex);
                return null;
            }
        }

        public TourDTO? DisplayTourPopUp(string title, TourDTO tour = null)
        {
            _logger.Info($"Displaying Tour popup with title: {title}");

            try
            {
                var fields = FormFieldFactory.CreateForTour(tour);
                var vm = new GenericDialogViewModel(title, fields, _logger);
                var dialog = new GenericDialogWindowView { DataContext = vm };
                vm.CloseRequested += () => dialog.DialogResult = true;

                if (dialog.ShowDialog() == true)
                {
                    var res = FormFieldFactory.ToTourDTO(fields);
                    if (tour != null) res.RouteImagePath = tour.RouteImagePath;
                    _logger.Info("Tour popup completed successfully");
                    return res;
                }

                _logger.Info("Tour popup was cancelled");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Error displaying Tour popup", ex);
                return null;
            }
        }

        public void ShowMessage(string message)
        {
            _logger.Info($"Showing message to user: {message}");
            MessageBox.Show(message);
        }

        public string? ShowOpenFileDialog(string filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*")
        {
            _logger.Debug($"Opening file dialog with filter: {filter}");

            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = filter,
                    Title = "Datei auswählen"
                };
                bool? result = openFileDialog.ShowDialog();
                if (result == true)
                {
                    _logger.Info($"File selected: {openFileDialog.FileName}");
                    return openFileDialog.FileName;
                }

                _logger.Debug("File dialog cancelled");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Error showing open file dialog", ex);
                return null;
            }
        }

        public string? ShowFolderDialog()
        {
            _logger.Debug("Opening folder dialog");

            try
            {
                var dialog = new VistaFolderBrowserDialog
                {
                    Description = "Bitte Ordner zum Exportieren wählen",
                    UseDescriptionForTitle = true,
                    ShowNewFolderButton = true
                };

                if (dialog.ShowDialog() == true)
                {
                    _logger.Info($"Folder selected: {dialog.SelectedPath}");
                    return dialog.SelectedPath;
                }

                _logger.Debug("Folder dialog cancelled");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Error showing folder dialog", ex);
                return null;
            }
        }

        public string? ShowSaveFileDialog(string defaultFileName = "export.csv", string filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*")
        {
            _logger.Debug($"Opening save file dialog with default name: {defaultFileName}");

            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = filter,
                    FileName = defaultFileName,
                    Title = "Datei speichern"
                };
                bool? result = saveFileDialog.ShowDialog();

                if (result == true)
                {
                    _logger.Info($"Save file selected: {saveFileDialog.FileName}");
                    return saveFileDialog.FileName;
                }

                _logger.Debug("Save file dialog cancelled");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Error showing save file dialog", ex);
                return null;
            }
        }
    }
}
