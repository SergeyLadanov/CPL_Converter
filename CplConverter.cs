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

        public CplConverter(String OutFileName)
        {
            FilePath = OutFileName;
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
        private void OutInExcel(StreamReader inputSr)
        {
            // Создаём объект - экземпляр нашего приложения
            Excel.Application excelApp = new Excel.Application();
            // Создаём экземпляр рабочей книги Excel
            Excel.Workbook workBook;
            // Создаём экземпляр листа Excel
            Excel.Worksheet workSheet;
            workBook = excelApp.Workbooks.Add();
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);
            // Заполняем первый столбец листа из массива Y[0..n-1]
            //for (int j = 1; j <= n; j++)
            //    workSheet.Cells[j, 1] = j;


            workBook.SaveAs(FilePath);
            workBook.Close();
        }


        public void HandleCPL(String InputFileName)
        {
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


                Row = ParseRow(sr.ReadLine());

                Row[1] = "";
                //OutInExcel(sr);
            }

            sr.Close();

        }
    }
}
