﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MiniCompilador.GUI;

namespace Minic.Análisis_Semantico
{
    class Analysis
    {
        private List<TableElement> SimbolsTable = new List<TableElement>();

        private List<Tuple<string, string>> listTokens = new List<Tuple<string, string>>();

        private Regex comparations = new Regex(@"^(<=|>=|==|!=)$");

        private List<string> mistakes = new List<string>();

        private string cordenadas;
        private int positionList = 0;
        Cargar_Archivo Cargar_Archivo = new Cargar_Archivo();
        public Analysis(List<Tuple<string, string>> tokens_)
        {
            listTokens = tokens_;
            IdentifyIdent();
            Exit_Analyze();
        }

        private void IdentifyIdent()
        {

            foreach (var token in listTokens)
            {

                if (token.Item1 == "ident")
                {
                    ClassifyIdent();
                }
                else if (comparations.IsMatch(token.Item2))
                {
                    ValidateType();

                }
                positionList++;
            }
            Metodo_escritura();
        }

        private void ClassifyIdent()
        {
            var dataListNext = listTokens[positionList + 1];
            var dataListpreviously = listTokens[positionList - 1];
            var dataListActual = listTokens[positionList];
            cordenadas =  Obtener_linea(dataListActual.Item2);
            //si no esta declara 
            // declarar: tipo, nombre

            //declarcion de variaable
            if (dataListNext.Item1 == ";")
            {
                //llamar a la funcion que hace el split return nombre

                var Name = Split_Name(dataListActual.Item2);
                if (!ExistInTable(Name))
                {
                    SimbolsTable.Add(new TableElement { name = Name, value = null, type = dataListpreviously.Item1, ambit = null, isClass = false, isFunction = false });
                }
                else
                {
                    mistakes.Add($"Error la variable :{Name} ya fue definida con anterioridad  {cordenadas}");
                }
            }
            //validar si es una clase
            else if (dataListpreviously.Item1 == "class")
            {
                var Name = Split_Name(dataListActual.Item2);
                if (!ExistInTable(dataListActual.Item1))
                {
                    SimbolsTable.Add(new TableElement { name = Name, value = null, type = dataListpreviously.Item1, ambit = null, isClass = true, isFunction = false });
                }
                else
                {
                    mistakes.Add($"Error la variable :{Name} ya fue definida con anterioridad  {cordenadas}");
                }
            }
            //asignacion de una variable
            else if (dataListNext.Item1 == "=")
            {
                var Name = Split_Name(dataListActual.Item2);
                if (ExistInTable(Name))
                {
                    // obtener el valor del dato
                    var index = SimbolsTable.FindIndex(c => c.name == Name);
                    var Type = SimbolsTable[index].type;
                    var value_ = defination_value(positionList + 2, Type);
                    if (value_ != "")
                    {
                        SimbolsTable[index] = new TableElement { name = Name, value = value_, type = Type, ambit = null, isClass = true, isFunction = false };
                    }
                }
                else
                {
                    mistakes.Add($"Error la variable :{Name} no fue definida con anterioridad  {cordenadas}");
                }
            }
            //creacion de una funcion o precedimiento
            else if (dataListNext.Item1 == "(")
            {
                var Name = Split_Name(dataListActual.Item2);
                if (!ExistInTable(Name))
                {
                    SimbolsTable.Add(new TableElement { name = Name, value = null, type = dataListpreviously.Item1, ambit = null, isClass = false, isFunction = true });
                }
                else
                {
                    if (!ExistInTable(Split_Name(Name)))
                        mistakes.Add($"Error la Funcion :{Name} ya fue declarada con anterioridad  {cordenadas}");
                }

            }
            else if (dataListNext.Item1 == ")" || dataListNext.Item1 == ",")
            {

            }
        }

        private void ValidateType()
        {
            var dataListNext = listTokens[positionList + 1];
            var dataListpreviously = listTokens[positionList - 1];
            var dataListActuals = listTokens[positionList];

            var nameVariable1 = Split_Name(dataListpreviously.Item2);
            var nameVariable2 = Split_Name(dataListpreviously.Item2);
            var variable1 = SearchInTable(nameVariable1);
            var variable2 = SearchInTable(Split_Name(nameVariable2));


            if (ExistInTable(nameVariable1) && ExistInTable(Split_Name(nameVariable1)))
            {
                if (!(variable1.type == variable2.type))
                {
                    mistakes.Add($"No se puede realziar la comparacion logica:{nameVariable1} y {nameVariable1} no son del mismo tipo  {cordenadas}");
                }
            }
            else
            {
                mistakes.Add($"No se puede realziar la comparacion logica:{nameVariable1} y {nameVariable1} no estan definidas con anterioridad {cordenadas}");
            }

        }

