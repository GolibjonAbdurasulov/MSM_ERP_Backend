using Core.Attributes;
using DataAccess.Entities;
using Microsoft.AspNetCore.Components;
using OfficeOpenXml;
using Services.Interfaces;
using Services.ViewModels.WorkerViewModels;

namespace Services.Services;
[Injectable]
public class ExcelReaderService: IExcelReaderService
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
                    PersonnelNumber = long.Parse(tabNo.Trim()),
                
                    // C ustun — FIO
                    FullName = sheet.Cells[row, 3].Value?.ToString()?.Trim() ?? "",
                
                    // D ustun — Штатная единица узб
                    Position = sheet.Cells[row, 4].Value?.ToString()?.Trim() ?? "",
                
                    DepartmentId = departmentId
                };
            
                workers.Add(worker);
            }
            
            return workers; 
    } 
}
