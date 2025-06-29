using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;

namespace TourPlanner.UI.Interfaces
{
    public interface ITourCoordinatorService
    {
        event Action? ToursChanged;
        void AddTour();
        void RequestTourRefresh();
    }
}
