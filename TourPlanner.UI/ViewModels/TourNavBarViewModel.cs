using TourPlanner.BL.Interfaces;
using TourPlanner.UI.Interfaces;
using TourPlanner.UI.Interfaces.Coordinators;

namespace TourPlanner.UI.ViewModels
{
    public class TourNavBarViewModel : ViewModelBase
    {
        private IDialogService _dialogService;

        private ITourCoordinatorService _tourCoordinatorService;

        private ITourExportCoordinator _tourExportCoordinator;

        private ITourImportCoordinator _tourImportCoordinator;

        public TourNavBarViewModel
            (
            IDialogService dialogService,
            ITourCoordinatorService tourCoordinatorService,
            ITourExportCoordinator tourExportCoordinator,
            ITourImportCoordinator tourImportCoordinator
            ) 
        { 
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

        public RelayCommand ExitCommand => new RelayCommand(execute => Environment.Exit(0));

        private void ImportFromCsv()
        {
            var path = _dialogService.ShowOpenFileDialog("CSV Files (*.csv)|*.csv|All Files (*.*)|*.*");

            if (path != null)
            {
                _tourImportCoordinator.ImportFromCsv(path);
                _tourCoordinatorService.RequestTourRefresh();
            }
        }

        private void ImportFromJson()
        {
            var path = _dialogService.ShowOpenFileDialog("JSON Files (*.json)|*.json|All Files (*.*)|*.*");

            if (path != null)
            {
                _tourImportCoordinator.ImportFromJson(path);
                _tourCoordinatorService.RequestTourRefresh();
            }
        }

        private void ExportAllToursAsPdf()
        {
            var path = _dialogService.ShowSaveFileDialog("output.pdf", "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*");
            if (path != null) _tourExportCoordinator.ExportAllToursAsPdf(path);
        }

        private void ExportAllToursAsCsv()
        {
            var folderPath = _dialogService.ShowFolderDialog();
            if (folderPath != null) _tourExportCoordinator.ExportAllToursAsCsv(folderPath); 
        }

        private void ExportAllToursAsJson()
        {
            var path = _dialogService.ShowSaveFileDialog("output.json", "JSON Files (*.json)|*.json|All Files (*.*)|*.*");
            if (path != null) _tourExportCoordinator.ExportAllToursAsJson(path);
        }
    }
}
