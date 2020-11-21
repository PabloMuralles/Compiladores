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

        private Regex comparations = new Regex(@"^(>|<|<=|>=|==|!=)$");

        private List<string> mistakes = new List<string>();

        private int positionList = 0;

        Cargar_Archivo Cargar_Archivo = new Cargar_Archivo();

        private Stack<string> ambit = new Stack<string>();

        private bool function = false;

        private bool classe = false;

        private bool general = true;

        private int finishClass = 0;

        private int finishFunction = 0;



        /// <summary>
        /// Cosntructor of the clase
        /// </summary>
        /// <param name="tokens_">list of the tokens to analysis</param>
        public Analysis(List<Tuple<string, string>> tokens_)
        {
            listTokens = tokens_;
            IdentifyIdent();
            Exit_Analyze();
        }
        /// <summary>
        /// Method to identify the ident that have the list
        /// </summary>
        private void IdentifyIdent()
        {
            ambit.Push("general");
            for (positionList = 0; positionList < listTokens.Count; positionList++)
            {
                var token = listTokens[positionList];
                if (token.Item1 == "ident")
                {
                    ClassifyIdent();
                }
                else if (comparations.IsMatch(token.Item2))
                {
                    ValidateType();
                }
                else if (positionList == finishFunction && function == true)
                {
                    ambit.Pop();
                    function = false;
                }

            }

            Metodo_escritura();
        }
        /// <summary>
        /// Method to classify the indent that hace the list of tokens
        /// </summary>
        private void ClassifyIdent()
        {
            var dataListNext = listTokens[positionList + 1];
            var dataListpreviously = listTokens[positionList - 1];
            var dataListActual = listTokens[positionList];
            //si no esta declara 
            // declarar: tipo, nombre

            //declarcion de variaable
            if (dataListNext.Item1 == ";")
            {
                var Name = Split_Name(dataListActual.Item2);
                if (!ExistInTable(Name, ambit.Peek()))
                {
                    SimbolsTable.Add(new TableElement { name = Name, value = null, type = dataListpreviously.Item1, ambit = ambit.Peek(), isClass = false, isFunction = false });
                }
                else
                {
                    mistakes.Add($"Error la variable :{Name} ya fue definida con anterioridad");
                }
            }
            //validar si es una clase
            else if (dataListpreviously.Item1 == "class")
            {

                var Name = Split_Name(dataListActual.Item2);
                ambit.Push(Name);
                if (!ExistInTable(dataListActual.Item1, ambit.Peek()))
                {
                    SimbolsTable.Add(new TableElement { name = Name, value = null, type = dataListpreviously.Item1, ambit = null, isClass = true, isFunction = false });
                }
                else
                {
                    mistakes.Add($"Error la variable :{Name} ya fue definida con anterioridad");
                }
            }
            //asignacion de una variable
            else if (dataListNext.Item1 == "=")
            {
                var Name = Split_Name(dataListActual.Item2);
                if (ExistInTable(Name, ambit.Peek()))
                {
                    // obtener el valor del dato
                    var index = SimbolsTable.FindIndex(c => c.name == Name);
                    var Type = SimbolsTable[index].type;
                    var value_ = defination_value(positionList + 2, Type);
                    if (value_ != "")
                    {
                        SimbolsTable[index] = new TableElement { name = Name, value = value_, type = Type, ambit = ambit.Peek(), isClass = true, isFunction = false };
                    }
                }
                else
                {
                    mistakes.Add($"Error la variable :{Name} no fue definida con anterioridad");
                }
            }
            //creacion de una funcion o precedimiento
            else if (dataListNext.Item1 == "(")
            {
                var Name = Split_Name(dataListActual.Item2);
                function = true;
                var tempAmbit = ambit.Peek() + Name;
                ambit.Push(tempAmbit);
                SearchEndFunction();
                if (!ExistInTable(Name, ambit.Peek()))
                {
                    SimbolsTable.Add(new TableElement { name = Name, value = null, type = dataListpreviously.Item1, ambit = ambit.Peek(), isClass = false, isFunction = true });

                    positionList++;
                    positionList++;
                    var tempData = listTokens[positionList];
                    var tempParameters = string.Empty;
                    while (tempData.Item1 != ")")
                    {
                        if (tempData.Item1 == "ident")
                        {
                            tempParameters += split_value(tempData.Item2) + " ";
                        }
                        else
                        {
                            tempParameters += tempData.Item1 + " ";
                        }


                        positionList++;
                        tempData = listTokens[positionList];
                    }

                    var tempSplit = tempParameters.Split(',');

                    foreach (var parameter in tempSplit)
                    {
                        var splitParameter = parameter.Trim().Split(' ');
                        if (!ExistInTable(splitParameter[1], ambit.Peek()))
                        {
                            SimbolsTable.Add(new TableElement { name = splitParameter[1], value = null, type = splitParameter[0], ambit = ambit.Peek(), isClass = false, isFunction = false });
                        }
                    }

                }
                else
                {
                    if (!ExistInTable(Split_Name(Name), ambit.Peek()))
                        mistakes.Add($"Error la Funcion :{Name} ya fue declarada con anterioridad");
                }

            }

        }

        /// <summary>
        /// method to find the position when the funciton or void declaration finish
        /// </summary>
        private void SearchEndFunction()
        {
            int i = 0;
            int count = 0;
            for (i = positionList; i < listTokens.Count(); i++)
            {
                var tempData = listTokens[i];
                if (tempData.Item1 == "{")
                {
                    count++;
                }
                if (tempData.Item1 == "}")
                {
                    count++;
                    if (count % 2 == 0)
                    {
                        finishFunction = i;
                    }
                }
            }
        }

        /// <summary>
        /// Metodo para validar los casos que se hagan comparaciones con operadores logicos se valida que sean ambos del mismo tipo
        /// </summary>
        private void ValidateType()
        {
            var dataListNext = listTokens[positionList + 1];
            var dataListpreviously = listTokens[positionList - 1];
            var dataListActuals = listTokens[positionList];

            var nameVariable1 = Split_Name(dataListpreviously.Item2);
            var nameVariable2 = Split_Name(dataListpreviously.Item2);
            var variable1 = SearchInTable(nameVariable1);
            var variable2 = SearchInTable(Split_Name(nameVariable2));


            if (ExistInTable(nameVariable1,ambit.Peek()) && ExistInTable(Split_Name(nameVariable1),))
            {
                if (!(variable1.type == variable2.type))
                {
                    mistakes.Add($"No se puede realziar la comparacion logica:{nameVariable1} y {nameVariable1} no son del mismo tipo");
                }
            }
            else
            {
                mistakes.Add($"No se puede realziar la comparacion logica:{nameVariable1} y {nameVariable1} no estan definidas con anterioridad");
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

        /// <summary>
        /// Method to validate if the assignments of a variable are correct
        /// </summary>
        /// <param name="position">the position after the equal</param>
        /// <param name="Type">The type of the variable that is being assigned </param>
        /// <returns>the result of the assigned</returns>
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
                            resultado = date_value;
                            return resultado;
                        }
                        else
                        {
                            if (!listTokens[position].Item1.Contains("stringConstant"))
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
                                    if (ExistInTable(date_value, ambit.Peek()))
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
                                            mistakes.Add($"No coinciden en terminos");
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
                                mistakes.Add($"Valor incorrecto declarado tipo {Type}");
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

        /// <summary>
        /// Method to get the name of the ident of the string
        /// </summary>
        /// <param name="value"></param>
        /// <returns>the name of the ident</returns>
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
        private bool ExistInTable(string name, string ambit)
        {
            foreach (var item in SimbolsTable)
            {
                if (item.name == name && item.ambit == ambit)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Method to write the simbols table
        /// </summary>
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
                        write.Write("Type: " + item.type + " " + "Name:" + item.name + " " + "Value:  " + item.value);
                        write.Write(" \n ");
                    }
                    write.Close();
                }
            }
        }
        /// <summary>
        /// Method to show the errors to the user
        /// </summary>
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
