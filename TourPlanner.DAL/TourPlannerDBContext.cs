using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TourPlanner.DAL.Entities;
using static TourPlanner.DAL.Entities.Enums;
using Npgsql;
using DotNetEnv;

namespace TourPlanner.DAL
{
    public class TourPlannerDBContext : DbContext
    {
        public DbSet<TourEntity> Tours { get; set; }
        public DbSet<TourLogEntity> TourLogs { get; set; }

        public TourPlannerDBContext(DbContextOptions<TourPlannerDBContext> options)
            : base(options)
        {
        }

        // Configure db connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Env.Load();

            var connectionString = $"Host={Env.GetString("DB_HOST")};" +
                                   $"Port={Env.GetString("DB_PORT")};" +
                                   $"Database={Env.GetString("DB_NAME")};" +
                                   $"Username={Env.GetString("DB_USER")};" +
                                   $"Password={Env.GetString("DB_PASSWORD")}";
            // PostgreSQL connection
            optionsBuilder.UseNpgsql(connectionString);

            // Explicitly map the enum to a PostgreSQL enum type, ensures that them enum is handled correctly
            optionsBuilder.UseNpgsql(options =>
            {
                options.MapEnum<TransportType>("transport_type");
            });

            base.OnConfiguring(optionsBuilder);
        }

        // Configure model entities to the db
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map 
            modelBuilder.Entity<TourEntity>(entity =>
            {
                entity.ToTable("tours");
                entity.Property(t => t.Id).HasColumnName("id");
                entity.Property(t => t.Name).HasColumnName("name");
                entity.Property(t => t.Description).HasColumnName("description");
                entity.Property(t => t.StartLocation).HasColumnName("start_location");
                entity.Property(t => t.EndLocation).HasColumnName("end_location");
                entity.Property(t => t.TransportType).HasColumnName("transport_type");
                entity.Property(t => t.DistanceKm).HasColumnName("distance_km");
                entity.Property(t => t.EstimatedTimeHours).HasColumnName("estimated_time_hours");
                entity.Property(t => t.RouteImagePath).HasColumnName("route_image_path");
            });
        }
    }
}