        private TableElement SearchInTable(string name)
        {
            foreach (var item in SimbolsTable)
            {
                if (item.name == name)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// function no know what is the name of the ident
        /// </summary>
        /// <param name="Name">Data data have de name, line and columns</param>
        /// <returns>The name of the indent</returns>
        private string Split_Name(string Name)
        {
            var Name_ = Name.Split(',');
            return Name_[2];
        }

        private string defination_value(int position, string Type)
        {
            var resultado_int = 0;
            var resultado_double = 0.0;
            var resultado_string = "";
            var resultado_ident = "";
            var resultado_bool = true;
            var resultado = "";
            while (listTokens[position].Item1 != ";")
            {
                var date_value = split_value(listTokens[position].Item2);
                switch (Type)
                {
                    case "int":
                        if (date_value == "New")
                        {
                            mistakes.Add($"Valor incorrecto declarado tipo New  {cordenadas}");
                            return resultado;
                        }
                        else
                        {
                            if (!listTokens[position].Item1.Contains("stringConstant") || !listTokens[position].Item1.Contains("boolConstant")
                                || !listTokens[position].Item1.Contains("doubleConstant"))
                            {

                                if (date_value == "\"+\"")
                                {
                                    resultado_int += Convert.ToInt32(split_value(listTokens[position + 1].Item2));
                                    return Convert.ToString(resultado_int);
                                }
                                else if (date_value == "\"%\"")
                                {
                                    resultado_int %= Convert.ToInt32(split_value(listTokens[position + 1].Item2));
                                    return Convert.ToString(resultado_int);
                                }
                                else if (date_value == "\"*\"")
                                {
                                    resultado_int *= Convert.ToInt32(split_value(listTokens[position + 1].Item2));
                                    return Convert.ToString(resultado_int);
                                }
                                else if (date_value == "\"-\"")
                                {
                                    resultado_int -= Convert.ToInt32(split_value(listTokens[position + 1].Item2));
                                    return Convert.ToString(resultado_int);
                                }
                                else
                                {
                                    // validar si el valor esta contenido en otro lado
                                    if (ExistInTable(date_value))
                                    {
                                        var index = SimbolsTable.FindIndex(c => c.name == date_value);
                                        var date_value_ = SimbolsTable[index].value;
                                        var date_type_ = SimbolsTable[index].type;
                                        if (date_value_ != null)
                                        {
                                            resultado_int = Convert.ToInt32(date_value_);
                                        }
                                        if (Type != date_type_)
                                        {
                                            mistakes.Add($"No coinciden en terminos  {cordenadas}");
                                            return "";
                                        }
                                        else
                                        {
                                            return "";
                                        }
                                    }
                                    else
                                    {
                                        resultado_int = Convert.ToInt32(date_value);
                                    }
                                }
                            }
                            else
                            {
                                mistakes.Add($"Valor incorrecto declarado tipo {Type}  {cordenadas}");
                                return "";
                            }
                        }

                        break;
                    case "string":
                        if (date_value == "New")
                        {
                            resultado = date_value;
                        }
                        resultado_string = date_value;
                        break;
                    case "bool":
                        if (date_value == "New")
                        {
                            resultado = date_value;
                        }
                        else if (date_value.Contains("("))
                        {
                            resultado_bool = Convert.ToBoolean(true);
                            return Convert.ToString(resultado_bool);
                        }
                        else
                        {
                            resultado_bool = Convert.ToBoolean(date_value);
                        }
                        break;
                    case "double":
                        if (date_value == "New")
                        {
                            resultado = date_value;
                        }
                        resultado_double += Convert.ToDouble(date_value);
                        break;
                    case "ident":
                        if (date_value == "New")
                        {
                            resultado = date_value;
                        }
                        resultado_ident = date_value;
                        break;
                    default:
                        break;
                }
                
                position++;
            }
            if (Type == "int")
            {
                resultado = Convert.ToString(resultado_int);
            }
            else if (Type == "string")
            {
                resultado = resultado_string;
            }
            else if (Type == "bool")
            {
                resultado = Convert.ToString(resultado_bool);
            }
            else if (Type == "double")
            {
                resultado = Convert.ToString(resultado_double);
            }
            return resultado;
        }

        private string split_value(string value)
        {
            var value_ = value.Split(',');
            if (value_[2] == "Palabra_Reservada ->\"New\"")
            {
                return "New";
            }
            else
            {
                return value_[2];
            }
        }

        /// <summary>
        /// function to verify is the ident already exist in the table
        /// </summary>
        /// <param name="name">the name of the ident to search in the list of the table</param>
        /// <returns>true if exist and false if not</returns>
        private bool ExistInTable(string name)
        {
            foreach (var item in SimbolsTable)
            {
                if (item.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public void Metodo_escritura()
        {
            string CarpetaOut = Environment.CurrentDirectory;
            if (!Directory.Exists(Path.Combine(CarpetaOut, "Salida")))
            {
                Directory.CreateDirectory(Path.Combine(CarpetaOut, "Salida"));
            }
            else
            {
                if (File.Exists(Path.Combine(CarpetaOut, "Salida", "Analisis_semantico.out")))
                {
                    File.WriteAllText(Path.Combine(CarpetaOut, "Salida", "Analisis_semantico.out"), string.Empty);
                }
            }
            using (var writeStream = new FileStream(Path.Combine(CarpetaOut, "Salida", "Analisis_semantico.out"), FileMode.OpenOrCreate))
            {
                using (var write = new StreamWriter(writeStream))
                {
                    foreach (var item in SimbolsTable)
                    {
                        write.Write("Type: "+ item.type + " " + "Name:" + item.name + " " +  "Value:  "+ item.value );
                        write.Write(" \n ");
                    }
                    write.Close();
                }
            }
        }
        private string Obtener_linea(string cordenada)
        {
            var resultado = "";
            var dato_ = cordenada.Split(',');
            resultado = $"linea: {dato_[0]} Columna: {dato_[1]}";
            return resultado;
        }
        private void Exit_Analyze()
        {
            string msg_Analyze = string.Empty;
            foreach (var item in mistakes)
            {
                msg_Analyze += item + " \n ";
            }
            Cargar_Archivo.msg_Analyze_semantic(msg_Analyze);
        }
    }
}
