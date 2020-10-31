using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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

             var path = Path.Combine(projectPath, "Gramatica", "Tabla_analisis.txt");

            var sw = new StreamReader(path);

            var line = sw.ReadLine();

            var countProduction = 0;

            var listSymbols = new List<string>();

            var tableTemp = new Dictionary<int, Dictionary<string, string>>();

            while (line != null)
            {
                var tempDictionary = new Dictionary<string, string>();
                var state = 0;
                if (countProduction == 0)
                {
                    listSymbols = line.Split('_').ToList();
                }
                else
                {
                    
                    var tempLine = line.Split('_');

                    for (int i = 0; i < tempLine.Length; i++)
                    {
                        if (i == 0)
                        {
                            state = Convert.ToInt32(tempLine[i]);
                        }
                        else
                        {
                            if (tempLine[i] != " ")
                            {
                                tempDictionary.Add(listSymbols[i-1],tempLine[i].ToString());
                            }
                        }

                    }
                    tableTemp.Add(state, tempDictionary);
                }
                line = sw.ReadLine();
                countProduction++;
            }
            sw.Close();
            table = tableTemp;
        }

        /// <summary>
        /// Method to read the file that have the grammar in folder of the proyect inside of the folder grammar 
        /// </summary>
        static public void ReadTxtFileGrammar()
        {
            grammar = new Dictionary<int, Tuple<int, string>>();

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
            sw.Close();

        }

    }
}
