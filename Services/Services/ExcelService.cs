using System.Drawing;
using Core.Attributes;
using DataAccess.Entities;
using Microsoft.AspNetCore.Components;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Services.Interfaces;
using Services.ViewModels.JobViewModels;
using Services.ViewModels.WorkerViewModels;

namespace Services.Services;
[Injectable]
public class ExcelService: IExcelService
{
    public async Task<List<Worker>> ReadWorkersFromExcel(Stream fileStream, long departmentId) 
    {
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
            var workers = new List<Worker>();
        
            using var package = new ExcelPackage(fileStream);
            var sheet = package.Workbook.Worksheets[0];
        
            // 3-qatordan boshlanadi (1,2 - header)
            var rowCount = sheet.Dimension.Rows;
        
            for (int row = 3; row <= rowCount; row++)
            {
                // Bo'sh qatorni o'tkazish
                var tabNo = sheet.Cells[row, 2].Value?.ToString();
                if (string.IsNullOrWhiteSpace(tabNo)) continue;
            
                var worker = new Worker
                {
                    // B ustun — Таб №
                    PersonnelNumber = tabNo.Trim(),
                
                    // C ustun — FIO
                    FullName = sheet.Cells[row, 3].Value?.ToString()?.Trim() ?? "",
                
                    // D ustun — Штатная единица узб
                    Position = sheet.Cells[row, 4].Value?.ToString()?.Trim() ?? "",
                
                    DepartmentId = departmentId
                };
                worker.SubDepartmentId = 1;
                workers.Add(worker);
            }
            
            return workers; 
    } 
    
    public byte[] Export(List<JobGetViewModel> jobs, string departmentName, DateTime from, DateTime to)
    {
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Hisobot");

        // Sarlavha
        ws.Cells["A1:I1"].Merge = true;
        ws.Cells["A1"].Value = "JOBLAR STATISTIKASI HISOBOTI";
        ws.Cells["A1"].Style.Font.Bold = true;
        ws.Cells["A1"].Style.Font.Size = 16;
        ws.Cells["A1"].Style.Font.Color.SetColor(Color.White);
        ws.Cells["A1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        ws.Cells["A1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 84, 150));
        ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        ws.Row(1).Height = 35;

        ws.Cells["A2:I2"].Merge = true;
        ws.Cells["A2"].Value = $"Davr: {from:dd.MM.yyyy} — {to:dd.MM.yyyy}    |    Bo'lim: {departmentName}";
        ws.Cells["A2"].Style.Font.Size = 10;
        ws.Cells["A2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        ws.Cells["A2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
        ws.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        // Header
        var headers = new[] { "№", "Nomi", "Bo'lim", "Nashriyotchi", "Holati", "Nashr sanasi", "Boshlanish", "Tugash", "Ishchilar" };
        var widths  = new[] { 5,   30,     20,       20,              15,       15,              15,           15,       12 };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cells[4, i + 1];
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Font.Color.SetColor(Color.White);
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(68, 114, 196));
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Column(i + 1).Width = widths[i];
        }

        // Ma'lumotlar
        var statusColors = new Dictionary<string, Color>
        {
            ["Completed"]  = Color.FromArgb(112, 173, 71),
            ["InProgress"] = Color.FromArgb(68,  114, 196),
            ["Pending"]    = Color.FromArgb(237, 125, 49),
            ["Cancelled"]  = Color.Red,
        };

        for (int i = 0; i < jobs.Count; i++)
        {
            var job = jobs[i];
            var row = i + 5;
            var bg  = i % 2 == 0 ? Color.FromArgb(242, 242, 242) : Color.White;

            ws.Cells[row, 1].Value = i + 1;
            ws.Cells[row, 2].Value = job.Title;
            ws.Cells[row, 3].Value = job.DepartmentName;
            ws.Cells[row, 4].Value = job.PublisherName;
            ws.Cells[row, 5].Value = job.JobStatus.ToString();
            ws.Cells[row, 6].Value = job.PublishedDate.ToString("dd.MM.yyyy");
            ws.Cells[row, 7].Value = job.StartedDate.ToString("dd.MM.yyyy");
            ws.Cells[row, 8].Value = job.EndDate.ToString("dd.MM.yyyy");
            ws.Cells[row, 9].Value = job.MobilizedWorkers;

            // Fon
            ws.Cells[row, 1, row, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[row, 1, row, 9].Style.Fill.BackgroundColor.SetColor(bg);

            // Status rang
            var statusCell = ws.Cells[row, 5];
            if (statusColors.TryGetValue(job.JobStatus.ToString(), out var sc))
            {
                statusCell.Style.Fill.BackgroundColor.SetColor(sc);
                statusCell.Style.Font.Color.SetColor(Color.White);
                statusCell.Style.Font.Bold = true;
            }

            ws.Cells[row, 1, row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[row, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        }

        // Jami
        var totalRow = jobs.Count + 5;
        ws.Cells[totalRow, 1, totalRow, 8].Merge = true;
        ws.Cells[totalRow, 1].Value = "JAMI ISHCHILAR:";
        ws.Cells[totalRow, 1].Style.Font.Bold = true;
        ws.Cells[totalRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        ws.Cells[totalRow, 1, totalRow, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
        ws.Cells[totalRow, 1, totalRow, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
        ws.Cells[totalRow, 9].Formula = $"SUM(I5:I{totalRow - 1})";
        ws.Cells[totalRow, 9].Style.Font.Bold = true;

        ws.Cells.Style.Font.Name = "Arial";
        ws.Cells.Style.Font.Size = 10;
        ws.View.FreezePanes(5, 1);

        return package.GetAsByteArray();
    }
}
