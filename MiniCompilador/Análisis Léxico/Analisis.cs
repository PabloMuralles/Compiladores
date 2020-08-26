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

            var archivo = new StreamReader(path);

            IdentificadorLexemas(lexemas, archivo);


        }
        /// <summary>
        /// Metodo que va identificar los lexemas del archivo de entrada
        /// </summary>
        /// <param name="Cadena">Linea que se va analizar</param>
        /// <param name="linea_">numero de linea que se esta analizando</param>
        /// <param name="lexemas_">diccionario donde se encontraran los lexemas con el numero de linea y columan</param>
        private void IdentificadorLexemas(List<Tuple<string, string>> lexemas_, StreamReader archivo_)
        {

            string dato = string.Empty;
            var objExpreciones = new Expreciones();
            var stringEncontrado = false;
            var comentarioLinea = false;
            var comentarioMultiple = false;
            var contadorLinea = 1;
            var validarDoubles = false;
            var notacionCientifica = false;
            var contadorColumAux = 0;

            var linea = archivo_.ReadLine();

            while (linea != null)
            {
                // no puedo borrar el dato al final por si encuentra un string pero se podria hacer si lo guardamos en un temp
                var listaCaracteres = linea.ToList();

                if (stringEncontrado == true)
                {
                    // cuando los strings son de mas lineas
                    stringEncontrado = false;
                    lexemas_.Add(new Tuple<string, string>(dato, $"{(contadorColumAux + 1) - (dato.Length - 1)}-{contadorColumAux + 1},{contadorLinea - 1},Error la cadena no se cerro"));
                    dato = string.Empty;


                }
                for (int i = 0; i < listaCaracteres.Count(); i++)
                {
                    if (listaCaracteres[i].ToString() == " " || listaCaracteres[i].ToString() == "\t")
                    {
                        // si es un espacio en blanco o tab
                        if (stringEncontrado == true)
                        {
                            dato += listaCaracteres[i].ToString();
                        }

                    }
                    else
                    {
                        dato += listaCaracteres[i].ToString();
                        if (listaCaracteres[i] == '"' && comentarioMultiple == false)
                        {
                            // esto es para no separar los strings
                            if (stringEncontrado == false)
                            {
                                stringEncontrado = true;
                                dato += listaCaracteres[i].ToString();

                            }
                            else if (stringEncontrado == true)
                            {
                                stringEncontrado = false;
                                lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}, {contadorLinea}"));
                                dato = string.Empty;
                            }
                        }
                        else if (objExpreciones.signosPuntuacion_.IsMatch(listaCaracteres[i].ToString()) && stringEncontrado == false && comentarioMultiple == false)
                        {
                            // esto es para no separar un double a la hora de identificar lexemas
                            if (listaCaracteres[i] == '.' && (validarDoubles == false || notacionCientifica == true))
                            {
                                if (i + 1 < listaCaracteres.Count())
                                {
                                    if (i != 0 && dato.Length > 2)
                                    {
                                        var datoAnterior = Convert.ToChar(dato.Substring(dato.Length - 2, 1));
                                        if (char.IsDigit(listaCaracteres[i + 1]) && char.IsDigit(datoAnterior) && validarDoubles == false)
                                        {
                                            validarDoubles = true;
                                        }
                                        else if (char.IsDigit(listaCaracteres[i + 1]) && !char.IsDigit(datoAnterior))
                                        {
                                            dato += listaCaracteres[i + 1].ToString();
                                            validarDoubles = true;
                                            i++;

                                        }
                                        else if ((listaCaracteres[i + 1] == 'E' || listaCaracteres[i + 1] == 'e'))
                                        {
                                            dato += listaCaracteres[i + 1].ToString();
                                            validarDoubles = true;
                                            notacionCientifica = true;
                                            i++;

                                        }
                                        else if (notacionCientifica == true)
                                        {
                                            var datoAux = dato.Remove(dato.Length - 1, 1);
                                            dato = dato.Remove(0, dato.Length - 1);
                                            lexemas_.Add(new Tuple<string, string>(datoAux, $"{(i + 1) - (dato.Length - 1)}-{i - dato.Length}, {contadorLinea}"));
                                        }
                                        else
                                        {
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                            dato = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        if (char.IsDigit(listaCaracteres[i + 1]))
                                        {
                                            dato += listaCaracteres[i + 1].ToString();
                                            validarDoubles = true;
                                            i++;
                                        }
                                        else if (listaCaracteres[i + 1] == 'E' || listaCaracteres[i + 1] == 'e')
                                        {
                                            dato += listaCaracteres[i + 1].ToString();
                                            validarDoubles = true;
                                            notacionCientifica = true;
                                            i++;
                                        }
                                    }
                                }
                                else
                                {
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                    dato = string.Empty;
                                }
                            }
                            else
                            {
                                //verificar que pasa cuando empieza con punto
                                if (dato.Length > 1)
                                {
                                    var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                    dato = dato.Remove(0, dato.Length - 1);
                                    lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{(i) - (cadenaAux.Length - 1)}-{i},{contadorLinea}"));
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{i + 1}-{i + dato.Length},{contadorLinea}"));
                                    dato = string.Empty;

                                }
                                else
                                {
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                    dato = string.Empty;
                                }

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
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                        dato = string.Empty;
                                        i++;
                                    }
                                    else
                                    {
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                        dato = string.Empty;
                                    }
                                }
                                else
                                {
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                    dato = string.Empty;
                                }

                            }
                            else
                            {
                                lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                dato = string.Empty;
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
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
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
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                            dato = string.Empty;
                                        }
                                        comentarioMultiple = true;
                                        i++;

                                    }
                                    else
                                    {
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                        dato = string.Empty;
                                    }
                                }
                                else
                                {
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                    dato = string.Empty;
                                }

                            }
                            else if (listaCaracteres[i] == '*' && comentarioMultiple == true)
                            {
                                if (i + 1 < listaCaracteres.Count())
                                {
                                    if (listaCaracteres[i + 1] == '/')
                                    {
                                        if (comentarioMultiple == true)
                                        {
                                            comentarioMultiple = false;
                                            dato = string.Empty;
                                            i++;

                                        }
                                        else
                                        {
                                            lexemas_.Add(new Tuple<string, string>("Ç", $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}, Comentario sin emparejar"));
                                            dato = string.Empty;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (comentarioMultiple == false && comentarioLinea == false && notacionCientifica == false)
                                {
                                    //verificar que pasa cuando empieza con un caracter simple
                                    if (dato.Length > 1)
                                    {
                                        var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                        dato = dato.Remove(0, dato.Length - 1);
                                        lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{(i) - (cadenaAux.Length - 1)}-{i},{contadorLinea}"));
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{i + 1}-{i + dato.Length},{contadorLinea}"));
                                        dato = string.Empty;

                                    }
                                    else
                                    {
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                        dato = string.Empty;
                                    }
                                }
                            }
                        }
                        else if (validarDoubles == true || notacionCientifica == true)
                        {
                            if (validarDoubles == true && notacionCientifica == false)
                            {
                                if (char.IsDigit(listaCaracteres[i]))
                                {
                                    if (i + 1 < listaCaracteres.Count())
                                    {
                                        if (char.IsDigit(listaCaracteres[i + 1]))
                                        {
                                            /// ver para que sirve esto 
                                        }
                                        else if (listaCaracteres[i + 1] == 'E' || listaCaracteres[i + 1] == 'e')
                                        {
                                            dato += listaCaracteres[i + 1].ToString();
                                            notacionCientifica = true;
                                            i++;
                                        }
                                        else
                                        {
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                            dato = string.Empty;
                                            validarDoubles = false;

                                        }
                                    }
                                    else
                                    {
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                        dato = string.Empty;
                                        validarDoubles = false;
                                    }

                                }
                                else if ((listaCaracteres[i] == 'e' || listaCaracteres[i] == 'E') && notacionCientifica == false && validarDoubles == true)
                                {
                                    if (i + 1 < listaCaracteres.Count())
                                    {

                                        if (char.IsDigit(listaCaracteres[i + 1]) || listaCaracteres[i + 1] == '-' || listaCaracteres[i + 1] == '+')
                                        {
                                            dato += listaCaracteres[i + 1].ToString();
                                            notacionCientifica = true;
                                            i++;
                                        }
                                        else
                                        {
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                            dato = string.Empty;
                                            validarDoubles = false;
                                        }
                                    }
                                    else
                                    {
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                        dato = string.Empty;
                                        validarDoubles = false;
                                    }
                                }
                                else
                                {
                                    /// deje este como ok porque no se si de verdad va llegar a funcionar
                                    var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                    dato = dato.Remove(0, dato.Length - 1);
                                    //
                                    lexemas_.Add(new Tuple<string, string>(cadenaAux, $"ok - "));
                                    validarDoubles = false;

                                }
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
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                    dato = string.Empty;
                                }
                                else if (!objExpreciones.letras_.IsMatch(listaCaracteres[i].ToString()) && stringEncontrado == false && comentarioMultiple == false &&
                                    comentarioLinea == false && notacionCientifica == false && !objExpreciones.caracteres_.IsMatch(listaCaracteres[i].ToString()) &&
                                    !char.IsDigit(listaCaracteres[i]) && listaCaracteres[i] != '_')
                                     {
                                        var cadenaAux = dato.Remove(0, dato.Length - 1);
                                        dato = dato.Remove(dato.Length - 1, 1);
                                        lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{(i + 1) - (cadenaAux.Length - 1)}-{i + 1}, {contadorLinea}"));
                                     }
                                
                            }
                            else if (stringEncontrado == false && comentarioLinea == false && comentarioMultiple == false)
                            {
                                // este else if se quito en el commit donde se separaban caracteres diferentes de nuestro contexto. No recuerdo porque si no interifere con ese codigo.
                                lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                dato = string.Empty;
                            }

                        }
                    }
                    contadorColumAux = i;
                }
                if (validarDoubles == true)
                {
                    if (dato.Length != 0)
                    {
                        lexemas_.Add(new Tuple<string, string>(dato, $"{(contadorColumAux + 1) - (dato.Length - 1)}-{contadorColumAux + 1}, {contadorLinea}"));
                        dato = string.Empty;

                    }
                    validarDoubles = false;

                }
                contadorLinea++;

                linea = archivo_.ReadLine();
            }
            if (stringEncontrado == true)
            {
                lexemas_.Add(new Tuple<string, string>(dato, $"EOF Cadena"));
                dato = string.Empty;
            }
            if (comentarioMultiple == true)
            {
                lexemas_.Add(new Tuple<string, string>("Ç", $"EOF Comentario"));
                dato = string.Empty;
            }
        }
    }
}