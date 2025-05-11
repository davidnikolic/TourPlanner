using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using TourPlanner.BL.Interfaces;
using TourPlanner.BL.Services;
using TourPlanner.DAL.Repositories.Interfaces;
using TourPlanner.DAL;
using TourPlanner.UI.ViewModels;
using TourPlanner.DAL.Repositories;

namespace TourPlanner.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// DI configuration
    /// </summary>
    public partial class App : Application
    {
        // Allows global acces to the custom app instance
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

            InitializeComponent();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // DBContext
            services.AddDbContext<TourPlannerDBContext>();

            // Repositories
            services.AddScoped<ITourRepository, TourRepositories>();

            // Services
            services.AddScoped<ITourService, TourService>();

            services.AddSingleton<TourListViewModel>();
            // AddTourViewModels, all are automatically instantiated
            services.AddTransient<AddTourViewModel>();

        }
    }

}
