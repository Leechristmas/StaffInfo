using System;
using System.IO;
using System.Web;
using OfficeOpenXml;

namespace Staffinfo.Reports
{
    public static class ReportDataManager
    {
        public static MemoryStream GetTotalEmployeesListAsAReport()
        {
            try
            {

                var t = AppDomain.CurrentDomain;

                string templateDocument = "~/Staffinfo.Reports/Templates/Total_Employees.xlsx";//Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                 //$"Reporting/Templates/Total_Employees.xlxs");

                MemoryStream output = new MemoryStream();

                using (FileStream templateDocumentStream = File.OpenRead(templateDocument))
                {
                    using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                    {
                        ExcelWorksheet sheet = package.Workbook.Worksheets["TE"];

                        int rowIndex = 4;

                        sheet.Cells[rowIndex, 1].Value = "Иванов";
                        sheet.Cells[rowIndex, 2].Value = "Иван";
                        sheet.Cells[rowIndex, 3].Value = "Иванович";
                        sheet.Cells[rowIndex, 4].Value = (new DateTime(1978, 3, 4)).ToString("d");
                        sheet.Cells[rowIndex, 5].Value = "Сержант";
                        sheet.Cells[rowIndex, 6].Value = "Водитель-водолаз";


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