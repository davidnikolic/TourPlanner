using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;

namespace TourPlanner.UI.ViewModels
{
    internal class TourLogsViewModel : ViewModelBase
    {
        public List<TourLogDTO> TourLogs { get; set; }

        public TourLogsViewModel() { 
            TourLogs = new List<TourLogDTO>();
            TourLogs.Add(new TourLogDTO()
            {
                Id = 0,
                TourId = 1,
                DurationHours = 12,
            });
        }
    }
}
