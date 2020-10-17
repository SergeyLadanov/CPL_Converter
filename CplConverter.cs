using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

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
        
        // Сохранение данных  в MS Excel
        private int OutInExcel(StreamReader inputSr)
        {
            String Line;
            String[] RowData;
            int RowCount = 2;
            // Создаём объект - экземпляр нашего приложения
            Excel.Application excelApp = new Excel.Application();
            // Создаём экземпляр рабочей книги Excel
            Excel.Workbook workBook;
            // Создаём экземпляр листа Excel
            Excel.Worksheet workSheet;
            workBook = excelApp.Workbooks.Add();
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);


            // Заполнение шапки таблицы
            for (int i = 1; i <= ColumnNames.Length; i++)
            {
                workSheet.Cells[1, i] = ColumnNames[i - 1];
            }
            
            // Заполнение данными
            while ((Line = inputSr.ReadLine()) != null)
            {
                Line = LineCorrection(Line);
                RowData = ParseRow(Line);
                for (int i = 1; i <= ColumnCount; i++)
                {
                    workSheet.Cells[RowCount, i] = RowData[i - 1];
                }

                RowCount++;
            }

            // Сохранение данных
            if (AutoSave)
            {
                workBook.SaveAs(FilePath);
                workBook.Close();
            }
            else
            {
                excelApp.Visible = true;
                excelApp.UserControl = true;
            }

            // Возврат количество обработанных строк
            return RowCount;
        }


        public int HandleCPL(String InputFileName)
        {
            int RowCount = 0;
            sr = new StreamReader(InputFileName);
            String Line;

            String[] Row;

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
