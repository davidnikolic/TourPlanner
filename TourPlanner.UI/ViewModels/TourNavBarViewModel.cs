using TourPlanner.BL.Interfaces;
using TourPlanner.UI.Interfaces;
using TourPlanner.UI.Interfaces.Coordinators;
using TourPlanner.Logging;
using TourPlanner.Logging.Interfaces;

namespace TourPlanner.UI.ViewModels
{
    public class TourNavBarViewModel : ViewModelBase
    {
        private readonly ILoggerWrapper _logger;
        private IDialogService _dialogService;

        private ITourCoordinatorService _tourCoordinatorService;

        private ITourExportCoordinator _tourExportCoordinator;

        private ITourImportCoordinator _tourImportCoordinator;

        public TourNavBarViewModel
            (
            IDialogService dialogService,
            ITourCoordinatorService tourCoordinatorService,
            ITourExportCoordinator tourExportCoordinator,
            ITourImportCoordinator tourImportCoordinator,
            ILoggerFactory loggerFactory
            ) 
        {
            _logger = loggerFactory.CreateLogger<TourNavBarViewModel>();
            _logger.Info("TourNavBarViewModel initialized");
            _dialogService = dialogService;
            _tourCoordinatorService = tourCoordinatorService;
            _tourExportCoordinator = tourExportCoordinator;
            _tourImportCoordinator = tourImportCoordinator;
        }


        public RelayCommand AddTourCommand => new RelayCommand(execute => _tourCoordinatorService.AddTour());
        
        public RelayCommand ImportFromCsvCommand => new RelayCommand(execute => ImportFromCsv());
        
        public RelayCommand ImportFromJSONCommand => new RelayCommand(execute => ImportFromJson());
        
        public RelayCommand SelectedTourPdfCommand => new RelayCommand(execute => _tourExportCoordinator.ExportSelectedTourAsPdf());
        
        public RelayCommand SelectedTourCsvCommand => new RelayCommand(execute => _tourExportCoordinator.ExportSelectedTourAsCsv());
        
        public RelayCommand SelectedTourJsonCommand => new RelayCommand(execute => _tourExportCoordinator.ExportSelectedTourAsJson());
        
        public RelayCommand AllToursPdfCommand => new RelayCommand(execute => ExportAllToursAsPdf());
        
        public RelayCommand AllToursCsvCommand => new RelayCommand(execute => ExportAllToursAsCsv());
        
        public RelayCommand AllToursJsonCommand => new RelayCommand(execute => ExportAllToursAsJson());
        
        public RelayCommand SummarizeReportCommand => new RelayCommand(execute => _tourExportCoordinator.ExportSummarizeReport());
        public RelayCommand AboutCommand => new RelayCommand(execute => About());

        public RelayCommand ExitCommand => new RelayCommand(execute => Environment.Exit(0));

        private void ImportFromCsv()
        {
            _logger.Info("Starting CSV import");
            var path = _dialogService.ShowOpenFileDialog("CSV Files (*.csv)|*.csv|All Files (*.*)|*.*");

            if (path != null)
            {
                _logger.Debug("CSV import cancelled by user");
                _tourImportCoordinator.ImportFromCsv(path);
                _tourCoordinatorService.RequestTourRefresh();
            }
        }

        private void ImportFromJson()
        {
            _logger.Info("Starting JSON import");
            var path = _dialogService.ShowOpenFileDialog("JSON Files (*.json)|*.json|All Files (*.*)|*.*");

            if (path != null)
            {
                _tourImportCoordinator.ImportFromJson(path);
                _tourCoordinatorService.RequestTourRefresh();
            }
        }

        private void ExportAllToursAsPdf()
        {
            _logger.Info("Starting export all tours as PDF");
            var path = _dialogService.ShowSaveFileDialog("output.pdf", "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*");
            if (path != null) _tourExportCoordinator.ExportAllToursAsPdf(path);
        }

        private void ExportAllToursAsCsv()
        {
            _logger.Info("Starting export all tours as CSV");
            var folderPath = _dialogService.ShowFolderDialog();
            if (folderPath != null) _tourExportCoordinator.ExportAllToursAsCsv(folderPath); 
        }

        private void ExportAllToursAsJson()
        {
            _logger.Info("Starting export all tours as JSON");
            var path = _dialogService.ShowSaveFileDialog("output.json", "JSON Files (*.json)|*.json|All Files (*.*)|*.*");
            if (path != null) _tourExportCoordinator.ExportAllToursAsJson(path);
        }

        private void About()
        {
            _logger.Info("About information loaded");
            _dialogService.ShowMessage("TourPlanner ist eine plattformübergreifende Desktop-Anwendung zur Planung, Verwaltung und Analyse von Touren aller Art – egal ob Wanderung, Radtour, Laufstrecke oder Urlaubsreise.\r\n\r\nDie Anwendung ermöglicht es Nutzer:innen, Touren anzulegen, mit Beschreibungen und automatischen Routing-Daten zu versehen, und zu jeder Tour detaillierte Logs zu erfassen.\r\nDank integrierter OpenRouteService-API, Wetterintegration, volltextbasierter Suche, PDF-Reports, CSV-/JSON-Import & Export und eines responsiven Designs bietet TourPlanner sowohl funktionale Tiefe als auch intuitive Bedienung.\r\n\r\nDie App wurde im Rahmen eines Hochschulprojekts mit Fokus auf sauberer Architektur (MVVM), testbarer Logik und modularer Erweiterbarkeit entwickelt.\r\n\r\nBesonderheiten:\r\n\r\n    Übersichtliche Benutzeroberfläche mit auf- und zuklappbarer Tourliste\r\n\r\n    Wiederverwendbare Komponenten wie ActionButtons und PopUps\r\n\r\n    Dynamische Darstellung je nach Fenstergröße\r\n\r\n    Einzigartiges View-Toggling für individuell anpassbare Arbeitsbereiche\r\n\r\nEntwickelt von:\r\nDavid & Alex – FH-Projekt 2025");
        }
    }
}
