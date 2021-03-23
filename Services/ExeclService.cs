using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SupplierPlatform.Services.Interface;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SupplierPlatform.Services
{
    /// <summary>
    /// EXECL 匯出服務
    /// </summary>
    public class ExeclService : IExeclService
    {
        /// <summary>
        /// 撥款匯出
        /// </summary>
        /// <param name="rowTitle"></param>
        /// <param name="rowData"></param>
        /// <returns></returns>
        public FileStreamResult GetCurrentDetail(string[] rowTitle, string[][] rowData)
        {
            MemoryStream memoryStream = new MemoryStream();

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                sheets.Append(new Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Sheet 1"
                });

                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                Row row = new Row();

                IEnumerable<Cell> title = rowTitle.Select(
                    o => new Cell
                    {
                        CellValue = new CellValue(o),
                        DataType = CellValues.String
                    });
                row.Append(title);
                sheetData.AppendChild(row);

                foreach (string[] item in rowData)
                {
                    row = new Row();
                    IEnumerable<Cell> datas = item.Select(
                    o => new Cell
                    {
                        CellValue = new CellValue(o),
                        DataType = CellValues.String
                    });

                    row.Append(datas);
                    sheetData.AppendChild(row);
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(memoryStream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}