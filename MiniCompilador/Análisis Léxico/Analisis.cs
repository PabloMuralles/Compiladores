using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace MiniCompilador.Análisis_Léxico
{
    class Analisis
    {
        Expreciones expreciones = new Expreciones();
        public void LecturaArchivo(string path)
        {
            int contadorLinea = 0;
            var archivo = new StreamReader(path);
            var linea = archivo.ReadLine();
            while (linea != null)
            {
                linea = linea.Trim();
                Analizador(linea);
                contadorLinea++;
                linea = archivo.ReadLine();
            }
        }

        private void Analizador(string Cadena)
        {
           var listaMatch = new List<string>();
            string dato = string.Empty;
            var objExpreciones = new Expreciones();

            foreach (var item in Cadena)
            {
                dato += item;

                if (Validar(dato,objExpreciones))
                {
                    listaMatch.Add(dato);
                    dato = string.Empty;
                }
                

                 
            }
        }
        

        private bool Validar(string cadena, Expreciones objExpreciones_)
        {
            

            if (objExpreciones_.palabrasReservadas_.IsMatch(cadena))
            {
                return true;
            }
            else
            {
                return default;
            }
        }
    }
}
