using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Personal_Finance_Manager.Models;
using Quan_Li_Chi_Tieu.Models;
using System.ComponentModel;

namespace Quan_Li_Chi_Tieu.Services;

public static class ExportService
{
    public static void ExportToExcel(int userId, string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        using var context = new QuanlichitieuContext();

        // Sheet 1: Overview
        var wsOverview = package.Workbook.Worksheets.Add("Tổng Quan");
        CreateOverviewSheet(wsOverview, context, userId);

        // Sheet 2: Transactions
        var wsTransactions = package.Workbook.Worksheets.Add("Giao Dịch");
        CreateTransactionsSheet(wsTransactions, context, userId);

        // Sheet 3: Categories
        var wsCategories = package.Workbook.Worksheets.Add("Danh Mục");
        CreateCategoriesSheet(wsCategories, context, userId);

        // Sheet 4: Monthly Summary
        var wsMonthlySummary = package.Workbook.Worksheets.Add("Thống Kê Tháng");
        CreateMonthlySummarySheet(wsMonthlySummary, context, userId);

        package.SaveAs(new FileInfo(filePath));
    }

    private static void CreateOverviewSheet(ExcelWorksheet ws, QuanlichitieuContext context, int userId)
    {
        // Title
        ws.Cells["A1"].Value = "BÁO CÁO TỔNG QUAN CHI TIÊU";
        ws.Cells["A1:D1"].Merge = true;
        ws.Cells["A1"].Style.Font.Size = 16;
        ws.Cells["A1"].Style.Font.Bold = true;
        ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        ws.Cells["A2"].Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Cells["A2:D2"].Merge = true;

        // Summary data
        var totalIncome = context.Transactions
            .Where(t => t.UserId == userId && t.TransactionType == "Income")
            .Sum(t => (decimal?)t.Amount) ?? 0;
        var totalExpense = context.Transactions
            .Where(t => t.UserId == userId && t.TransactionType == "Expense")
            .Sum(t => (decimal?)t.Amount) ?? 0;
        var balance = totalIncome - totalExpense;

        ws.Cells["A4"].Value = "Tổng Thu Nhập";
        ws.Cells["B4"].Value = totalIncome;
        ws.Cells["B4"].Style.Numberformat.Format = "#,##0";

        ws.Cells["A5"].Value = "Tổng Chi Tiêu";
        ws.Cells["B5"].Value = totalExpense;
        ws.Cells["B5"].Style.Numberformat.Format = "#,##0";

        ws.Cells["A6"].Value = "Số Dư";
        ws.Cells["B6"].Value = balance;
        ws.Cells["B6"].Style.Numberformat.Format = "#,##0";
        ws.Cells["B6"].Style.Font.Bold = true;

        // Current month
        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;

        var monthIncome = context.Transactions
            .Where(t => t.UserId == userId && t.TransactionType == "Income" &&
                       t.TransactionDate.Month == currentMonth && t.TransactionDate.Year == currentYear)
            .Sum(t => (decimal?)t.Amount) ?? 0;
        var monthExpense = context.Transactions
            .Where(t => t.UserId == userId && t.TransactionType == "Expense" &&
                       t.TransactionDate.Month == currentMonth && t.TransactionDate.Year == currentYear)
            .Sum(t => (decimal?)t.Amount) ?? 0;

        ws.Cells["A8"].Value = $"THÁNG {currentMonth}/{currentYear}";
        ws.Cells["A8:B8"].Merge = true;
        ws.Cells["A8"].Style.Font.Bold = true;

        ws.Cells["A9"].Value = "Thu Nhập Tháng";
        ws.Cells["B9"].Value = monthIncome;
        ws.Cells["B9"].Style.Numberformat.Format = "#,##0";

        ws.Cells["A10"].Value = "Chi Tiêu Tháng";
        ws.Cells["B10"].Value = monthExpense;
        ws.Cells["B10"].Style.Numberformat.Format = "#,##0";

        ws.Column(1).Width = 20;
        ws.Column(2).Width = 20;
    }

    private static void CreateTransactionsSheet(ExcelWorksheet ws, QuanlichitieuContext context, int userId)
    {
        // Header
        ws.Cells["A1"].Value = "STT";
        ws.Cells["B1"].Value = "Ngày";
        ws.Cells["C1"].Value = "Loại";
        ws.Cells["D1"].Value = "Danh Mục";
        ws.Cells["E1"].Value = "Mô Tả";
        ws.Cells["F1"].Value = "Số Tiền";

        var headerRange = ws.Cells["A1:F1"];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(30, 60, 114));
        headerRange.Style.Font.Color.SetColor(System.Drawing.Color.White);

