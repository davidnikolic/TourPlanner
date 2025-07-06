using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.UI.Interfaces;

namespace TourPlanner.UI.Services
{
    public class UITabNavigationService : IUITabNavigationService
    {
        private int _selectedTabIndex = 1;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (_selectedTabIndex != value)
                {
                    _selectedTabIndex = value;
                    SelectedTabIndexChanged?.Invoke(value);
                }
            }
        }

        public event Action<int> SelectedTabIndexChanged;
    }
}
