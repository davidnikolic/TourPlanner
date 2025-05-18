using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TourPlanner.BL.DTOs;
using static TourPlanner.BL.DTOs.EnumsDTO;

namespace TourPlanner.UI.ViewModels
{
    public class TourLogDialogViewModel : ViewModelBase
    {
        public event Action? CloseRequested;

        public string PopUpText { get; private set; } = "";

        public TourLogDTO Result { get; private set; } = new();

        public ICommand ConfirmCommand { get; }

        // Eingabefelder:
        public DateTime LogDate
        {
            get => Result.LogDate;
            set { Result.LogDate = value; OnPropertyChanged(); }
        }

        public string Comment
        {
            get => Result.Comment ?? "";
            set { Result.Comment = value; OnPropertyChanged(); }
        }

        public DifficultyLevel SelectedDifficulty
        {
            get => Result.Difficulty;
            set { Result.Difficulty = value; OnPropertyChanged(); }
        }

        public float DistanceKm
        {
            get => Result.DistanceKm;
            set { Result.DistanceKm = value; OnPropertyChanged(); }
        }

        public float DurationHours
        {
            get => Result.DurationHours;
            set { Result.DurationHours = value; OnPropertyChanged(); }
        }

        public SatisfactionRating SelectedRating
        {
            get => Result.Rating;
            set { Result.Rating = value; OnPropertyChanged(); }
        }

        // Dropdown-Listen:
        public List<DifficultyLevel> DifficultyLevels { get; set; } = Enum.GetValues(typeof(DifficultyLevel)).Cast<DifficultyLevel>().ToList();
        public List<SatisfactionRating> SatisfactionRatings { get; set; } = Enum.GetValues(typeof(SatisfactionRating)).Cast<SatisfactionRating>().ToList();

        public TourLogDialogViewModel(string text, TourLogDTO? existingTourLog = null)
        {
            PopUpText = text;

            ConfirmCommand = new RelayCommand(_ => ConfirmActivity());

            if (existingTourLog != null)
            {
                Result = existingTourLog;
            }
        }

        private void ConfirmActivity()
        {
            // Optional: Validierung hier
            CloseRequested?.Invoke();
        }
    }
}
