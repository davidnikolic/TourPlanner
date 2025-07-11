﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;

namespace TourPlanner.BL.Interfaces
{
    public interface ISearchService
    {
        string CurrentSearchTerm { get; set; }

        event EventHandler SearchTermChanged;
        List<TourDTO> SearchTours(string searchTerm);
    }
}
