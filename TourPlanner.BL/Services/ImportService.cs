﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Interfaces;
using CsvHelper;
using System.Globalization;
using TourPlanner.BL.DTOs;
using CsvHelper.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using TourPlanner.Logging;
using TourPlanner.Logging.Interfaces;

namespace TourPlanner.BL.Services
{
    public class ImportService : IImportService
    {

        private readonly ILoggerWrapper _logger;

        public ImportService(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<ImportService>();
        }

        public enum ContentType
        {
            Error,
            Unknown,
            Tour,
            TourLog
        }

        public ContentType DetectCsvType(string filePath)
        {
            try
            {
                var headerLine = File.ReadLines(filePath).FirstOrDefault();
                if (headerLine is null)
                    return ContentType.Unknown;

                var headers = headerLine.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                var tourHeaders = new[] {"Name", "StartLocation", "EndLocation", "TransportType" };
                var logHeaders = new[] {"LogDate", "Comment", "Difficulty" };

                if (tourHeaders.All(h => headers.Contains(h)))
                    return ContentType.Tour;

                if (logHeaders.All(h => headers.Contains(h)))
                    return ContentType.TourLog;

                return ContentType.Unknown;
            }
            catch (IOException ex)
            {
                _logger.Error("Cannot read file: " + ex.Message);
            }
            return ContentType.Error;
        }

        public List<TourDTO> ImportToursFromCSV(string filepath)
        {
            try
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
            catch (IOException ex)
            {
                _logger.Error("Cannot read file: " + ex.Message);
            }

            return new List<TourDTO>();
        }

        public List<TourLogDTO> ImportTourLogsFromCSV(string filepath)
        {
            try
            {
                using var reader = new StreamReader(filepath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var tourData = csv.GetRecords<TourLogDTO>().ToList();

                return tourData;
            }
            catch (IOException ex)
            {
                _logger.Error("Cannot read file: " + ex.Message);
            }

            return new List<TourLogDTO>();
        }

        public ContentType DetectJsonType(string filePath)
        {
            try 
            { 
                var json = File.ReadAllText(filePath);
                using var doc = JsonDocument.Parse(json);

                var root = doc.RootElement;
                if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
                    return ContentType.Unknown;

                var firstObj = root[0];

                if (firstObj.TryGetProperty("StartLocation", out _) &&
                    firstObj.TryGetProperty("EndLocation", out _))
                    return ContentType.Tour;

                if (firstObj.TryGetProperty("Comment", out _) &&
                    firstObj.TryGetProperty("Difficulty", out _))
                    return ContentType.TourLog;

                return ContentType.Unknown;
            }
            catch (IOException ex)
            {
                _logger.Error("Cannot read file: " + ex.Message);
            }
            return ContentType.Error;
        }

        public List<TourDTO> ImportToursFromJson(string filePath)
        {
            try 
            {
                var json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                var tours = JsonSerializer.Deserialize<List<TourDTO>>(json, options);
                return tours ?? new List<TourDTO>();
            }
            catch (IOException ex)
            {
                _logger.Error("Cannot read file: " + ex.Message);
            }
            return new List<TourDTO>();
        }

        public List<TourLogDTO> ImportTourLogsFromJson(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                var tours = JsonSerializer.Deserialize<List<TourLogDTO>>(json, options);
                return tours ?? new List<TourLogDTO>();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot read file: " + ex.Message);
            }
            return new List<TourLogDTO>();
        }
    }
}
