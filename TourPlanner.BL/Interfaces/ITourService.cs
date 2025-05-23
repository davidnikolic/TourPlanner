﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using TourPlanner.DAL.Entities;

namespace TourPlanner.BL.Interfaces
{
    public interface ITourService
    {
        void AddTour(TourDTO tour);

        void DeleteTour(TourDTO tour);
        List<TourDTO> GetTours();

        void UpdateTour(TourDTO tour);
    }
}
