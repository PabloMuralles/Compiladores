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
        Expreciones expreciones = new Expreciones();
        public void Analizador(string path)
        {
            int contador_linea = 0;
            var archivo = new StreamReader(path);
            var linea = archivo.ReadLine();
            while (linea != null)
            {
                linea = linea.Trim();
                Caracteres(linea);
                contador_linea++;
                linea = archivo.ReadLine();
            }
        }

        public void Caracteres(string Cadena)
        {
          
            foreach (var item in Cadena)
            {
                var dato = item ;
                // Analisar item si pertenece a una expresion 
                // regular
                if (Exprecion(Convert.ToString(dato)))
                {
                  dato += dato;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dato"> Caracter leido de la cadena</param>
        /// <returns>
        /// muestra si pertenece a la "ER" Exprecion regular
        /// </returns>
        public bool Exprecion(string dato)
        {
            bool ER = expreciones.identificador_.IsMatch(dato);
            return ER;
        }
    }
}
