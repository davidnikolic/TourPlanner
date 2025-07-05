using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TourPlanner.UI.Interfaces.Coordinators;
using TourPlanner.UI.Interfaces;
using TourPlanner.UI.Services;
using TourPlanner.UI.ViewModels;
using TourPlanner.BL.Interfaces;
using TourPlanner.Logging.Interfaces;
using TourPlanner.Logging;

namespace TourPlanner.Tests
{
    public class NavBarViewModelTests
    {
        private Mock<IDialogService> _dialogServiceMock;
        private Mock<ITourCoordinatorService> _tourCoordinatorMock;
        private Mock<ITourExportCoordinator> _exportMock;
        private Mock<ITourImportCoordinator> _importMock;
        private TourNavBarViewModel _viewModel;
        ILoggerFactory loggerFactory;

        [SetUp]
        public void Setup()
        {
            _dialogServiceMock = new Mock<IDialogService>();
            _tourCoordinatorMock = new Mock<ITourCoordinatorService>();
            _exportMock = new Mock<ITourExportCoordinator>();
            _importMock = new Mock<ITourImportCoordinator>();
            loggerFactory = new LoggerFactory();

            _viewModel = new TourNavBarViewModel(
                _dialogServiceMock.Object,
                _tourCoordinatorMock.Object,
                _exportMock.Object,
                _importMock.Object,
                loggerFactory
            );
        }

        [Test]
        public void ImportFromJson_Should_CallDialogAndImportCoordinator()
        {
            // Arrange
            string testPath = "C:\\test.json";
            _dialogServiceMock.Setup(d => d.ShowOpenFileDialog(It.IsAny<string>())).Returns(testPath);

            // Act
            _viewModel.ImportFromJSONCommand.Execute(null);

            // Assert
            _importMock.Verify(i => i.ImportFromJson(testPath), Times.Once);
            _tourCoordinatorMock.Verify(c => c.RequestTourRefresh(), Times.Once);
        }

        [Test]
        public void ImportFromCsv_Should_NotCallImport_WhenDialogCanceled()
        {
            // Arrange
            _dialogServiceMock.Setup(d => d.ShowOpenFileDialog(It.IsAny<string>())).Returns((string)null);

            // Act
            _viewModel.ImportFromCsvCommand.Execute(null);

            // Assert
            _importMock.Verify(i => i.ImportFromCsv(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void AllToursJsonCommand_CallsExportWithPath()
        {
            string fakePath = "C:\\output.json";
            _dialogServiceMock.Setup(d => d.ShowSaveFileDialog(It.IsAny<string>(), It.IsAny<string>())).Returns(fakePath);

            _viewModel.AllToursJsonCommand.Execute(null);

            _exportMock.Verify(e => e.ExportAllToursAsJson(fakePath), Times.Once);
        }
    }
}
