﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.UI.Interfaces.Coordinators
{
    public interface ITourImportCoordinator
    {
        Task ImportFromCsv(string path);
        Task ImportFromJson(string path);
    }
}
