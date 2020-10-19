using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MiniCompilador.Análisis_Léxico
{
    class Analisis
    {
        GUI.Cargar_Archivo cargar_Archivo = new GUI.Cargar_Archivo();
        private string Nombre_Archivo = string.Empty;
        Minic.Análisis_sintactico.Analis_LR_1_ Analis_LR_1_ = new Minic.Análisis_sintactico.Analis_LR_1_();
        Queue<Tuple<string, string>> Pila_Token = new Queue<Tuple<string, string>>();

        /// <summary>
        /// Metodo para poder leer el archivo que ingresa el usuriario
        /// </summary>
        /// <param name="path">direccion del archivo</param>
        public void LecturaArchivo(string path)
        {
            var lexemas = new List<Tuple<string, string>>();

            Nombre_Archivo = Path.GetFileNameWithoutExtension(path);

            var archivo = new StreamReader(path);
            IdentificadorLexemas(lexemas, archivo);
            Categorizacion(lexemas);
            archivo.Close();
            Analis_LR_1_.table(Pila_Token);
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
                regresar:
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
                                //dato += listaCaracteres[i].ToString();
                                if (dato.Length != 0)
                                {
                                    // porque puede quedar un " arriba entonces si llega hacer esto mete un espacio vacio
                                    if (dato.Length == 1)
                                    {
                                        if (dato != "\"")
                                        {
                                            var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                            dato = dato.Remove(0, dato.Length - 1);
                                            lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{(i) - (cadenaAux.Length - 1)}-{i},{contadorLinea}"));
                                        }
                                    }
                                    else
                                    {
                                        var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                        dato = dato.Remove(0, dato.Length - 1);
                                        lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{(i) - (cadenaAux.Length - 1)}-{i},{contadorLinea}"));
                                    }

                                }

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
                                            var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                            dato = dato.Remove(0, dato.Length - 1);
                                            lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{(i) - (cadenaAux.Length - 1)}-{i},{contadorLinea}"));
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{i + 1}-{i + dato.Length},{contadorLinea}"));
                                            dato = string.Empty;

                                        }
                                        else if ((listaCaracteres[i + 1] == 'E' || listaCaracteres[i + 1] == 'e') && notacionCientifica == false)
                                        {
                                            // ese cumple para el caso de que 1.e y activar el bool
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
                                            var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                            dato = dato.Remove(0, dato.Length - 1);
                                            lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{(i) - (cadenaAux.Length - 1)}-{i},{contadorLinea}"));
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{i + 1}-{i + dato.Length},{contadorLinea}"));
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
                                        else if (char.IsLetter(listaCaracteres[i + 1]))
                                        {
                                            var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                            dato = dato.Remove(0, dato.Length - 1);
                                            lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{(i) - (cadenaAux.Length - 1)}-{i},{contadorLinea}"));
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{i + 1}-{i + dato.Length},{contadorLinea}"));
                                            dato = string.Empty;
                                        }
                                    }
                                }
                                else
                                {
                                    if (dato.Length > 2)
                                    {
                                        var datoAnterior = Convert.ToChar(dato.Substring(dato.Length - 2, 1));
                                        if (char.IsDigit(datoAnterior))
                                        {
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                            dato = string.Empty;
                                        }
                                        else if (char.IsLetter(datoAnterior))
                                        {
                                            var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                            dato = dato.Remove(0, dato.Length - 1);
                                            lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{(i) - (cadenaAux.Length - 1)}-{i},{contadorLinea}"));
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{i + 1}-{i + dato.Length},{contadorLinea}"));
                                            dato = string.Empty;
                                        }

                                    }
                                    else
                                    {
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                        dato = string.Empty;

                                    }
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

                                        // este contador de columnas se le sumo dos por tomar el siguiente y guardarlo de una vez
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 2) - (dato.Length - 1)}-{i + 2}, {contadorLinea}"));

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
                                            break;
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
                                            // revisar esta asignacion de columna
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
                            else if (listaCaracteres[i] == '*')
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
                                            i++;
                                        }
                                    }
                                    else
                                    {
                                        if (comentarioMultiple == false)
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
                                        //45.45
                                        // dato 45.4
                                        if (char.IsDigit(listaCaracteres[i + 1]))
                                        {
                                            // para que se concaten los numeros si son doubles 
                                        }
                                        else if (listaCaracteres[i + 1] == 'E' || listaCaracteres[i + 1] == 'e')
                                        {
                                            // este es para el caso que se 1.4e45 se active el bool de notacion cientifica
                                            dato += listaCaracteres[i + 1].ToString();
                                            notacionCientifica = true;
                                            i++;
                                        }
                                        else
                                        {
                                            // esto es por si viene 45.45carro lo corta
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
                                    /// 
                                    var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                    dato = dato.Remove(0, dato.Length - 1);
                                    //
                                    lexemas_.Add(new Tuple<string, string>(cadenaAux, $"ok - "));
                                    validarDoubles = false;

                                }
                            }
                            // esto era para poder tomar
                            else if (validarDoubles == true && notacionCientifica == true)
                            {

                                if (i + 1 < listaCaracteres.Count())
                                {
                                    if (!char.IsDigit(listaCaracteres[i + 1]))
                                    {

                                        lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                        dato = string.Empty;
                                        validarDoubles = false;
                                        notacionCientifica = false;

                                    }
                                    else
                                    {
                                        i++;
                                        goto regresar;
                                    }

                                }
                                else
                                {
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                    dato = string.Empty;
                                    validarDoubles = false;
                                    notacionCientifica = false;
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
                                else if (!objExpreciones.letras_.IsMatch(listaCaracteres[i].ToString()) && stringEncontrado == false && comentarioMultiple == false && comentarioLinea == false && notacionCientifica == false && !objExpreciones.caracteres_.IsMatch(listaCaracteres[i].ToString()) && !char.IsDigit(listaCaracteres[i]) && listaCaracteres[i] != '_')
                                {
                                    // Este if sirve para validar cualquier caracter que no sea conocido en nuestra gramatica funciona si y solo si no es el ultimo caracter de la gramatica
                                    if (dato.Length > 1)
                                    {
                                        // a____?
                                        var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                        dato = dato.Remove(0, dato.Length - 1);
                                        lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{(i) - (cadenaAux.Length - 1)}-{i},{contadorLinea}"));
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{i + 1}-{i + dato.Length},{contadorLinea}"));
                                        dato = string.Empty;

                                    }
                                    else
                                    {
                                        //#
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                        dato = string.Empty;
                                    }

                                }

                            }
                            else if (stringEncontrado == false && comentarioLinea == false && comentarioMultiple == false)
                            {
                                if (!objExpreciones.letras_.IsMatch(listaCaracteres[i].ToString()) && stringEncontrado == false && comentarioMultiple == false &&
                                    comentarioLinea == false && notacionCientifica == false && !objExpreciones.caracteres_.IsMatch(listaCaracteres[i].ToString()) &&
                                    !char.IsDigit(listaCaracteres[i]) && listaCaracteres[i] != '_')
                                {
                                    // Este if sirve para validar cualquier caracter que no sea conocido en nuestra gramatica funciona si y solo si  es el ultimo caracter de la gramatica
                                    var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                    dato = dato.Remove(0, dato.Length - 1);
                                    lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{(i) - (cadenaAux.Length - 1)}-{i},{contadorLinea}"));
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{i + 1}-{i + dato.Length},{contadorLinea}"));
                                    dato = string.Empty;
                                }
                                else
                                {
                                    // este else if se quito en el commit donde se separaban caracteres diferentes de nuestro contexto. No recuerdo porque si no interifere con ese codigo.
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{(i + 1) - (dato.Length - 1)}-{i + 1}, {contadorLinea}"));
                                    dato = string.Empty;

                                }
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
                lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},EOF Cadena"));
                dato = string.Empty;
            }
            if (comentarioMultiple == true)
            {
                lexemas_.Add(new Tuple<string, string>("Ç", $"{contadorLinea},EOF Comentario"));
                dato = string.Empty;
            }

        }

        /// <summary>
        /// Metodo para podes categorizar los lexemas y poder convertirlos en tokens
        /// </summary>
        /// <param name="Lexema_">una lista de tuplas string string donde viene en el primero string el lexema en el segundo viene linea, columna y un error si lo existe separado por comas</param>
        public void Categorizacion(List<Tuple<string, string>> Lexema_)
        {
            var objExpreciones = new Expreciones();
            string CarpetaOut = Environment.CurrentDirectory;
            if (!Directory.Exists(Path.Combine(CarpetaOut, "Salida")))
            {
                Directory.CreateDirectory(Path.Combine(CarpetaOut, "Salida"));
            }
            else
            {
                if (File.Exists(Path.Combine(CarpetaOut, "Salida", $"{Nombre_Archivo}.out")))
                {
                    File.WriteAllText(Path.Combine(CarpetaOut, "Salida", $"{Nombre_Archivo}.out"), string.Empty);
                }
            }
            using (var writeStream = new FileStream(Path.Combine(CarpetaOut, "Salida", $"{Nombre_Archivo}.out"), FileMode.OpenOrCreate))
            {
                using (var write = new StreamWriter(writeStream))
                {
                    List<string> Errores = new List<string>();
                    foreach (var item in Lexema_)
                    {
                        if (item.Item1 == "Ç")
                        {
                            write.Write(" \n ");
                            var LC = item.Item2.Split(',');
                            if (LC.Contains(" Comentario sin emparejar"))
                            {
                                write.Write($"{LC[2]} encontrado en linea {LC[1]}");
                                Errores.Add($"Error \"{LC[2]}\" encontrado en linea {LC[1]}");
                            }
                            else
                            {
                                write.Write($"{LC[1]} encontrado en linea {LC[0]}");
                                Errores.Add($"Error \"EOF comentario \" encontrado en linea {LC[0]}");
                            }
                        }
                        else
                        {
                            var LC = item.Item2.Split(',');
                            string Categoria = Validar(item.Item1, objExpreciones);
                            var Mostrar_Categoria = Categoria.Split(',');
                            ///
                            if (Categoria == "Token unido")
                            {
                                var CN = LC[0].Split('-');
                                var dato_separado = Separar_caracter(item.Item1, objExpreciones, Convert.ToInt32(CN[0]));
                                if (!objExpreciones.entero_.IsMatch(dato_separado[0]))
                                {
                                    string Ncategiria = Validar(dato_separado[0], objExpreciones);
                                    var Mostrar_Ncategoria = Ncategiria.Split(',');
                                    string Scategiria = Validar(dato_separado[1], objExpreciones);
                                    var Mostrar_Scategoria = Scategiria.Split(',');
                                    write.Write(" \n ");
                                    Pila_Token.Enqueue(new Tuple<string, string>(Mostrar_Ncategoria[1], dato_separado[2] + "," + LC[1] + "," + dato_separado[1]));
                                    write.Write("{0}  Línea: {1} , columna: {2}  Categoria:  {3} \n", dato_separado[2], LC[1], dato_separado[1], Mostrar_Ncategoria[0]);
                                    write.Write(" \n ");
                                    Pila_Token.Enqueue(new Tuple<string, string>(Mostrar_Scategoria[1], dato_separado[0] + "," + LC[1] + "," + dato_separado[3]));
                                    write.Write("{0}  Línea: {1} , columna: {2}  Categoria:  {3} \n", dato_separado[0], LC[1], dato_separado[3], Mostrar_Scategoria[0]);
                                }
                                else
                                {
                                    string Ncategiria = Validar(dato_separado[0], objExpreciones);
                                    var Mostrar_Ncategoria = Ncategiria.Split(',');
                                    string Scategiria = Validar(dato_separado[1], objExpreciones);
                                    var Mostrar_Scategoria = Scategiria.Split(',');
                                    write.Write(" \n ");
                                    Pila_Token.Enqueue(new Tuple<string, string>(Mostrar_Scategoria[1], dato_separado[0] + "," + LC[1] + "," + dato_separado[3]));
                                    write.Write("{0}  Línea: {1} , columna: {2}  Categoria:  {3} \n", dato_separado[0], LC[1], dato_separado[3], Mostrar_Scategoria[0]);
                                    write.Write(" \n ");
                                    Pila_Token.Enqueue(new Tuple<string, string>(Mostrar_Ncategoria[1], dato_separado[2] + "," + LC[1] + "," + dato_separado[1]));
                                    write.Write("{0}  Línea: {1} , columna: {2}  Categoria:  {3} \n", dato_separado[2], LC[1], dato_separado[1], Mostrar_Ncategoria[0]);
                                }
                            }
                            else if (Categoria == "Token no encontrado")
                            {
                                if (LC.Contains("Error la cadena no se cerro") || LC.Contains(" Comentario sin emparejar"))
                                {
                                    Errores.Add($"Error Cadena/Comentario en linea {LC[1]}");
                                    write.Write(" \n ");
                                    write.Write("{0}  Línea: {1}, columna {2} {3} \n", item.Item1, LC[1], LC[0], LC[2]);
                                    write.Write("\n");
                                }
                                else
                                {
                                    Errores.Add($"Error encontrado en linea {LC[1]}");
                                    write.Write(" \n ");
                                    write.Write("{0}  Línea: {1} , columna: {2}  Categoria:  {3} \n", item.Item1, LC[1], LC[0], Categoria);
                                }
                            }
                            else if (LC.Contains("EOF Cadena"))
                            {
                                Errores.Add($"Error EOF Cadena en linea {LC[0]}");
                                write.Write(" \n ");
                                write.Write("{0}  Línea: {1}, Error {2} \n", item.Item1, LC[0], LC[1]);
                            }

                            else if (Categoria == "identificador mayor a 31 caracteres,identificador")
                            {
                                //31
                                var Cadena_anterior = item.Item1;
                                string Nueva_cadena = string.Empty;
                                for (int i = 0; i < 32; i++)
                                {
                                    Nueva_cadena += Cadena_anterior[i];
                                }
                                Errores.Add($"Error {Mostrar_Categoria[0]} Linea {LC[1]}");
                                write.Write(" \n ");
                                Pila_Token.Enqueue(new Tuple<string, string>(Mostrar_Categoria[1], Nueva_cadena + "," + LC[1] + "," + LC[0]));
                                write.Write("{0}  Línea: {1} , columna: {2}  {3} \n", Nueva_cadena, LC[1], LC[0], Mostrar_Categoria[0]);
                            }
                            else if (Categoria == "Error cadena contiene caracter nulo")
                            {
                                Errores.Add($"{Categoria} Linea {LC[1]}");
                                write.Write(" \n ");
                                write.Write("{0} \"/0/\" Línea: {1} , columna: {2} \n", Categoria, LC[1], LC[0]);
                            }
                            else
                            {
                                write.Write(" \n ");
                                Pila_Token.Enqueue(new Tuple<string, string>(Mostrar_Categoria[1].Replace('"', ','), item.Item1 + "," + LC[1] + "," + LC[0]));
                                write.Write("{0}  Línea: {1} , columna: {2}  Categoria:  {3} \n", item.Item1, LC[1], LC[0], Mostrar_Categoria[0]);
                            }
                        }
                    }
                    if (!Errores.Any())
                    {
                        Errores.Add("0 Errores detectados");
                        Mandar_mensaje(Errores, Nombre_Archivo);
                    }
                    else
                    {
                        Mandar_mensaje(Errores, Nombre_Archivo);
                    }

                    write.Close();
                }
            }
        }


        /// <summary>
        /// Metodo que valida con las expresiones regulares para ve en cual casa
        /// </summary>
        /// <param name="cadena">caracter o cadena a evaluar</param>
        /// <param name="objExpreciones_">el objeto de la clase expresiones</param>
        /// <returns>verdadero si caso con alguna y un int que seria el identificador con cual caso</returns>
        private string Validar(string cadena, Expreciones objExpreciones_)
        {

            if (objExpreciones_.palabrasReservadas_.IsMatch(cadena))
            {

                return ("Palabra_Reservada ->" + "\"" + cadena + "\"" + $",{cadena}");
            }
            else if (objExpreciones_.booleanas_.IsMatch(cadena))
            {
                return ("booleana,boolena");
            }
            else if (objExpreciones_.identificador_.IsMatch(cadena))
            {
                int Cantidad = Cantidad_identificador(cadena);
                if (Cantidad < 32)
                {
                    return ("ident,ident");
                }
                else
                {
                    return ("identificador mayor a 31 caracteres,identificador");
                }

            }
            else if (objExpreciones_.entero_.IsMatch(cadena))
            {
                return ($"entero (Valor ={cadena})" + ",entero");
            }
            else if (objExpreciones_.hexadecimal_.IsMatch(cadena))
            {
                return ("Hexadecimal,hexadecimal");
            }
            else if (objExpreciones_.doubles_.IsMatch(cadena))
            {
                return ($"doubles (Valor ={cadena})" + ",doubles");
            }
            else if (objExpreciones_.cadena_.IsMatch(cadena))
            {
                if (!objExpreciones_.cadenaNulo_.IsMatch(cadena))
                {
                    return ("Error cadena contiene caracter nulo");
                }
                else
                {
                    return ("cadena,cadena");
                }

            }
            else if (objExpreciones_.caracteresDobles_.IsMatch(cadena))
            {
                return ("\"" + cadena + "\"" + $",{cadena}");
            }
            else if (objExpreciones_.caracteresSimples_.IsMatch(cadena))
            {
                return ("\"" + cadena + "\"" + $",{cadena}");
            }
            else if (objExpreciones_.llavesSimples_.IsMatch(cadena))
            {
                return ("\"" + cadena + "\"" + $",{cadena}");
            }
            else if (objExpreciones_.llavesDobles_.IsMatch(cadena))
            {
                return ("\"" + cadena + "\"" + $",{cadena}");
            }
            else if (objExpreciones_.signosPuntuacion_.IsMatch(cadena))
            {
                return ("\"" + cadena + "\"" + $",{cadena}");
            }
            else
            {
                return (Caracter_unidos(cadena, objExpreciones_));
            }
        }

        /// <summary>
        /// Este metodo sirve si existe el caso que venga un entero junto con palabras para evaluar si es un entero con una palabra reserva o un identificar mal escritos
        /// </summary>
        /// <param name="caracter">el lexema a evluar</param>
        /// <param name="objExpreciones_">el objeto de la clase expresiones</param>
        /// <returns></returns>
        private string Caracter_unidos(string caracter, Expreciones objExpreciones_)
        {
            string Numero = string.Empty;
            string Letra = string.Empty;
            foreach (var item in caracter)
            {
                if (objExpreciones_.entero_.IsMatch(Convert.ToString(item)))
                {
                    Numero += item;
                }
                else if (objExpreciones_.identificador_.IsMatch(Convert.ToString(item)))
                {
                    Letra += item;
                }
                else
                {
                    return ("Token no encontrado");
                }
            }
            return ("Token unido");
        }

        /// <summary>
        /// Metodo para separar lexemas que vengan unido por ejemplo el 23this
        /// </summary>
        /// <param name="caracter">el lexema unido</param>
        /// <param name="objExpreciones_">el objeto de la clase expresiones</param>
        /// <param name="columna">la ultima columna de donde esta el lexema</param>
        /// <returns></returns>
        private string[] Separar_caracter(string caracter, Expreciones objExpreciones_, int columna)
        {
            string Numero = string.Empty;
            string Letra = string.Empty;
            int columna_auxiliar = columna;
            int columna_final = 0;
            foreach (var item in caracter)
            {
                if (objExpreciones_.entero_.IsMatch(Convert.ToString(item)))
                {
                    Numero += item;
                    columna_auxiliar++;
                }
                else if (objExpreciones_.identificador_.IsMatch(Convert.ToString(item)))
                {
                    Letra += item;
                    columna_final++;
                }
            }
            string dato = Numero + "," + $"{columna}-{columna_auxiliar}" + "," + Letra + "," + $"{columna_auxiliar}-{columna_auxiliar + columna_final}";
            var dato_separado = dato.Split(',');
            return (dato_separado);
        }

        /// <summary>
        /// Metodo que manda el mensaje que si todo esta correcto o no en la interfaz grafica
        /// </summary>
        /// <param name="mensaje">mensaje que se desea mostrar</param>
        public void Mandar_mensaje(List<string> mensaje, string direccion)
        {
            string M_mostrar = string.Empty;
            foreach (var item in mensaje)
            {
                M_mostrar += item + " \n ";
            }
            cargar_Archivo.Mostrar_mensaje(M_mostrar, direccion);
        }

        private int Cantidad_identificador(string identificador)
        {
            int Caracteres = 0;
            foreach (var item in identificador)
            {
                Caracteres++;
            }
            return (Caracteres);
        }

    }

}






