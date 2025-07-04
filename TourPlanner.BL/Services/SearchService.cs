using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;

namespace TourPlanner.BL.Services
{
    public class SearchService : ISearchService
    {
        private readonly ITourService _tourService;
        private readonly ITourLogService _tourLogService;

        private string _currentSearchTerm = string.Empty;
        public string CurrentSearchTerm
        {
            get => _currentSearchTerm;
            set
            {
                if (_currentSearchTerm != value)
                {
                    _currentSearchTerm = value;
                    SearchTermChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler? SearchTermChanged;

        public SearchService(ITourService tourService, ITourLogService tourLogService)
        {
            _tourService = tourService;
            _tourLogService = tourLogService;
        }

        public List<TourDTO> SearchTours(string searchTerm)
        {
            var allTours = _tourService.GetTours();

            if (string.IsNullOrWhiteSpace(searchTerm))
                return allTours;

            var lowerTerm = searchTerm.ToLower().Trim();

            if (lowerTerm.StartsWith("popularity:") || lowerTerm.StartsWith("childfriendliness:")
                || lowerTerm.StartsWith("distance:") || lowerTerm.StartsWith("duration:"))
            {
                var matchingLogs = _tourLogService.SearchTourLogs(searchTerm);
                var tourIds = matchingLogs.Select(l => l.TourId).Distinct();
                return allTours.Where(t => tourIds.Contains(t.Id)).ToList();
            }

            // Direkt in Tours suchen
            var matchedTourIds = new HashSet<int>();

            foreach (var t in allTours)
            {
                if (t.Name != null && t.Name.ToLower().Contains(lowerTerm) ||
                    t.Description != null && t.Description.ToLower().Contains(lowerTerm) ||
                    t.StartLocation != null && t.StartLocation.ToLower().Contains(lowerTerm) ||
                    t.EndLocation != null && t.EndLocation.ToLower().Contains(lowerTerm) ||
                    t.TransportType.ToString().ToLower().Contains(lowerTerm) ||
                    t.DistanceKm.ToString(CultureInfo.InvariantCulture).Contains(lowerTerm) ||
                    t.EstimatedTimeHours.ToString(CultureInfo.InvariantCulture).Contains(lowerTerm))
                {
                    matchedTourIds.Add(t.Id);
                }
            }

            // Suche in TourLogs
            var logs = _tourLogService.GetAllTourLogs();
            foreach (var log in logs)
            {
                if (log.Comment != null && log.Comment.ToLower().Contains(lowerTerm) ||
                    log.Difficulty.ToString().ToLower().Contains(lowerTerm) ||
                    log.DistanceKm.ToString(CultureInfo.InvariantCulture).Contains(lowerTerm) ||
                    log.DurationHours.ToString(CultureInfo.InvariantCulture).Contains(lowerTerm) ||
                    log.Rating.ToString().ToLower().Contains(lowerTerm) ||
                    log.LogDate.ToString().ToLower().Contains(lowerTerm))
                {
                    matchedTourIds.Add(log.TourId);
                }
            }

            return allTours.Where(t => matchedTourIds.Contains(t.Id)).ToList();
        }
    }
}