        // Data
        var transactions = context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.TransactionDate)
            .ThenByDescending(t => t.CreatedDate)
            .ToList();

        var row = 2;
        var stt = 1;
        foreach (var t in transactions)
        {
            ws.Cells[row, 1].Value = stt++;
            ws.Cells[row, 2].Value = t.TransactionDate.ToString("dd/MM/yyyy");
            ws.Cells[row, 3].Value = t.TransactionType == "Income" ? "Thu" : "Chi";
            ws.Cells[row, 4].Value = t.Category?.CategoryName ?? "";
            ws.Cells[row, 5].Value = t.Description ?? "";
            ws.Cells[row, 6].Value = t.Amount;
            ws.Cells[row, 6].Style.Numberformat.Format = "#,##0";

            if (t.TransactionType == "Income")
                ws.Cells[row, 6].Style.Font.Color.SetColor(System.Drawing.Color.Green);
            else
                ws.Cells[row, 6].Style.Font.Color.SetColor(System.Drawing.Color.Red);

            row++;
        }

        // Auto fit columns
        ws.Column(1).Width = 8;
        ws.Column(2).Width = 15;
        ws.Column(3).Width = 10;
        ws.Column(4).Width = 20;
        ws.Column(5).Width = 30;
        ws.Column(6).Width = 15;
    }

    private static void CreateCategoriesSheet(ExcelWorksheet ws, QuanlichitieuContext context, int userId)
    {
        // Header
        ws.Cells["A1"].Value = "STT";
        ws.Cells["B1"].Value = "Tên Danh Mục";
        ws.Cells["C1"].Value = "Loại";
        ws.Cells["D1"].Value = "Số Giao Dịch";
        ws.Cells["E1"].Value = "Tổng Tiền";

        var headerRange = ws.Cells["A1:E1"];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(30, 60, 114));
        headerRange.Style.Font.Color.SetColor(System.Drawing.Color.White);

        var categories = context.Categories
            .Where(c => c.UserId == userId)
            .Select(c => new
            {
                c.CategoryName,
                c.CategoryType,
                TransactionCount = c.Transactions.Count,
                TotalAmount = c.Transactions.Sum(t => t.Amount)
            })
            .OrderBy(c => c.CategoryType)
            .ThenBy(c => c.CategoryName)
            .ToList();

        var row = 2;
        var stt = 1;
        foreach (var c in categories)
        {
            ws.Cells[row, 1].Value = stt++;
            ws.Cells[row, 2].Value = c.CategoryName;
            ws.Cells[row, 3].Value = c.CategoryType == "Income" ? "Thu Nhập" : "Chi Tiêu";
            ws.Cells[row, 4].Value = c.TransactionCount;
            ws.Cells[row, 5].Value = c.TotalAmount;
            ws.Cells[row, 5].Style.Numberformat.Format = "#,##0";
            row++;
        }

        ws.Column(1).Width = 8;
        ws.Column(2).Width = 25;
        ws.Column(3).Width = 15;
        ws.Column(4).Width = 15;
        ws.Column(5).Width = 15;
    }

    private static void CreateMonthlySummarySheet(ExcelWorksheet ws, QuanlichitieuContext context, int userId)
    {
        var currentYear = DateTime.Now.Year;

        // Header
        ws.Cells["A1"].Value = $"THỐNG KÊ THEO THÁNG - NĂM {currentYear}";
        ws.Cells["A1:D1"].Merge = true;
        ws.Cells["A1"].Style.Font.Size = 14;
        ws.Cells["A1"].Style.Font.Bold = true;

        ws.Cells["A3"].Value = "Tháng";
        ws.Cells["B3"].Value = "Thu Nhập";
        ws.Cells["C3"].Value = "Chi Tiêu";
        ws.Cells["D3"].Value = "Chênh Lệch";

        var headerRange = ws.Cells["A3:D3"];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(30, 60, 114));
        headerRange.Style.Font.Color.SetColor(System.Drawing.Color.White);

        var monthlyData = context.Transactions
            .Where(t => t.UserId == userId && t.TransactionDate.Year == currentYear)
            .GroupBy(t => new { t.TransactionDate.Month, t.TransactionType })
            .Select(g => new { g.Key.Month, g.Key.TransactionType, Total = g.Sum(t => t.Amount) })
            .ToList();

        for (int month = 1; month <= 12; month++)
        {
            var row = month + 3;
            var income = monthlyData.FirstOrDefault(d => d.Month == month && d.TransactionType == "Income")?.Total ?? 0;
            var expense = monthlyData.FirstOrDefault(d => d.Month == month && d.TransactionType == "Expense")?.Total ?? 0;

            ws.Cells[row, 1].Value = $"Tháng {month}";
            ws.Cells[row, 2].Value = income;
            ws.Cells[row, 2].Style.Numberformat.Format = "#,##0";
            ws.Cells[row, 3].Value = expense;
            ws.Cells[row, 3].Style.Numberformat.Format = "#,##0";
            ws.Cells[row, 4].Value = income - expense;
            ws.Cells[row, 4].Style.Numberformat.Format = "#,##0";

            if (income - expense >= 0)
                ws.Cells[row, 4].Style.Font.Color.SetColor(System.Drawing.Color.Green);
            else
                ws.Cells[row, 4].Style.Font.Color.SetColor(System.Drawing.Color.Red);
        }

        // Total row
        ws.Cells["A16"].Value = "TỔNG CỘNG";
        ws.Cells["A16"].Style.Font.Bold = true;
        ws.Cells["B16"].Formula = "SUM(B4:B15)";
        ws.Cells["B16"].Style.Numberformat.Format = "#,##0";
        ws.Cells["B16"].Style.Font.Bold = true;
        ws.Cells["C16"].Formula = "SUM(C4:C15)";
        ws.Cells["C16"].Style.Numberformat.Format = "#,##0";
        ws.Cells["C16"].Style.Font.Bold = true;
        ws.Cells["D16"].Formula = "SUM(D4:D15)";
        ws.Cells["D16"].Style.Numberformat.Format = "#,##0";
        ws.Cells["D16"].Style.Font.Bold = true;

        ws.Column(1).Width = 15;
        ws.Column(2).Width = 15;
        ws.Column(3).Width = 15;
        ws.Column(4).Width = 15;
    }
}
