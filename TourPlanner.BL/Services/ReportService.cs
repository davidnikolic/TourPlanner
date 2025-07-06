using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql.Internal;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using System.IO;
using TourPlanner.BL.DTOs;
using TourPlanner.BL.Interfaces;
using iText.IO.Font;
using iText.Layout.Properties;

namespace TourPlanner.BL.Services
{
    public class ReportService : IReportService
    {
        public void GenerateTourReport(TourDTO tour, string filePath)
        {
            // 1. OPEN PDF Writer & Document
            using PdfWriter writer = new(filePath);
            using PdfDocument pdf = new(writer);
            using Document doc = new(pdf);

            // 2. Fonts
            PdfFont titleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD, PdfEncodings.WINANSI);
            PdfFont bodyFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.WINANSI);

            // 3. Ttile
            doc.Add(new Paragraph(tour.Name)
                .SetFont(titleFont)
                .SetFontSize(16)
                .SetMarginBottom(10));

            // 4. Comment
            doc.Add(new Paragraph(tour.Description)
                .SetFont(bodyFont)
                .SetFontSize(12)
                .SetMarginBottom(15));

            // 5. Optional: Tour Image
            if (!string.IsNullOrEmpty(tour.RouteImagePath) && File.Exists(tour.RouteImagePath))
            {
                try
                {
                    var imgData = ImageDataFactory.Create(tour.RouteImagePath);
                    var image = new Image(imgData).ScaleToFit(400, 300).SetMarginBottom(15);
                    doc.Add(image);
                }
                catch (iText.IO.Exceptions.IOException ex)
                {
                    // LOG
                }
            }

            // 6. Table with information
            Table table = new Table(2);
            table.AddCell("Start");
            table.AddCell(tour.StartLocation);
            table.AddCell("End");
            table.AddCell(tour.EndLocation);
            table.AddCell("Distance");
            table.AddCell(tour.DistanceKm + " km");
            table.AddCell("Duration");
            table.AddCell(tour.EstimatedTimeHours.ToString("0.0") + " h");

            doc.Add(table);

