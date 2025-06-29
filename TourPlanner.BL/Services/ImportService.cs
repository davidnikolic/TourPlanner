using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Interfaces;
using CsvHelper;
using System.Globalization;
using TourPlanner.BL.DTOs;
using CsvHelper.Configuration;

namespace TourPlanner.BL.Services
{
    public class ImportService : IImportService
    {
        public enum CsvContentType
        {
            Unknown,
            Tour,
            TourLog
        }

        public CsvContentType DetectCsvType(string filePath)
        {
            var headerLine = File.ReadLines(filePath).FirstOrDefault();
            if (headerLine is null)
                return CsvContentType.Unknown;

            var headers = headerLine.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var tourHeaders = new[] { "Id", "Name", "StartLocation", "EndLocation", "TransportType" };
            var logHeaders = new[] { "Id", "TourId", "Date", "Comment", "Difficulty" };

            if (tourHeaders.All(h => headers.Contains(h)))
                return CsvContentType.Tour;

            if (logHeaders.All(h => headers.Contains(h)))
                return CsvContentType.TourLog;

            return CsvContentType.Unknown;
        }

        public List<TourDTO> ImportToursFromCSV(string filepath)
        {
            using var reader = new StreamReader(filepath);
            
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,                // Ignoriere fehlende Spalten
                HeaderValidated = null,                  // Ignoriere fehlende Header
                IgnoreBlankLines = true
            };

            using var csv = new CsvReader(reader, config);

            var tourData = csv.GetRecords<TourDTO>().ToList();

            return tourData;
        }

        public List<TourLogDTO> ImportTourLogsFromCSV(string filepath)
        {
            using var reader = new StreamReader(filepath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var tourData = csv.GetRecords<TourLogDTO>().ToList();

            return tourData;
        }

        public void ImportJSON(string filepath)
        {
            throw new NotImplementedException();
        }
    }
}
