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
        GUI.Cargar_Archivo cargar_Archivo = new GUI.Cargar_Archivo();
        /// <summary>
        /// Metodo para poder leer el archivo que ingresa el usuriario
        /// </summary>
        /// <param name="path">direccion del archivo</param>
        public void LecturaArchivo(string path)
        {
            var lexemas = new List<Tuple<string, string>>();

            var archivo = new StreamReader(path);

            IdentificadorLexemas(lexemas, archivo);

            Categorizacion(lexemas);


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
            var contadorColumana = 1;
            var contadorAux = 1;
            var stringEncontrado = false;
            var comentarioLinea = false;
            var comentarioMultiple = false;
            var contadorLinea = 1;
            var validarDoubles = false;
            var notacionCientifica = false;

            var linea = archivo_.ReadLine();

            while (linea != null)
            {
                var listaCaracteres = linea.ToList();

                if (stringEncontrado == true)
                {
                    stringEncontrado = false;
                    lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana},Error la cadena no se cerro"));
                    dato = string.Empty;
                    contadorAux = contadorColumana + 1;
                }
                for (int i = 0; i < listaCaracteres.Count(); i++)
                {
                    if (listaCaracteres[i].ToString() == " " || listaCaracteres[i].ToString() == "\t")
                    {
                        if (stringEncontrado == true)
                        {
                            dato += listaCaracteres[i].ToString();
                        }
                        contadorAux = contadorColumana + 1;
                    }

                    else
                    {
                        // esto es para no separar los strings

                        dato += listaCaracteres[i].ToString();
                        if (listaCaracteres[i] == '"' && comentarioMultiple == false)
                        {
                            if (stringEncontrado == false)
                            {
                                stringEncontrado = true;
                                dato += listaCaracteres[i].ToString();
                                contadorAux = contadorColumana + 1;

                            }
                            else if (stringEncontrado == true)
                            {
                                stringEncontrado = false;
                                lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                dato = string.Empty;
                                contadorAux = contadorColumana + 1;

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
                                            contadorColumana++;

                                        }
                                        else if (char.IsDigit(listaCaracteres[i + 1]) && !char.IsDigit(datoAnterior))
                                        {
                                            dato += listaCaracteres[i + 1].ToString();
                                            validarDoubles = true;
                                            contadorColumana++;
                                            // verificar este +++
                                            i++;
                                            //contadorAux = contadorColumana + 1;
                                        }
                                        else if ((listaCaracteres[i + 1] == 'E' || listaCaracteres[i + 1] == 'e'))
                                        {
                                            dato += listaCaracteres[i + 1].ToString();
                                            validarDoubles = true;
                                            notacionCientifica = true;
                                            contadorColumana++;

                                            // verificar este +++
                                            i++;
                                            //contadorAux = contadorColumana + 1;
                                        }
                                        else if (notacionCientifica == true)
                                        {

                                            var datoAux = dato.Remove(dato.Length - 1, 1);
                                            dato = dato.Remove(0, dato.Length - 1);
                                            lexemas_.Add(new Tuple<string, string>(datoAux, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                            contadorAux = contadorColumana + 1;

                                        }
                                        else
                                        {

                                            lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                            dato = string.Empty;
                                            contadorAux = contadorColumana + 1;

                                        }
                                    }
                                    else
                                    {
                                        if (char.IsDigit(listaCaracteres[i + 1]))
                                        {
                                            dato += listaCaracteres[i + 1].ToString();
                                            validarDoubles = true;
                                            contadorColumana++;
                                            // verificar este +++
                                            i++;
                                            //contadorAux = contadorColumana + 1;

                                        }
                                        else if (listaCaracteres[i + 1] == 'E' || listaCaracteres[i + 1] == 'e')
                                        {
                                            dato += listaCaracteres[i + 1].ToString();
                                            validarDoubles = true;
                                            notacionCientifica = true;
                                            contadorColumana++;

                                            // verificar este +++
                                            i++;
                                            //contadorAux = contadorColumana + 1;
                                        }
                                    }
                                }
                                else
                                {
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                    dato = string.Empty;
                                    contadorAux = contadorColumana + 1;

                                }
                            }
                            else
                            {
                                var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                dato = dato.Remove(0, dato.Length - 1);
                                lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                dato = string.Empty;
                                contadorAux = contadorColumana + 1;

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
                                        contadorColumana++;
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                        dato = string.Empty;
                                        //verificar este ++
                                        i++;
                                        contadorAux = contadorColumana + 1;

                                    }
                                    else
                                    {
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                        dato = string.Empty;
                                        contadorAux = contadorColumana + 1;

                                    }
                                }
                                else
                                {
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                    dato = string.Empty;
                                    contadorAux = contadorColumana + 1;

                                }

                            }
                            else
                            {
                                lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                dato = string.Empty;
                                contadorAux = contadorColumana + 1;

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
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                            contadorAux = contadorColumana + 1;
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
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                            contadorAux = contadorColumana + 1;
                                            dato = string.Empty;

                                        }
                                        comentarioMultiple = true;
                                        // verificar este contador 
                                        i++;

                                    }
                                    else
                                    {
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                        dato = string.Empty;
                                        contadorAux = contadorColumana + 1;

                                    }
                                }
                                else
                                {
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                    dato = string.Empty;
                                    contadorAux = contadorColumana + 1;

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
                                            contadorColumana++;
                                            contadorAux = contadorColumana + 1;
                                            // verificar este contador 
                                            contadorColumana++;
                                            i++;

                                        }
                                        else
                                        {
                                            lexemas_.Add(new Tuple<string, string>("Ç", $"{contadorLinea},{contadorAux}-{contadorColumana},Comentario sin emparejar"));
                                            dato = string.Empty;
                                            contadorAux = contadorColumana + 1;

                                        }
                                    }



                                }


                            }
                            else
                            {
                                if (comentarioMultiple == false && comentarioLinea == false && notacionCientifica == false)
                                {
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                    dato = string.Empty;
                                    contadorAux = contadorColumana + 1;


                                }
                                else
                                {
                                    contadorAux = contadorColumana + 1;

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
                                            contadorColumana++;

                                        }
                                        else if (listaCaracteres[i + 1] == 'E' || listaCaracteres[i + 1] == 'e')
                                        {
                                            dato += listaCaracteres[i + 1].ToString();
                                            notacionCientifica = true;
                                            contadorColumana++;

                                            // verificar este +++
                                            i++;
                                        }
                                        else
                                        {
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                            dato = string.Empty;
                                            contadorAux = contadorColumana + 1;
                                            validarDoubles = false;

                                        }
                                    }
                                    else
                                    {
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                        dato = string.Empty;
                                        contadorAux = contadorColumana + 1;
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
                                            contadorColumana++;
                                            notacionCientifica = true;
                                            //lexemas_.Add(new Tuple<string, string>(dato, $"{contadorAux}-{contadorColumana}"));
                                            // verificar este +++
                                            i++;
                                            //contadorAux = contadorColumana + 1;

                                        }
                                        else
                                        {
                                            lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                            dato = string.Empty;
                                            contadorAux = contadorColumana + 1;
                                            validarDoubles = false;

                                        }
                                    }
                                    else
                                    {
                                        lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                        dato = string.Empty;
                                        contadorAux = contadorColumana + 1;
                                        validarDoubles = false;
                                    }
                                }
                                else
                                {
                                    var cadenaAux = dato.Remove(dato.Length - 1, 1);
                                    dato = dato.Remove(0, dato.Length - 1);
                                    lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                    contadorAux = contadorColumana + 1;
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
                                    lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                    dato = string.Empty;
                                    contadorAux = contadorColumana + 1;
                                }
                                else if (!objExpreciones.letras_.IsMatch(listaCaracteres[i].ToString()) && stringEncontrado == false && comentarioMultiple == false && comentarioLinea == false && notacionCientifica == false && !objExpreciones.caracteres_.IsMatch(listaCaracteres[i].ToString()) && !char.IsDigit(listaCaracteres[i]) && listaCaracteres[i] != '_')
                                {
                                    var cadenaAux = dato.Remove(0, dato.Length - 1);
                                    dato = dato.Remove(dato.Length - 1, 1);
                                    lexemas_.Add(new Tuple<string, string>(cadenaAux, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                                    contadorAux = contadorColumana + 1;

                                }

                            }
                        }
                    }
                    contadorColumana++;

                }
                if (validarDoubles == true)
                {
                    if (dato.Length != 0)
                    {
                        lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},{contadorAux}-{contadorColumana}"));
                        dato = string.Empty;

                    }
                    validarDoubles = false;
                    contadorAux = contadorColumana + 1;
                    contadorColumana++;
                }
                contadorLinea++;
                contadorColumana = 1;
                contadorAux = 1;
                linea = archivo_.ReadLine();
            }
            if (stringEncontrado == true)

            {
                lexemas_.Add(new Tuple<string, string>(dato, $"{contadorLinea},EOF Cadena"));
                dato = string.Empty;
                contadorAux = contadorColumana + 1;
                contadorColumana++;
            }
            if (comentarioMultiple == true)
            {
                lexemas_.Add(new Tuple<string, string>("Ç", $"{contadorLinea},EOF Comentario"));
                dato = string.Empty;
                contadorAux = contadorColumana + 1;
                contadorColumana++;
            }
        }

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
                File.WriteAllText(Path.Combine(CarpetaOut, "Salida", "Salida.out"), string.Empty);
            }
            using (var writeStream = new FileStream(Path.Combine(CarpetaOut, "Salida", "Salida.out"), FileMode.OpenOrCreate))
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
                            write.Write($"{LC[1]}");
                            Errores.Add($"Error \"EOF comentario \" encontrado en linea {LC[0]}");
                        }
                        else
                        {
                            var LC = item.Item2.Split(',');
                            string Categoria = Validar(item.Item1, objExpreciones);
                            ///
                            if (Categoria == "Token unido")
                            {
                                var CN = LC[1].Split('-');
                                var dato_separado = Separar_caracter(item.Item1,objExpreciones,Convert.ToInt32(CN[0]));
                                string Ncategiria = Validar(dato_separado[0], objExpreciones);
                                string Scategiria = Validar(dato_separado[1], objExpreciones);
                                write.Write(" \n ");
                                write.Write("{0}  Línea: {1} , columna: {2}  Categoria:  {3} \n",dato_separado[0],LC[0],dato_separado[1],Ncategiria);
                                write.Write(" \n ");
                                write.Write("{0}  Línea: {1} , columna: {2}  Categoria:  {3} \n", dato_separado[2], LC[0], dato_separado[3], Scategiria);
                            }
                            else if (Categoria == "Token no encontrado")
                            {
                                Errores.Add($"Error encontrado en linea {LC[0]}");
                                write.Write(" \n ");
                                write.Write("{0}  Línea: {1} , columna: {2}  Categoria:  {3} \n", item.Item1, LC[0], LC[1], Categoria);
                            }
                            else
                            { 
                            write.Write(" \n ");
                            write.Write("{0}  Línea: {1} , columna: {2}  Categoria:  {3} \n", item.Item1, LC[0], LC[1], Categoria);
                            }
                        }
                    }
                    if (!Errores.Any())
                    {
                        Errores.Add("0 Errores detectados");
                        Mandar_mensaje(Errores);
                    }
                    else
                    {
                        Mandar_mensaje(Errores);
                    }

                    write.Close();
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
        public string Validar(string cadena, Expreciones objExpreciones_)
        {

            if (objExpreciones_.palabrasReservadas_.IsMatch(cadena))
            {
                return ("Palabra_Reservada ->" + "\"" + cadena + "\"");
            }
            else if (objExpreciones_.booleanas_.IsMatch(cadena))
            {
                return ("booleana");
            }
            else if (objExpreciones_.identificador_.IsMatch(cadena))
            {
                return ("identificador");
            }
            else if (objExpreciones_.entero_.IsMatch(cadena))
            {
                return ($"entero (Valor ={cadena})");
            }
            else if (objExpreciones_.hexadecimal_.IsMatch(cadena))
            {
                return ("Hexadecimal");
            }
            else if (objExpreciones_.doubles_.IsMatch(cadena))
            {
                return ("dobles");
            }
            else if (objExpreciones_.cadena_.IsMatch(cadena))
            {
                return ("cadena");
            }
            else if (objExpreciones_.caracteresDobles_.IsMatch(cadena))
            {
                return ("\"" + cadena + "\"");
            }
            else if (objExpreciones_.caracteresSimples_.IsMatch(cadena))
            {
                return ("\"" + cadena + "\"");
            }
            else if (objExpreciones_.llavesSimples_.IsMatch(cadena))
            {
                return ("\"" + cadena + "\"");
            }
            else if (objExpreciones_.llavesDobles_.IsMatch(cadena))
            {
                return ("\"" + cadena + "\"");
            }
            else if (objExpreciones_.signosPuntuacion_.IsMatch(cadena))
            {
                return ("\"" + cadena + "\"");
            }
            else
            {
                return (Caracter_unidos(cadena, objExpreciones_));
            }
        }
        public string Caracter_unidos(string caracter, Expreciones objExpreciones_)
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
        public string[] Separar_caracter(string caracter, Expreciones objExpreciones_, int columna) 
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
            string dato = Numero + ","+$"{columna}-{columna_auxiliar}"+ ","+  Letra+","+$"{columna_auxiliar}-{columna_auxiliar+columna_final}";
            var dato_separado = dato.Split(',');
            return (dato_separado);
        }
        public void Mandar_mensaje(List<string> mensaje)
        {
            

            string M_mostrar = string.Empty;
            foreach (var item in mensaje)
            {            
              M_mostrar += item + " \n ";
            }
            cargar_Archivo.Mostrar_mensaje(M_mostrar);
        }
    }

}




