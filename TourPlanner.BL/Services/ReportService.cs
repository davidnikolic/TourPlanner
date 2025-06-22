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

namespace TourPlanner.BL.Services
{
    public class ReportService : IReportService
    {
        public void GenerateTourReport(TourDTO tour, string filePath)
        {
            // 1. PDF Writer & Document öffnen
            using PdfWriter writer = new(filePath);
            using PdfDocument pdf = new(writer);
            using Document doc = new(pdf);

            // 2. Fonts
            PdfFont titleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD, PdfEncodings.WINANSI);
            PdfFont bodyFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.WINANSI);

            // 3. Titel
            doc.Add(new Paragraph(tour.Name)
                .SetFont(titleFont)
                .SetFontSize(16)
                .SetMarginBottom(10));

            // 4. Beschreibung
            doc.Add(new Paragraph(tour.Description)
                .SetFont(bodyFont)
                .SetFontSize(12)
                .SetMarginBottom(15));

            // 5. Optional: Tour-Bild einfügen
            if (!string.IsNullOrEmpty(tour.RouteImagePath) && File.Exists(tour.RouteImagePath))
            {
                var imgData = ImageDataFactory.Create(tour.RouteImagePath);
                var image = new Image(imgData).ScaleToFit(400, 300).SetMarginBottom(15);
                doc.Add(image);
            }

            // 6. Tabelle mit Infos
            Table table = new Table(2);
            table.AddCell("Start");
            table.AddCell(tour.StartLocation);
            table.AddCell("Ziel");
            table.AddCell(tour.EndLocation);
            table.AddCell("Distanz");
            table.AddCell(tour.DistanceKm + " km");
            table.AddCell("Dauer");
            table.AddCell(tour.EstimatedTimeHours.ToString("0.0") + " h");

            doc.Add(table);
        }

        public void GenerateSummarizeReport(string file)
        {
            throw new NotImplementedException();
        }
    }
}
