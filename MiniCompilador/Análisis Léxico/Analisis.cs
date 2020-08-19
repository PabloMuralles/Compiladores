using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MiniCompilador.Análisis_Léxico
{
    class Analisis
    {
        public void Analizador(string path)
        {
            var archivo = new StreamReader(path);
            var linea = archivo.ReadLine();
            while (linea != null)
            {
                linea = linea.Trim().ToLower();

            }
        }
        
    }
}
