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
    class Upload
    {
        public static Thread threadTable;

        public static Dictionary<int, Dictionary<string, string>> table = new Dictionary<int, Dictionary<string, string>>();

        public static Dictionary<int, Tuple<int, string>> grammar = new Dictionary<int, Tuple<int, string>>();

        /// <summary>
        /// Method to create the thread to do de method to read de excel file
        /// </summary>
        static public void LoadThread()
        {
            threadTable = new Thread(new ThreadStart(ReadExcelFile));
            threadTable.Start();
        }
        /// <summary>
        /// Method to read de excel file that conteins the analysis table 
        /// </summary>
        static public void ReadExcelFile()
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

        /// <summary>
        /// Method to read the file that have the grammar in folder of the proyect inside of the folder grammar 
        /// </summary>
        static public void ReadTxtFile()
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var projectPath = appDirectory.Substring(0, appDirectory.IndexOf("\\MiniCompilador"));

            var path = Path.Combine(projectPath, "Gramatica", "Gramatica.txt");

            var sw = new StreamReader(path);

            var line = sw.ReadLine();

            var countProduction = 0;

            while (line != null)
            {
                string[] stringSeparators = new string[] { "->" };
                line.Trim();
                var splitPoduction = line.Split(stringSeparators, StringSplitOptions.None);
                var splitDefinition = splitPoduction[1].Trim().Split(' ');

                grammar.Add(countProduction, new Tuple<int, string>(splitDefinition.Count(), splitPoduction[0].Trim()));

                line = sw.ReadLine();
                countProduction++;
            }

        }

    }
}