            if (tour.TourLogs != null && tour.TourLogs.Any())
            {
                doc.Add(new Paragraph("Tour Logs")
                    .SetFont(titleFont)
                    .SetFontSize(14)
                    .SetMarginTop(20)
                    .SetMarginBottom(10));

                // 8. Table for tourlogs
                Table logTable = new Table(6).UseAllAvailableWidth();
                logTable.AddHeaderCell("Date");
                logTable.AddHeaderCell("Comment");
                logTable.AddHeaderCell("Difficulty");
                logTable.AddHeaderCell("Distance");
                logTable.AddHeaderCell("Duration");
                logTable.AddHeaderCell("Rating");

                foreach (var log in tour.TourLogs)
                {
                    logTable.AddCell(log.LogDate.ToShortDateString());
                    logTable.AddCell(log.Comment ?? "");
                    logTable.AddCell(log.Difficulty.ToString());
                    logTable.AddCell(log.DistanceKm.ToString("0.0") + " km");
                    logTable.AddCell(log.DurationHours.ToString("0.0") + " h");
                    logTable.AddCell(log.Rating.ToString());
                }

                doc.Add(logTable);
            }
            else
            {
                doc.Add(new Paragraph("No Tour-Logs")
                    .SetFont(bodyFont)
                    .SetFontSize(10)
                    .SetItalic()
                    .SetMarginTop(10));
            }
        }

        public void GenerateAllToursReport(List<TourDTO> tours, string filePath)
        {
            // 1. OPEN PDF Writer & Document 
            using PdfWriter writer = new(filePath);
            using PdfDocument pdf = new(writer);
            using Document doc = new(pdf);

            // 2. Fonts
            PdfFont titleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD, PdfEncodings.WINANSI);
            PdfFont bodyFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.WINANSI);

            foreach (TourDTO tour in tours)
            {

                doc.Add(new Paragraph(tour.Name)
                    .SetFont(titleFont)
                    .SetFontSize(16)
                    .SetMarginBottom(10));

                // 4. Comment
                doc.Add(new Paragraph(tour.Description)
                    .SetFont(bodyFont)
                    .SetFontSize(12)
                    .SetMarginBottom(15));

                // 5. Optional: Add Tour Image
                if (!string.IsNullOrEmpty(tour.RouteImagePath) && File.Exists(tour.RouteImagePath))
                {
                    try
                    {
                        var imgData = ImageDataFactory.Create(tour.RouteImagePath);
                        var image = new Image(imgData).ScaleToFit(400, 300).SetMarginBottom(15);
                        doc.Add(image);
                    }
                    catch(IOException)
                    {
                        //LOG
                    }
                }

                // 6. Table with more information
                Table table = new Table(2);
                table.AddCell("Start");
                table.AddCell(tour.StartLocation);
                table.AddCell("End");
                table.AddCell(tour.EndLocation);
                table.AddCell("Distance");
                table.AddCell(tour.DistanceKm + " km");
                table.AddCell("Duration");
                table.AddCell(tour.EstimatedTimeHours.ToString("0.0") + " h");

                doc.Add(table);

                if (tour.TourLogs != null && tour.TourLogs.Any())
                {
                    doc.Add(new Paragraph("Tour Logs")
                        .SetFont(titleFont)
                        .SetFontSize(14)
                        .SetMarginTop(20)
                        .SetMarginBottom(10));

                    // 8. Table for tourlogs
                    Table logTable = new Table(6).UseAllAvailableWidth();
                    logTable.AddHeaderCell("Date");
                    logTable.AddHeaderCell("Comment");
                    logTable.AddHeaderCell("Difficulty");
                    logTable.AddHeaderCell("Distance");
                    logTable.AddHeaderCell("Duration");
                    logTable.AddHeaderCell("Rating");

                    foreach (var log in tour.TourLogs)
                    {
                        logTable.AddCell(log.LogDate.ToShortDateString());
                        logTable.AddCell(log.Comment ?? "");
                        logTable.AddCell(log.Difficulty.ToString());
                        logTable.AddCell(log.DistanceKm.ToString("0.0") + " km");
                        logTable.AddCell(log.DurationHours.ToString("0.0") + " h");
                        logTable.AddCell(log.Rating.ToString());
                    }

                    doc.Add(logTable);
                }
                else
                {
                    doc.Add(new Paragraph("No Tour-Logs")
                        .SetFont(bodyFont)
                        .SetFontSize(10)
                        .SetItalic()
                        .SetMarginTop(10));
                }

                if (tours.Last() != tour) doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            }
        }

        public void GenerateSummarizeReport(List<TourStatisticsDTO> stats, string filePath)
        {
            // 1. Open PDF Writer & Document
            using PdfWriter writer = new(filePath);
            using PdfDocument pdf = new(writer);
            using Document doc = new(pdf);

            // 2. Fonts
            PdfFont titleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD, PdfEncodings.WINANSI);
            PdfFont bodyFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.WINANSI);

            foreach (TourStatisticsDTO stat in stats)
            {
                // Tour name as section title
                doc.Add(new Paragraph(stat.TourName)
                    .SetFont(titleFont)
                    .SetFontSize(14)
                    .SetMarginBottom(10));

                // Table with statistical values
                Table table = new Table(2).UseAllAvailableWidth();

                table.AddCell(CreateCell("Tour-ID:", bodyFont));
                table.AddCell(CreateCell(stat.TourId.ToString(), bodyFont));

                table.AddCell(CreateCell("Average Difficulty:", bodyFont));
                table.AddCell(CreateCell(stat.AvgDifficulty.ToString("0.00"), bodyFont));

                table.AddCell(CreateCell("Average Rating:", bodyFont));
                table.AddCell(CreateCell(stat.AvgRating.ToString("0.00"), bodyFont));

                table.AddCell(CreateCell("Popularity (Number of logs):", bodyFont));
                table.AddCell(CreateCell(stat.Popularity.ToString(), bodyFont));

                table.AddCell(CreateCell("Childfriendliness:", bodyFont));
                table.AddCell(CreateCell(stat.IsChildFriendly ? "Ja" : "Nein", bodyFont));

                doc.Add(table);
                doc.Add(new Paragraph("\n")); // blank line after each tour
            }

            doc.Close();
        }

        // Helper method for clean table cells
        private Cell CreateCell(string text, PdfFont font)
        {
            return new Cell()
                .Add(new Paragraph(text).SetFont(font).SetFontSize(11))
                .SetPadding(4)
                .SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER);
        }
    }
}
