using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Models;

namespace TourPlanner.BL.Interfaces
{
    internal interface ITourLogService
    {
        void AddTourLog(TourLog tourLog);

        void DeleteTourLog(TourLog tourLog);
        List<TourLog> GetTourLogs();

        void UpdateTourLog(TourLog tourLog);
    }
}
