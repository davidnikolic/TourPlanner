﻿using Microsoft.Extensions.DependencyInjection;
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
using TourPlanner.Logging.Interfaces;
using TourPlanner.Logging;
using TourPlanner.BL.Services.Map;
using TourPlanner.UI.Interfaces.Coordinators;
using TourPlanner.UI.Services.Coordinators;

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

            // DB test
            using (var scope = Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TourPlannerDBContext>();
                if (!dbContext.Database.CanConnect())
                {
                    MessageBox.Show(
                        "The connection to the database could not be established.\n" +
                         "Please check your configuration and try again.",
                         "Connection error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );

                    Shutdown(); // Shutdown the application if the database connection fails
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
            services.AddSingleton<ITourStatisticsService, TourStatisticsService>();
            services.AddSingleton<IImportService, ImportService>();
            services.AddSingleton<IExportService, ExportService>();
            services.AddSingleton<ISearchService, SearchService>();
            services.AddSingleton<IUITabNavigationService, UITabNavigationService>();

            // Coordinators
            services.AddSingleton<ITourCoordinatorService, TourCoordinatorService>();
            services.AddSingleton<ITourExportCoordinator, TourExportCoordinator>();
            services.AddSingleton<ITourImportCoordinator, TourImportCoordinator>();

            // Logging
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddTransient<ILoggerWrapper>(provider =>
            {
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                return loggerFactory.CreateLogger<object>();
            });
            // Repositories (DAL)
            services.AddScoped<ITourRepository, TourRepositories>();
            services.AddScoped<ITourLogRepository, TourLogRepository>();

            // UI Services
            services.AddSingleton<IMapViewService, MapViewService>();

            // BL
            services.AddSingleton<IMapService, MapService>();

            // ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<TourNavBarViewModel>();
            services.AddSingleton<TourListViewModel>();
            services.AddSingleton<TourLogsViewModel>();
            services.AddSingleton<TourDetailViewModel>();

        }
    }

}
