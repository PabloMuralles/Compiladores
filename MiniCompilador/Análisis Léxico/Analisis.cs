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
            // 
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
            var stringEncontrado = false;
            var comentarioLinea = false;
            var comentarioMultiple = false;


            for (int i = 0; i < listaCaracteres.Count(); i++)
            {
                if (listaCaracteres[i].ToString() == " " || listaCaracteres[i].ToString() == "\t")
                {
                    if (stringEncontrado == true)
                    {
                        dato += listaCaracteres[i].ToString();
                    }

                    contadorAux = contadorColumana + 1;
                    contadorColumana++;


                }
                else
                {
                    // esto es para no separar los strings

                    // falta ver como validar el cambio de linea
                    dato += listaCaracteres[i].ToString();
                    if (listaCaracteres[i] == '"' && comentarioMultiple == false)
                    {
                        if (stringEncontrado == false)
                        {
                            stringEncontrado = true;
                            dato += listaCaracteres[i].ToString();
                            contadorAux = contadorColumana + 1;
                            contadorColumana++;
                        }
                        else if (stringEncontrado == true)
                        {
                            stringEncontrado = false;
                            lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                            dato = string.Empty;
                            contadorAux = contadorColumana + 1;
                            contadorColumana++;

                        }
                    }
                    else if (objExpreciones.signosPuntuacion_.IsMatch(listaCaracteres[i].ToString()) && stringEncontrado == false && comentarioMultiple == false)
                    {
                        // esto es para no separar un double a la hora de identificar lexemas
                        if (listaCaracteres[i] == '.' && dato.Length != 0)
                        {
                            if (i + 1 < listaCaracteres.Count())
                            {
                                var datoAnterior = Convert.ToChar(dato.Substring(dato.Length - 2, 1));
                                if (char.IsDigit(listaCaracteres[i + 1]) && char.IsDigit(datoAnterior))
                                {
                                    dato += listaCaracteres[i + 1].ToString();
                                    lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                                    i++;
                                }
                                else
                                {
                                    lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                                    dato = string.Empty;
                                    contadorAux = contadorColumana + 1;
                                    contadorColumana++;
                                }
                            }
                            else
                            {
                                lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                                dato = string.Empty;
                                contadorAux = contadorColumana + 1;
                                contadorColumana++;
                            }
                        }
                        else
                        {
                            lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                            dato = string.Empty;
                            contadorAux = contadorColumana + 1;
                            contadorColumana++;
                        }

                    }
                    else if (objExpreciones.llavesSimples_.IsMatch(listaCaracteres[i].ToString()) && stringEncontrado == false && comentarioMultiple == false)
                    {
                        // esto es para verificar las llaves que vienen juntas []
                        if (listaCaracteres[i] == '[' || listaCaracteres[i] == '(' || listaCaracteres[i] == '{')
                        {
                            if (i + 1 < listaCaracteres.Count())
                            {
                                if (listaCaracteres[i + 1] == ']' || listaCaracteres[i + 1] == '}' || listaCaracteres[i + 1] == ')')
                                {
                                    dato += listaCaracteres[i + 1].ToString();
                                    lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                                    dato = string.Empty;
                                    i++;
                                }
                                else
                                {
                                    lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                                    dato = string.Empty;
                                    contadorAux = contadorColumana + 1;
                                    contadorColumana++;
                                }
                            }
                            else
                            {
                                lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                                dato = string.Empty;
                                contadorAux = contadorColumana + 1;
                                contadorColumana++;
                            }

                        }
                        else
                        {
                            lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                            dato = string.Empty;
                            contadorAux = contadorColumana + 1;
                            contadorColumana++;
                        }

                    }
                    else if (objExpreciones.caracteresSimples_.IsMatch(listaCaracteres[i].ToString()) && stringEncontrado == false)
                    {
                        // esto es para verificar los comentarios
                        if (listaCaracteres[i] == '/' && comentarioMultiple == false)
                        {
                            if (i + 1 < listaCaracteres.Count())
                            {
                                if (listaCaracteres[i + 1] == '/')
                                {
                                    dato = dato.Remove(dato.Length - 1, 1);
                                    if (dato.Length != 0)
                                    {
                                        lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                                        dato = string.Empty;

                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                                else if (listaCaracteres[i + 1] == '*')
                                {
                                    dato = dato.Remove(dato.Length - 1, 1);
                                    if (dato.Length != 0)
                                    {
                                        lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                                        dato = string.Empty;

                                    }
                                    comentarioMultiple = true;
                                    i++;

                                }
                                else
                                {
                                    lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                                    dato = string.Empty;
                                    contadorAux = contadorColumana + 1;
                                    contadorColumana++;
                                }
                            }
                            else
                            {
                                lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                                dato = string.Empty;
                                contadorAux = contadorColumana + 1;
                                contadorColumana++;
                            }

                        }
                        else if (listaCaracteres[i] == '*' && comentarioMultiple == true)
                        {
                            if (i + 1 < listaCaracteres.Count())
                            {
                                if (listaCaracteres[i + 1] == '/')
                                {
                                    comentarioMultiple = false;
                                    dato = string.Empty;
                                    i++;
                                }

                            }
                        }
                        else
                        {
                            lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                            dato = string.Empty;
                            contadorAux = contadorColumana + 1;
                            contadorColumana++;
                        }
                    }
                    else
                    {
                        if (i + 1 < listaCaracteres.Count())
                        {

                            if ((objExpreciones.caracteresDobles_.IsMatch(listaCaracteres[i + 1].ToString()) || listaCaracteres[i + 1].ToString() == " " ||
                                listaCaracteres[i + 1].ToString() == "\t" || (objExpreciones.llavesSimples_.IsMatch(listaCaracteres[i + 1].ToString())))
                            && stringEncontrado == false && comentarioMultiple == false)
                            {


                                lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                                dato = string.Empty;
                                contadorAux = contadorColumana + 1;
                                contadorColumana++;


                            }

                        }
                        else
                        {
                            lexemas_.Add(new Tuple<string, string>(dato, $"ok"));
                            dato = string.Empty;
                            contadorAux = contadorColumana + 1;
                            contadorColumana++;
                        }
                    }




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