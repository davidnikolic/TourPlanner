using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BL.Interfaces
{
    public interface IMapService
    {
        Task UpdateMapAsync(string startLocation, string endLocation);
        Task SaveMapImageAsync(string fullImagePath);
        Task ClearMapAsync();
    }
}
