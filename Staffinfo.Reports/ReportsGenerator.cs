using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Staffinfo.Reports
{
    /// <summary>
    /// Generates reports
    /// </summary>
    public static class ReportsGenerator
    {
        /// <summary>
        /// Returns the 'total employees' report
        /// </summary>
        /// <returns></returns>
        public async static Task<MemoryStream> GetTotalEmployeesListAsXlsx()
        {
            try
            {
                //for testing
                //var templateDocument =
                //    (Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName + @"\" +
                //     @"Staffinfo.Reports\Templates\Total_Employees.xlsx");

                var templateDocument = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\Staffinfo.Reports\Templates\Total_Employees.xlsx");
                MemoryStream output = new MemoryStream();

                using (FileStream templateDocumentStream = File.OpenRead(templateDocument))
                {
                    using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                    {
                        ExcelWorksheet sheet = package.Workbook.Worksheets["TE"];

                        int rowIndex = 4;

                        var query = await ReportDataManager.GetTotalEmployeesData();

                        foreach (var row in query.Rows)
                        {
                            sheet.Cells[rowIndex, 1].Value = ((DataRow) row)["lastname"];
                            sheet.Cells[rowIndex, 2].Value = ((DataRow) row)["firstname"];
                            sheet.Cells[rowIndex, 3].Value = ((DataRow) row)["middlename"];
                            sheet.Cells[rowIndex, 4].Value = ((DataRow) row)["birthdate"];
                            sheet.Cells[rowIndex, 5].Value = ((DataRow) row)["currentrank"];
                            sheet.Cells[rowIndex, 6].Value = ((DataRow) row)["currentpost"];

                            rowIndex++;
                        }

                        sheet.Cells[4, 1, (rowIndex - 1), 6].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                        sheet.Cells[4, 1, (rowIndex - 1), 6].Style.Font.Size = 10;
                        sheet.Cells[4, 1, (rowIndex - 1), 6].Style.Font.Name = "Times New Roman";
                        sheet.Cells[4, 1, (rowIndex - 1), 6].Style.WrapText = true;
                        sheet.Cells[4, 4, (rowIndex - 1), 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;//set data to center
                        sheet.Cells[4, 1, (rowIndex - 1), 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;//set all cells to center by vertical

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}