using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Staffinfo.Reports
{
    /// <summary>
    /// Generates reports
    /// </summary>
    public static class ReportsGenerator
    {

        public async static Task<MemoryStream> GetTotalEmployeesListAsPdf()
        {
            try
            {
                MemoryStream output = new MemoryStream();

                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, output);
                document.Open();

                //getting data
                var query = await ReportDataManager.GetTotalEmployeesData();

                //table creation
                PdfPTable table = new PdfPTable(6);

                //fonts
                string fg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Tahoma.TTF");
                BaseFont fgBaseFont = BaseFont.CreateFont(fg, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font textFont = new Font(fgBaseFont, 10, Font.NORMAL, BaseColor.BLACK);
                Font tableHeader = new Font(fgBaseFont, 12, Font.NORMAL, BaseColor.BLACK);
                Font titleFont = new Font(fgBaseFont, 16, Font.BOLD, BaseColor.BLACK);

                //title
                table.HeaderRows = 1;
                Paragraph title = new Paragraph(new Phrase("Сотрудники", titleFont))
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };

                //header
                table.AddCell(new PdfPCell(new Phrase(@"Фамилия", tableHeader))
                {
                    VerticalAlignment = Element.ALIGN_CENTER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    BorderWidthLeft = 1f,
                    BorderWidthTop = 1f,
                    BorderWidthBottom = 1f,
                    BorderWidthRight = 0.5f
                });
                table.AddCell(new PdfPCell(new Phrase(@"Имя", tableHeader))
                {
                    VerticalAlignment = Element.ALIGN_CENTER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    BorderWidthLeft = 0.5f,
                    BorderWidthTop = 1f,
                    BorderWidthBottom = 1f,
                    BorderWidthRight = 0.5f
                });
                table.AddCell(new PdfPCell(new Phrase(@"Отчество", tableHeader))
                {
                    VerticalAlignment = Element.ALIGN_CENTER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    BorderWidthLeft = 0.5f,
                    BorderWidthTop = 1f,
                    BorderWidthBottom = 1f,
                    BorderWidthRight = 0.5f
                });
                table.AddCell(new PdfPCell(new Phrase(@"Дата Рождения", tableHeader))
                {
                    VerticalAlignment = Element.ALIGN_CENTER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    BorderWidthLeft = 0.5f,
                    BorderWidthTop = 1f,
                    BorderWidthBottom = 1f,
                    BorderWidthRight = 0.5f
                });
                table.AddCell(new PdfPCell(new Phrase(@"Звание", tableHeader))
                {
                    VerticalAlignment = Element.ALIGN_CENTER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    BorderWidthLeft = 0.5f,
                    BorderWidthTop = 1f,
                    BorderWidthBottom = 1f,
                    BorderWidthRight = 0.5f
                });
                table.AddCell(new PdfPCell(new Phrase(@"Должность", tableHeader))
                {
                    VerticalAlignment = Element.ALIGN_CENTER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    BorderWidthLeft = 0.5f,
                    BorderWidthTop = 1f,
                    BorderWidthBottom = 1f,
                    BorderWidthRight = 1f
                });

                //filling table
                await Task.Run(() =>
                {
                    foreach (DataRow row in query.Rows)
                    {
                        table.AddCell(new PdfPCell(new Phrase(row["lastname"].ToString(), textFont))
                        {
                            VerticalAlignment = Element.ALIGN_CENTER,
                            BorderWidth = 0.2f,
                            BorderWidthLeft = 1f
                        });
                        table.AddCell(new PdfPCell(new Phrase(row["firstname"].ToString(), textFont))
                        {
                            VerticalAlignment = Element.ALIGN_CENTER,
                            BorderWidth = 0.2f,
                        });
                        table.AddCell(new PdfPCell(new Phrase(row["middlename"].ToString(), textFont))
                        {
                            VerticalAlignment = Element.ALIGN_CENTER,
                            BorderWidth = 0.2f,
                        });
                        table.AddCell(new PdfPCell(new Phrase(row["birthdate"].ToString(), textFont))
                        {
                            VerticalAlignment = Element.ALIGN_CENTER,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BorderWidth = 0.2f,
                        });
                        table.AddCell(new PdfPCell(new Phrase(row["currentrank"].ToString(), textFont))
                        {
                            VerticalAlignment = Element.ALIGN_CENTER,
                            BorderWidth = 0.2f,
                        });
                        table.AddCell(new PdfPCell(new Phrase(row["currentpost"].ToString(), textFont))
                        {
                            VerticalAlignment = Element.ALIGN_CENTER,
                            BorderWidth = 0.2f,
                            BorderWidthRight = 1f
                        });
                    }
                    
                    document.Add(title);
                    document.Add(table);
                });
                document.Close();

                return output;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

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