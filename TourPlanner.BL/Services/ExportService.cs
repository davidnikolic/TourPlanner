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
using TourPlanner.Logging;
using TourPlanner.Logging.Interfaces;

namespace TourPlanner.BL.Services
{
    public class ExportService : IExportService
    {

        public ILoggerWrapper _logger {  get; set; }

        public ExportService(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<ExportService>();
        }

        public void ExportToursToCsv(List<TourDTO> tours, string folderPath)
        {
            try
            {
                var toursFilePath = Path.Combine(folderPath, "tours.csv");
                using (var writer = new StreamWriter(toursFilePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(tours);
                }

                var logsFolder = Path.Combine(folderPath, "logs");
                Directory.CreateDirectory(logsFolder);

                foreach (var tour in tours)
                {
                    var logs = tour.TourLogs;
                    if (logs.Count == 0)
                        continue;


                    var logFilePath = Path.Combine(logsFolder, $"{tour.Name}_logs.csv");

                    using var writer = new StreamWriter(logFilePath);
                    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                    csv.WriteRecords(logs);
                }
            }
            catch (IOException ex)
            {
                _logger.Error("Cannot read file: " + ex.Message);
            }
        }

        public void ExportToursToJson(List<TourDTO> tours, string filePath)
        {
            try
            { 
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() } 
                };

                var json = JsonSerializer.Serialize(tours, options);
                File.WriteAllText(filePath, json);
            }
            catch (IOException ex)
            {
                _logger.Error("Cannot read file: " + ex.Message);
            }
        }
    }
}
