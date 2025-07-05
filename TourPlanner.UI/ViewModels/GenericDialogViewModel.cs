using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Sprache;
using TourPlanner.Logging;
using TourPlanner.BL.DTOs;
using TourPlanner.UI.ViewModels.Components;
using static TourPlanner.BL.DTOs.EnumsDTO;

namespace TourPlanner.UI.ViewModels
{
    public class GenericDialogViewModel : ViewModelBase
    {
        private readonly ILoggerWrapper _logger;
        public string PopUpText { get; private set; } = "Sample Text";

        public event Action? CloseRequested;

        public ICommand ConfirmCommand { get; }

        public ObservableCollection<FormField> Fields { get; }

        public GenericDialogViewModel(string title, ObservableCollection<FormField> fields, ILoggerWrapper logger)
        {
            _logger = logger;
            PopUpText = title;
            Fields = fields;
            ConfirmCommand = new RelayCommand(_ => ConfirmActivity());
            _logger.Info($"GenericDialogViewModel initialized with title: {title}");
        }

        private bool ValidateFields()
        {
            _logger.Debug("Starting field validation");
            foreach (var field in Fields)
            {
                field.Error = null;

                if (field.Type == "TextBox" && field.Value == String.Empty)
                {
                    field.Error = "This line must not be empty.";
                    _logger.Debug($"Validation failed for field {field.Label}: Empty value");
                }

                if (field.Type == "TextBox" && (field.Label.Contains("Distance") || field.Label.Contains("Duration")))
                {
                    if (!float.TryParse(field.Value?.ToString(), out _))
                    {
                        field.Error = "Please enter a valid number.";
                        _logger.Debug($"Validation failed for field {field.Label}: Invalid number format");
                    }
                }

                if (field.Type == "DatePicker" && field.Value is not DateTime)
                {
                    field.Error = "Please select a date.";
                    _logger.Debug($"Validation failed for field {field.Label}: Invalid date");
                }
            }
            bool isValid = Fields.All(f => f.IsValid);
            _logger.Info($"Field validation completed. Valid: {isValid}");
            return Fields.All(f => f.IsValid);
        }

        private void ConfirmActivity()
        {
            _logger.Info("Confirm activity initiated");

            try
            {
                if (!ValidateFields())
                {
                    _logger.Info("Validation failed, showing error message to user");
                    MessageBox.Show("Please fill out all fields correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _logger.Info("Validation successful, closing dialog");
                CloseRequested?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.Error("Error during confirm activity", ex);
                MessageBox.Show("An unexpected error occurred.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
