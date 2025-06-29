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
        public static event Action<string>? SaveMapImageRequested;

        public static event Action? MapReady; // NEU

        public static void RequestMapUpdate(string startLocation, string endLocation)
        {
            // Triggers the event
            UpdateMapRequested?.Invoke(startLocation, endLocation);
        }
        public static void RequestMapImageSave(string path)
        {
            SaveMapImageRequested?.Invoke(path);
        }

        public static void NotifyMapReady()
        {
            MapReady?.Invoke();
        }
    }
}
