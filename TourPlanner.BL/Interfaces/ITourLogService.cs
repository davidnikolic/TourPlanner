﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.DAL.Entities;

namespace TourPlanner.BL.Interfaces
{
    public interface ITourLogService
    {
        void AddTourLog(TourLogDTO log);
        List<TourLogDTO> GetTourLogsForTour(int tourId);
        List<TourLogDTO> GetAllTourLogs();
        void UpdateTourLog(TourLogDTO log);
        void DeleteTourLog(TourLogDTO log);
        List<TourLogDTO> SearchTourLogs(string searchTerm);
    }
}
