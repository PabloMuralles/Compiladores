using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Minic.Análisis_sintactico
{
    class Upload
    {
        /// <summary>
        /// Method to read de excel file that conteins the analysis table 
        /// </summary>
        /// <returns>dictionary that contains the analysis table </returns>
        public Dictionary<int, Dictionary<string, string>> ReadTxtFileAnalysisTable()
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
                            state = Convert.ToInt32(tempLine[i].Trim());
                        }
                        else
                        {
                            if (tempLine[i] != " ")
                            {
                                var temp = listSymbols[i];
                                tempDictionary.Add(listSymbols[i].Trim(),tempLine[i].Trim());
                            }
                        }

                    }
                    tableTemp.Add(state, tempDictionary);
                }
                line = sw.ReadLine();
                countProduction++;
            }
            sw.Close();
            return tableTemp;
        }

        /// <summary>
        /// Method to read the file that have the grammar in folder of the proyect inside of the folder grammar 
        /// </summary>
        /// <returns>a dictionary that contains the productions of the grammar</returns>
        public Dictionary<int, Tuple<int, string>> ReadTxtFileGrammar()
        {
           var grammar = new Dictionary<int, Tuple<int, string>>();

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

            return grammar;

        }

    }
}
