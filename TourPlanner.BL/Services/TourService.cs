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

namespace TourPlanner.BL.Services
{
    public class TourService : ITourService
    {
        public ITourRepository _tourRepository;

        public TourService(ITourRepository TourRepo)
        {
            _tourRepository = TourRepo;
        }

        public void AddTour(TourDTO tour)
        {
           TourEntity entity = ToEntity(tour);
           _tourRepository.AddTour(entity);
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
                TransportType = entity.TransportType,
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
                TransportType = model.TransportType,
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

            // Filter the list of tours based on the search term
            var filteredEntities = allTourEntities.Where(t =>
            {
                // Check if the search term appears in any of the string fields:
                // - Name
                // - Description
                // - StartLocation
                // - EndLocation
                if ((t.Name != null && t.Name.ToLower().Contains(lowerSearchTerm)) ||
                    (t.Description != null && t.Description.ToLower().Contains(lowerSearchTerm)) ||
                    (t.StartLocation != null && t.StartLocation.ToLower().Contains(lowerSearchTerm)) ||
                    (t.EndLocation != null && t.EndLocation.ToLower().Contains(lowerSearchTerm)))
                {
                    return true;
                }

                // Check if the search term appears in the TransportType field
                if (t.TransportType.ToString().ToLower().Contains(lowerSearchTerm))
                {
                    return true;
                }

                // Check if the search term appears in numeric fields 
                // Numeric number to string and "."
                if (t.DistanceKm.ToString(CultureInfo.InvariantCulture).ToLower().Contains(lowerSearchTerm) ||
                    t.EstimatedTimeHours.ToString(CultureInfo.InvariantCulture).ToLower().Contains(lowerSearchTerm))
                {
                    return true;
                }

                // If none of the above conditions are met, the tour does not match the search term
                return false;

            }).ToList();

            //Convert the filtered entities to TourDTO objects for UI consumption
            return filteredEntities.Select(ToModel).ToList();
        }
    }
}
