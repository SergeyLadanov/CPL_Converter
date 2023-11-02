using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Charts;
using OfficeOpenXml;
using OfficeOpenXml.Core.ExcelPackage;
using System.Reflection;

using GemBox.Spreadsheet;

namespace CPL_Converter
{
    class CplConverter
    {
        private String FilePath = "";
        private StreamReader sr;
        private int ColumnCount = 0;
        private String[] ColumnNames = { "Designator", "Layer", "Mid X", "Mid Y", "Rotation" };
        private bool AutoSave = false;


        public CplConverter(String OutFileName)
        {
            FilePath = OutFileName;
        }

        // Разрешение автосохранения
        public void AutoSaveEnable()
        {
            AutoSave = true;
        }

        // Запрещение автосохранения
        public void AutoSaveDisable()
        {
            AutoSave = false;
        }
        // Функция рассчета числа колонок в текстовом файле
        private int GetColumnCount(String Input)
        {
            int Result = 0;
            // Разделение строки по пробелам
            String[] Row = Input.Split(' ');

            for (int i = 0; i < Row.Length; i++)
            {
                if (Row[i] != "")
                {
                    Result++;
                }
            }

            return Result;
        }

        // Функция исправления слов в строке
        private String LineCorrection(String InputStr)
        {
            InputStr = InputStr.Replace("TopLayer", "Top");
            InputStr = InputStr.Replace("BottomLayer", "Bottom");
            return InputStr;
        }

        //Функция выделения данных из строки
        private String[] ParseRow(String Input)
        {
            // Разделение строки по пробелам
            String[] Row = Input.Split(' ');
            String[] Result = new string[ColumnCount];
            int ColPos = 0;
            // Выделение информации из строки
            for (int i = 0; i < Row.Length; i++)
            {
                if (Row[i] != "")
                {
                    if (ColPos < ColumnCount)
                    {
                        Result[ColPos] = Row[i];
                        ColPos++;
                    }
                }
            }

            return Result;
        }


        private string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";

            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }

            return columnName;
        }

        private Cell InsertCell(uint rowIndex, uint columnIndex, Worksheet worksheet)
        {
            Row row = null;
            var sheetData = worksheet.GetFirstChild<SheetData>();

            // Check if the worksheet contains a row with the specified row index.
            row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
            if (row == null)
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // Convert column index to column name for cell reference.
            var columnName = GetExcelColumnName((int)columnIndex);
            var cellReference = columnName + rowIndex;      // e.g. A1

            // Check if the row contains a cell with the specified column name.
            var cell = row.Elements<Cell>()
                       .FirstOrDefault(c => c.CellReference.Value == cellReference);
            if (cell == null)
            {
                cell = new Cell() { CellReference = cellReference };
                if (row.ChildElements.Count < columnIndex)
                    row.AppendChild(cell);
                else
                    row.InsertAt(cell, (int)columnIndex);
            }

            return cell;
        }



        // Сохранение данных  в MS Excel
        private int OutInExcel(StreamReader inputSr)
        {
            String Line;
            String[] RowData;
            int RowCount = 2;

            //SpreadsheetDocument document = SpreadsheetDocument.Create("TestFile.xls", SpreadsheetDocumentType.Workbook);


            //WorkbookPart workbookPart = document.AddWorkbookPart();
            //workbookPart.Workbook = new Workbook();

            //WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            //worksheetPart.Worksheet = new Worksheet(new SheetData());

            //Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

            //Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "List1" };


            //sheets.Append(sheet);

            //workbookPart.Workbook.Save();

            //document.Save();


            using (SpreadsheetDocument document = SpreadsheetDocument.Create("Test.xlsx", SpreadsheetDocumentType.Workbook))
            {
                //CreatePartsForExcel(package, data);

                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "List1" };


                sheets.Append(sheet);

                workbookPart.Workbook.First().AppendChild(new Row());

                //sheet.First().Last().AppendChild(new Cell() { CellValue = new CellValue("test") });


                //SheetData data = sheet.GetFirstChild<SheetData>();


                Cell cell = InsertCell(1, 1, worksheetPart.Worksheet);

                cell.CellValue = new CellValue("skdcjskdjc");


                workbookPart.Workbook.Save();


            }



            //// Создаём объект - экземпляр нашего приложения
            //Excel.Application excelApp = new Excel.Application();
            //// Создаём экземпляр рабочей книги Excel
            //Excel.Workbook workBook;
            //// Создаём экземпляр листа Excel
            //Excel.Worksheet workSheet;
            //workBook = excelApp.Workbooks.Add();
            //workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);


            //// Заполнение шапки таблицы
            //for (int i = 1; i <= ColumnNames.Length; i++)
            //{
            //    workSheet.Cells[1, i] = ColumnNames[i - 1];
            //}

            //// Заполнение данными
            //while ((Line = inputSr.ReadLine()) != null)
            //{
            //    Line = LineCorrection(Line);
            //    RowData = ParseRow(Line);
            //    for (int i = 1; i <= ColumnCount; i++)
            //    {
            //        workSheet.Cells[RowCount, i] = RowData[i - 1];
            //    }

            //    RowCount++;
            //}

            //// Сохранение данных
            //if (AutoSave)
            //{
            //    workBook.SaveAs(FilePath);
            //    workBook.Close();
            //}
            //else
            //{
            //    excelApp.Visible = true;
            //    excelApp.UserControl = true;
            //}

            // Возврат количество обработанных строк
            return RowCount - 2;
        }


        public int HandleCPL(String InputFileName)
        {
            int RowCount = 0;
            sr = new StreamReader(InputFileName);
            String Line;

            ColumnCount = 0;

            // Поиск данных о расположении компонентов
            while ((Line = sr.ReadLine()) != null)
            {
                if (Line.Contains("Designator"))
                {
                    ColumnCount = GetColumnCount(Line);
                    break;
                }
            }

            // Если удалось выделить колонки
            if (ColumnCount > 0)
            {
                RowCount = OutInExcel(sr);
            }

            sr.Close();

            return RowCount;
        }
    }
}
