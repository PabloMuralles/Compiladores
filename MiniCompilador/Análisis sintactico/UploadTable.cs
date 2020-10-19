using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace Minic.Análisis_sintactico
{
    class UploadTable
    {
        public static Thread threadTable;

        public static Dictionary<int, Dictionary<string, string>> table = new Dictionary<int, Dictionary<string, string>>();
        static public void LoadThread()
        {
            threadTable = new Thread(new ThreadStart(ReadFile));
            threadTable.Start();
        }
        static public void ReadFile()
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var projectPath = appDirectory.Substring(0, appDirectory.IndexOf("\\MiniCompilador"));

             var path = Path.Combine(projectPath, "Gramatica","Tabla_de_analisis_LR0.xlsx");

            _Application excel = new Excel.Application();
            Workbook wb = excel.Workbooks.Open(path);
            Worksheet ws = wb.Worksheets[1];

            Excel.Range range;

            range = ws.UsedRange;
            var rw = range.Rows.Count;
            var cl = range.Columns.Count;

            var symbols = new List<string>();
            var data = new List<string>();
             

            var state = 0;
            for (int i = 1; i < rw; i++)
            {
                var tempDictionary = new Dictionary<string, string>();
                for (int j = 1; j < cl; j++)
                {
                    if (i == 1)
                    {
                        symbols.Add((string)(range.Cells[i, j] as Excel.Range).Value2);
                    }
                    else
                    {
                        if (j == 1)
                        {
                            state = Convert.ToInt32((range.Cells[i, j] as Excel.Range).Value2);
                        }
                        else
                        {
                            var tempAction = Convert.ToString((range.Cells[i, j] as Excel.Range).Value2);

                            if (tempAction != null)
                            {
                                var tempSymbols = symbols[j - 1];
                                tempDictionary.Add(tempSymbols, tempAction);

                            }

                        }
                    }

                }
                if (i != 1)
                {
                    table.Add(state, tempDictionary);
                }
                
            }
            wb.Close(0);
            excel.Quit();
        }
    }
}
