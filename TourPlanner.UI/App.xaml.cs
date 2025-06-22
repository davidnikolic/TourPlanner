using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using TourPlanner.BL.Interfaces;
using TourPlanner.UI.Interfaces;
using TourPlanner.BL.Services;
using TourPlanner.DAL.Repositories.Interfaces;
using TourPlanner.DAL;
using TourPlanner.UI.ViewModels;
using TourPlanner.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Query.Internal;
using GalaSoft.MvvmLight.Views;
using TourPlanner.UI.Services;
using TourPlanner.UI.Map.Interface;
using TourPlanner.UI.Map.Services;

namespace TourPlanner.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// Stores the DI configuration for the project.
    /// </summary>
    public partial class App : Application
    {
        // Allows global access to the custom app instance
        public new static App Current => (App)Application.Current!;

        // Stores the application's dependency injection service provider
        public IServiceProvider Services { get; }

        public App()
        {
            // Create a new service collection to register dependencies
            var services = new ServiceCollection();

            // Register services, repositories, and view models
            ConfigureServices(services);

            Services = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // DB-Verbindung testen
            using (var scope = Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TourPlannerDBContext>();
                if (!dbContext.Database.CanConnect())
                {
                    MessageBox.Show(
                        "Die Verbindung zur Datenbank konnte nicht hergestellt werden.\n" +
                        "Bitte überprüfen Sie Ihre Konfiguration und versuchen Sie es erneut.",
                        "Verbindungsfehler",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );

                    Shutdown(); // Beendet die Anwendung sauber
                    return;
                }
            }

            var mainWindowViewModel = Services.GetRequiredService<MainWindowViewModel>();

            var mainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };

            mainWindow.Show();
        }

        /// <summary>
        /// The method that configures the services of the project
        /// </summary>
        /// <param name="services">The ServiceCollection as IServiceCollection</param>
        private void ConfigureServices(IServiceCollection services)
        {
            // DBContext
            services.AddDbContext<TourPlannerDBContext>();

            // Services
            services.AddScoped<ITourService, TourService>();
            services.AddScoped<ITourLogService, TourLogService>();
            services.AddSingleton<ISelectedTourService, SelectedTourService>();
            services.AddSingleton<Interfaces.IDialogService, DialogService>();
            services.AddSingleton<IReportService, ReportService>();

            // Repositories (DAL)
            services.AddScoped<ITourRepository, TourRepositories>();
            services.AddScoped<ITourLogRepository, TourLogRepository>();

            // ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<TourNavBarViewModel>();
            services.AddSingleton<TourListViewModel>();
            services.AddSingleton<TourLogsViewModel>();
            services.AddSingleton<TourDetailViewModel>();

            // UI Services
            services.AddSingleton<IMapViewService, MapViewService>();
        }
    }

}
