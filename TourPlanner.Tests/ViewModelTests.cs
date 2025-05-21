using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.UI.ViewModels;
using TourPlanner.UI.Services;
using TourPlanner.UI.Interfaces;

namespace TourPlanner.Tests
{
    public class ViewModelTests
    {
        TourListViewModel tourListViewModel;
        IDialogService dialogService;

        [SetUp]
        public void Setup()
        {
            dialogService = new MockDialogService();
            tourListViewModel = new(dialogService);
        }
    }
}
