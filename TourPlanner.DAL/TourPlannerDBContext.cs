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
            var envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env");
            Env.Load(envPath);

            string host = Env.GetString("DB_HOST");
            string port = Env.GetString("DB_PORT");
            string dbName = Env.GetString("DB_NAME");
            string user = Env.GetString("DB_USER");
            string pass = Env.GetString("DB_PASSWORD");

            string connectionString = $"Host={host};Port={port};Database={dbName};Username={user};Password={pass}";

            // Explicitly map the enum to a PostgreSQL enum type, ensures that them enum is handled correctly
            optionsBuilder.UseNpgsql(connectionString, options =>
            {
                options.MapEnum<TransportType>("transport_type");
                options.MapEnum<DifficultyLevel>("difficulty_level");
                options.MapEnum<SatisfactionRating>("satisfaction_rating");
            });

            base.OnConfiguring(optionsBuilder);
        }

        // Configure model entities to the db
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map Table Tour
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

            // Map Table Tour_Log
            modelBuilder.Entity<TourLogEntity>(entity =>
            {
                entity.ToTable("tour_logs");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TourId).HasColumnName("tour_id");
                entity.Property(e => e.LogDate).HasColumnName("log_date").HasColumnType("timestamp without time zone");
                entity.Property(e => e.Comment).HasColumnName("comment");
                entity.Property(e => e.Difficulty).HasColumnName("difficulty");
                entity.Property(e => e.DistanceKm).HasColumnName("distance_km");
                entity.Property(e => e.DurationHours).HasColumnName("duration_hours");
                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.HasOne(e => e.Tour)               
                      .WithMany(t => t.TourLogs)         // Tour can have many tourlogs
                      .HasForeignKey(e => e.TourId)  
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}