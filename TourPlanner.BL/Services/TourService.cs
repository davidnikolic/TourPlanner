using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;
using TourPlanner.DAL.Entities;
using TourPlanner.DAL.Repositories.Interfaces;
using TourPlanner.DAL.Repositories;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Text.RegularExpressions;
using iText.Svg.Renderers.Path.Impl;

namespace TourPlanner.BL.Services
{
    public class TourService : ITourService
    {
        public ITourRepository _tourRepository;
        private ITourLogService _tourLogService;

        public TourService(ITourRepository TourRepo, ITourLogService tourLogService)
        {
            _tourRepository = TourRepo;
            _tourLogService = tourLogService;
        }

        public TourService(ITourRepository TourRepo)
        {
            _tourRepository = TourRepo;
        }

        public TourDTO AddTour(TourDTO tour)
        {
           TourEntity entity = ToEntity(tour);
           var tourEntity = _tourRepository.AddTour(entity);
           return ToModel(tourEntity);
        }

        public List<TourDTO> GetTours()
        {
            var entities = (_tourRepository.GetTours()).ToList();

            List<TourDTO> tours = entities
                .Select(entity => ToModel(entity))
                .ToList();

            return tours;
        }

        public void UpdateTourMapImagePath(int tourId, string imagePath)
        {
            var entity = _tourRepository.GetTourById(tourId);
            if (entity == null) return;

            entity.RouteImagePath = imagePath;
            _tourRepository.UpdateTour(entity);
        }
        public int GetLastTourId()
        {
            return _tourRepository.GetLastTourId();
        }

        public void UpdateTour(TourDTO tour)
        {
            TourEntity entity = ToEntity(tour);
            _tourRepository.UpdateTour(entity);
        }

        public void DeleteTour(TourDTO tour)
        {
            TourEntity entity = ToEntity(tour);
            _tourRepository.DeleteTour(entity.Id);

            try
            {
                if (File.Exists(tour.RouteImagePath))
                {
                    File.Delete(tour.RouteImagePath);
                }
            }
            catch (IOException ex)
            {
                //LOG
            }
        }

        public static TourDTO ToModel(TourEntity entity)
        {
            if (entity == null)
                return null;

            return new TourDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                StartLocation = entity.StartLocation,
                EndLocation = entity.EndLocation,
                Description = entity.Description,
                TransportType = (EnumsDTO.TransportType)entity.TransportType,
                DistanceKm = entity.DistanceKm,
                EstimatedTimeHours = entity.EstimatedTimeHours,
                RouteImagePath = entity.RouteImagePath
            };
        }

        public static TourEntity ToEntity(TourDTO model)
        {
            if (model == null)
                return null;

            return new TourEntity
            {
                Id = model.Id,
                Name = model.Name,
                StartLocation = model.StartLocation,
                EndLocation = model.EndLocation,
                Description = model.Description,
                TransportType = (Enums.TransportType)model.TransportType,
                DistanceKm = model.DistanceKm,
                EstimatedTimeHours = model.EstimatedTimeHours,
                RouteImagePath = model.RouteImagePath
            };
        }

        public List<TourDTO> SearchTours(string searchTerm)
        {
            // Get all tours
            var allTourEntities = _tourRepository.GetTours().ToList();

            // If search term is null or empty, return all tours as DTOs
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return allTourEntities.Select(ToModel).ToList();
            }

            var lowerSearchTerm = searchTerm.ToLower().Trim();

            // Handle special search prefixes - delegate to TourLogService
            if (lowerSearchTerm.StartsWith("popularity:") ||
                lowerSearchTerm.StartsWith("childfriendliness:") ||
                lowerSearchTerm.StartsWith("distance:") ||
                lowerSearchTerm.StartsWith("duration:"))
            {
                // Use existing TourLogService logic to find matching logs
                var matchingLogs = _tourLogService.SearchTourLogs(searchTerm);
                var tourIds = matchingLogs.Select(log => log.TourId).Distinct();

                // Return tours that have matching logs
                return allTourEntities.Where(t => tourIds.Contains(t.Id)).Select(ToModel).ToList();
            }

            // Avoid duplicates int
            var matchingTourIds = new HashSet<int>();

            // First, find tours that match directly in their own fields
            var directlyMatchingTours = allTourEntities.Where(t =>
            {
                if ((t.Name != null && t.Name.ToLower().Contains(lowerSearchTerm)) ||
                    (t.Description != null && t.Description.ToLower().Contains(lowerSearchTerm)) ||
                    (t.StartLocation != null && t.StartLocation.ToLower().Contains(lowerSearchTerm)) ||
                    (t.EndLocation != null && t.EndLocation.ToLower().Contains(lowerSearchTerm)) ||
                    t.TransportType.ToString().ToLower().Contains(lowerSearchTerm) ||
                    t.DistanceKm.ToString(CultureInfo.InvariantCulture).ToLower().Contains(lowerSearchTerm) ||
                    t.EstimatedTimeHours.ToString(CultureInfo.InvariantCulture).ToLower().Contains(lowerSearchTerm))
                {
                    return true;
                }
                return false;
            });

            // Add directly matching tour IDs
            foreach (var tour in directlyMatchingTours)
            {
                matchingTourIds.Add(tour.Id);
            }

            // Now find tours that have matching tour logs
            var allTourLogs = _tourRepository.GetAllTourLogs();
            var toursWithMatchingLogs = allTourLogs.Where(log =>
                (log.Comment != null && log.Comment.ToLower().Contains(lowerSearchTerm)) ||
                log.Difficulty.ToString().ToLower().Contains(lowerSearchTerm) ||
                log.DistanceKm.ToString(CultureInfo.InvariantCulture).ToLower().Contains(lowerSearchTerm) ||
                log.DurationHours.ToString(CultureInfo.InvariantCulture).ToLower().Contains(lowerSearchTerm) ||
                log.Rating.ToString().ToLower().Contains(lowerSearchTerm) ||
                log.LogDate.ToString().ToLower().Contains(lowerSearchTerm)
            ).Select(log => log.TourId);

            // Add tour IDs that have matching logs
            foreach (var tourId in toursWithMatchingLogs)
            {
                matchingTourIds.Add(tourId);
            }

            // Return all tours that match either directly or through their logs
            var filteredEntities = allTourEntities.Where(t => matchingTourIds.Contains(t.Id)).ToList();
            return filteredEntities.Select(ToModel).ToList();
        }
    }
}
