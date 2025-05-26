using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.UI.Map
{
    public static class MapEventService
    {
        public static event Action<string, string>? UpdateMapRequested;
        public static event Action? SaveMapImageRequested;

        public static void RequestMapUpdate(string startLocation, string endLocation)
        {
            // Triggers the event
            UpdateMapRequested?.Invoke(startLocation, endLocation);
        }
        public static void RequestMapImageSave()
        {
            SaveMapImageRequested?.Invoke();
        }
    }
}
