using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms.VisualStyles;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Forms;

namespace MiniCompilador.Análisis_Léxico
{
    class Analisis
    {
        /// <summary>
        /// Metodo para poder leer el archivo que ingresa el usuriario
        /// </summary>
        /// <param name="path">direccion del archivo</param>
        public void LecturaArchivo(string path)
        {
            var lexemas = new List<Tuple<string, string>>();
            int contadorLinea = 1;
            var archivo = new StreamReader(path);
            var linea = archivo.ReadLine();
            while (linea != null)
            {
                //linea = linea.Trim();
                IdentificadorLexemas(linea, contadorLinea, lexemas);
                contadorLinea++;
                linea = archivo.ReadLine();
            }
        }
        /// <summary>
        /// Metodo que va identificar los lexemas del archivo de entrada
        /// </summary>
        /// <param name="Cadena">Linea que se va analizar</param>
        /// <param name="linea_">numero de linea que se esta analizando</param>
        /// <param name="lexemas_">diccionario donde se encontraran los lexemas con el numero de linea y columan</param>
        private void IdentificadorLexemas(string Cadena, int linea_, List<Tuple<string, string>> lexemas_)
        {

            string dato = string.Empty;
            var listaCaracteres = Cadena.ToList();
            var objExpreciones = new Expreciones();
            var contadorColumana = 1;
            var contadorAux = 1;

            for (int i = 0; i < listaCaracteres.Count(); i++)
            {
                if (listaCaracteres[i].ToString() != " ")
                {
                    dato += listaCaracteres[i].ToString();
                    if (i + 1 < listaCaracteres.Count())
                    {

                        if (objExpreciones.caracteres_.IsMatch(listaCaracteres[i + 1].ToString()) || listaCaracteres[i + 1].ToString() == " " ||
                            objExpreciones.caracteres_.IsMatch(listaCaracteres[i].ToString()))
                        {

                            lexemas_.Add(new Tuple<string, string>(dato, $"{linea_},{contadorAux}-{contadorColumana}"));
                            dato = string.Empty;
                            contadorAux = contadorColumana;

                        }

                    }
                    else
                    {
                        lexemas_.Add(new Tuple<string, string>(dato, $"{linea_},{contadorAux}-{contadorColumana}"));
                        dato = string.Empty;
                        contadorAux = contadorColumana;
                    }

                    contadorColumana++;
                }
                else
                {
                    contadorAux = contadorColumana + 1;
                    contadorColumana++;
                }

            }


        }




        /// <summary>
        /// Metodo que valida con las expresiones regulares para ve en cual casa
        /// 1 = palabrasReservadas,2=identificador,3=booleana,4=entero,5=hexadecimal,6=double,7=cadena
        /// </summary>
        /// <param name="cadena">caracter o cadena a evaluar</param>
        /// <param name="objExpreciones_">el objeto de la clase expresiones</param>
        /// <returns>verdadero si caso con alguna y un int que seria el identificador con cual caso</returns>
        private (bool, int) Validar(string cadena, Expreciones objExpreciones_)
        {

            if (objExpreciones_.palabrasReservadas_.IsMatch(cadena))
            {
                return (true, 1);
            }
            else if (objExpreciones_.booleanas_.IsMatch(cadena))
            {
                return (true, 3);
            }
            else if (objExpreciones_.identificador_.IsMatch(cadena))
            {
                return (true, 2);
            }
            else if (objExpreciones_.entero_.IsMatch(cadena))
            {
                return (true, 4);
            }
            else if (objExpreciones_.hexadecimal_.IsMatch(cadena))
            {
                return (true, 5);
            }
            else if (objExpreciones_.doubles_.IsMatch(cadena))
            {
                return (true, 6);
            }
            else if (objExpreciones_.cadena_.IsMatch(cadena))
            {
                return (true, 7);
            }
            else
            {
                return (true, 0);
            }
        }
    }
}
