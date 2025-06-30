using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using TourPlanner.BL.DTOs;
using CsvHelper;
using System.Globalization;
using TourPlanner.BL.Interfaces;

namespace TourPlanner.BL.Services
{
    public class ExportService : IExportService
    {

        public void ExportToursToCsv(List<TourDTO> tours, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(tours);
        }

        public void ExportToursToJson(List<TourDTO> tours, string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() } 
            };

            var json = JsonSerializer.Serialize(tours, options);
            File.WriteAllText(filePath, json);
        }
    }
}
