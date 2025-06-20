using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Sprache;
using TourPlanner.BL.DTOs;
using TourPlanner.UI.ViewModels.Components;
using static TourPlanner.BL.DTOs.EnumsDTO;

namespace TourPlanner.UI.ViewModels
{
    public class GenericDialogViewModel : ViewModelBase
    {
        public string PopUpText { get; private set; } = "Sample Text";

        public event Action? CloseRequested;

        public ICommand ConfirmCommand { get; }

        public ObservableCollection<FormField> Fields { get; }

        public GenericDialogViewModel(string title, ObservableCollection<FormField> fields)
        {
            PopUpText = title;
            Fields = fields;
            ConfirmCommand = new RelayCommand(_ => ConfirmActivity());
        }

        private bool ValidateFields()
        {
            foreach (var field in Fields)
            {
                field.Error = null;

                if (field.Type == "TextBox" && field.Value == String.Empty)
                {
                    field.Error = "Diese Zeile darf nicht leer sein.";
                }

                if (field.Type == "TextBox" && (field.Label.Contains("Distanz") || field.Label.Contains("Dauer")))
                {
                    if (!float.TryParse(field.Value?.ToString(), out _))
                        field.Error = "Bitte eine gültige Zahl eingeben.";
                }

                if (field.Type == "DatePicker" && field.Value is not DateTime)
                {
                    field.Error = "Bitte ein Datum auswählen.";
                }
            }
            return Fields.All(f => f.IsValid);
        }

        private void ConfirmActivity()
        {
            if (!ValidateFields()) {
                MessageBox.Show("Please fill out all fields correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CloseRequested?.Invoke();
        }
    }
}
