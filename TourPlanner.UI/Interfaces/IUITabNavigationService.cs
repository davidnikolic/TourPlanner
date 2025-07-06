using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.UI.Interfaces
{
    public interface IUITabNavigationService
    {
        int SelectedTabIndex { get; set; }

        event Action<int> SelectedTabIndexChanged;
    }
}
