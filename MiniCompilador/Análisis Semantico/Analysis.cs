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

        private string cordenadas;

        private int positionList = 0;

        Cargar_Archivo Cargar_Archivo = new Cargar_Archivo();

        private Stack<string> ambit = new Stack<string>();

        private List<string> ambitsHistory = new List<string>();

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
            if (!mistakes.Any())
            {
                mistakes.Add("Success");
                Exit_Analyze();
            }
            else
            {
                Exit_Analyze();
            }
          

        }
        /// <summary>
        /// Method to identify the ident that have the list
        /// </summary>
        private void IdentifyIdent()
        {
            ambit.Push("general");
            ambitsHistory.Add("general");
            for (positionList = 0; positionList < listTokens.Count; positionList++)
            {
                var token = listTokens[positionList];
                if (token.Item1 == "ident")
                {
                    ClassifyIdent();
                }
                else if (comparations.IsMatch(token.Item1))
                {
                    ValidateType();
                }
                else if (positionList == finishFunction && function == true)
                {
                    ambit.Pop();
                    function = false;
                    finishFunction = 0;
                }
                else if(token.Item1 == "return" && function == true)
                {
                    ValidateReturn();
                }

            }

            Metodo_escritura();
        }

        private void ValidateReturn()
        {
            var tempAmbit = ambit.Peek();
            TableElement temFunction;

            foreach (var item in SimbolsTable)
            {
                if (item.ambit == tempAmbit && item.isFunction)
                {
                    temFunction = item;
                    var index = SimbolsTable.FindIndex(c => c.name == item.name);
                    var Type = SimbolsTable[index].type;
                    var value = ReturnValue(positionList + 2, temFunction.type);
                    if (value != "")
                    {
                        SimbolsTable[index] = new TableElement { name = item.name, value = value, type = Type, ambit = ambit.Peek(), isClass = true, isFunction = false };
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Method to classify the indent that hace the list of tokens
        /// </summary>
        private void ClassifyIdent()
        {
            var dataListNext = listTokens[positionList + 1];
            var dataListpreviously = listTokens[positionList - 1];
            var dataListActual = listTokens[positionList];
            cordenadas = Obtener_linea(dataListActual.Item2);
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
                    // revisar que pasa cuando viene un error
                    if (dataListpreviously.Item1 == "int" || dataListpreviously.Item1 == "bool" || dataListpreviously.Item1 == "string" || dataListpreviously.Item1 == "double")
                    {
                        mistakes.Add($"Error la variable :{Name} ya fue definida con anterioridad  {cordenadas}");
                    }
                }
            }
            //validar si es una clase
            else if (dataListpreviously.Item1 == "class")
            {

                var Name = Split_Name(dataListActual.Item2);
                ambit.Push(Name);
                ambitsHistory.Add(Name);
                if (!ExistInTable(dataListActual.Item1, ambit.Peek()))
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
                if (ExistInTableAssignation(Name))
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
                    mistakes.Add($"Error la variable :{Name} no fue definida con anterioridad  {cordenadas}");
                }
            }
            //creacion de una funcion o precedimiento, pueda entrar a1.f1()
            else if (dataListNext.Item1 == "(" && (dataListpreviously.Item1 == "void" || dataListpreviously.Item1 == "int" || dataListpreviously.Item1 == "double" || dataListpreviously.Item1 == "bool" || dataListpreviously.Item1 == "string"))
            {
                var Name = Split_Name(dataListActual.Item2);
                function = true;
                var tempAmbit = ambit.Peek() + Name;
                ambit.Push(tempAmbit);
                ambitsHistory.Add(tempAmbit);
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
 
                    if (!ExistInTable(Split_Name(Name),ambit.Peek()))
                        mistakes.Add($"Error la Funcion :{Name} ya fue declarada con anterioridad  {cordenadas}");
 
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
                        break;
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
            var nameVariable2 = Split_Name(dataListNext.Item2);
             
            if (ExistInTableAssignation(nameVariable1) && ExistInTableAssignation(nameVariable2))
            {
                var variable1 = SearchInTable(nameVariable1);
                var variable2 = SearchInTable(nameVariable2);

                if (dataListActuals.Item1 == "==")
                {
                    if (!(variable1.type == variable2.type))
                    {
                        if ((variable1.type == "string" && (variable2.type == "int"|| variable2.type == "double")) || (variable2.type == "string" && (variable1.type == "int" || variable1.type == "double")))
                        {
                             mistakes.Add($"No se puede realziar la comparacion logica:{nameVariable1} y {nameVariable1} no son del mismo tipo  {cordenadas}");
                        }
                        else if ((variable1.type == "bool" && (variable2.type == "int" || variable2.type == "double")) || (variable2.type == "bool" && (variable1.type == "int" || variable1.type == "double")))
                        {
                            mistakes.Add($"No se puede realziar la comparacion logica:{nameVariable1} y {nameVariable1} no son del mismo tipo  {cordenadas}");
                        }
                        else if ((variable1.type == "bool" && variable2.type == "string") || (variable2.type == "string" && variable1.type == "bool"))
                        {
                            mistakes.Add($"No se puede realziar la comparacion logica:{nameVariable1} y {nameVariable1} no son del mismo tipo  {cordenadas}");
                        }

                    }
                     
                }
                else
                {
                    if (!((variable1.type == "double" && variable2.type == "int") || (variable2.type == "double" && variable1.type == "int") || (variable1.type == variable2.type)))
                    {
                        mistakes.Add($"No se puede realziar la comparacion logica:{nameVariable1} y {nameVariable1} no son del mismo tipo aceptado {cordenadas}");
                    }
                }

            }
            else
            {
                ///valirdar cuando venga alguna coonstantete
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
                            mistakes.Add($"Valor incorrecto declarado tipo New  {cordenadas}");
                            return resultado;
                        }
                        else
                        {
                            if (!listTokens[position].Item1.Contains("stringConstant") && !listTokens[position].Item1.Contains("boolConstant")
                                && !listTokens[position].Item1.Contains("doubleConstant"))
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
                        if (!listTokens[position].Item1.Contains("doubleConstant") && !listTokens[position].Item1.Contains("boolConstant")
                                && !listTokens[position].Item1.Contains("intConstant"))
                        {
                            if (date_value == "\"+\"")
                            {
                                resultado_string += split_value(listTokens[position + 1].Item2).Replace('"', ' ');
                                return resultado_string;
                            }
                            else
                            {
                                if (ExistInTable(date_value,ambit.Peek()))
                                {
                                    var index = SimbolsTable.FindIndex(c => c.name == date_value);
                                    var date_value_ = SimbolsTable[index].value;
                                    var date_type_ = SimbolsTable[index].type;
                                    if (date_value_ != null)
                                    {
                                        resultado_string = date_value_.Replace('"', ' ');
                                    }
                                    if (Type != date_type_)
                                    {
                                        mistakes.Add($"No coinciden en terminos  {cordenadas}");
                                        return "";
                                    }                               
                                }
                                else
                                {
                                    resultado_string = date_value.Replace('"', ' ');
                                }

                            }
                        }
                        else
                        {
                            mistakes.Add($"Valor incorrecto declarado tipo {Type}  {cordenadas}");
                            return "";
                        }
                        break;
                    case "bool":
                        if (date_value.Contains("("))
                        {
                            resultado_bool = Convert.ToBoolean(true);
                            return Convert.ToString(resultado_bool);
                        }

                        else if (ExistInTable(date_value,ambit.Peek()))
                        {
                            var index = SimbolsTable.FindIndex(c => c.name == date_value);
                            var date_value_ = SimbolsTable[index].value;
                            var date_type_ = SimbolsTable[index].type;
                            if (date_value_ != null)
                            {
                                resultado_bool = Convert.ToBoolean(date_value_);
                            }
                            if (Type != date_type_)
                            {
                                mistakes.Add($"No coinciden en terminos  {cordenadas}");
                                return "";
                            }                 
                        }
                        else
                        {
                            if (date_value.Contains("true") || date_value.Contains("false"))
                            {
                                resultado_bool = Convert.ToBoolean(date_value);
                            }
                            else
                            {
                                mistakes.Add($"Valor incorrecto declarado tipo {Type}  {cordenadas}");
                                return "";
                            }
                        }
                        break;
                    case "double":
                        if (date_value == "New")
                        {
                            mistakes.Add($"Valor incorrecto declarado tipo New  {cordenadas}");
                            return resultado;
                        }
                        else
                        {
                            if (!listTokens[position].Item1.Contains("stringConstant") && !listTokens[position].Item1.Contains("boolConstant"))
                            {

                                if (date_value == "\"+\"")
                                {
                                    resultado_double += Convert.ToDouble(split_value(listTokens[position + 1].Item2));
                                    return Convert.ToString(resultado_double);
                                }
                                else if (date_value == "\"%\"")
                                {
                                    resultado_double %= Convert.ToDouble(split_value(listTokens[position + 1].Item2));
                                    return Convert.ToString(resultado_double);
                                }
                                else if (date_value == "\"*\"")
                                {
                                    resultado_double *= Convert.ToDouble(split_value(listTokens[position + 1].Item2));
                                    return Convert.ToString(resultado_double);
                                }
                                else if (date_value == "\"-\"")
                                {
                                    resultado_double -= Convert.ToDouble(split_value(listTokens[position + 1].Item2));
                                    return Convert.ToString(resultado_double);
                                }
                                else
                                {
                                    // validar si el valor esta contenido en otro lado
                                    if (ExistInTable(date_value,ambit.Peek()))
                                    {
                                        var index = SimbolsTable.FindIndex(c => c.name == date_value);
                                        var date_value_ = SimbolsTable[index].value;
                                        var date_type_ = SimbolsTable[index].type;
                                        if (date_value_ != null)
                                        {
                                            resultado_double = Convert.ToDouble(date_value_);
                                        }
                                        if (Type != date_type_)
                                        {
                                            mistakes.Add($"No coinciden en terminos  {cordenadas}");
                                            return "";
                                        }                                                                             
                                    }
                                    else
                                    {
                                        resultado_double = Convert.ToDouble(date_value);
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
                    case "ident":

                        if (date_value == "New")
                        {
                            resultado = date_value;
                        }
                        if (!listTokens[position].Item1.Contains("doubleConstant") && !listTokens[position].Item1.Contains("boolConstant")
                                && !listTokens[position].Item1.Contains("intConstant"))
                        {
                            if (date_value == "\"+\"")
                            {
                                resultado_ident += split_value(listTokens[position + 1].Item2).Replace('"', ' ');
                                return resultado_ident;
                            }
                            else
                            {
                                if (ExistInTable(date_value,ambit.Peek()))
                                {
                                    var index = SimbolsTable.FindIndex(c => c.name == date_value);
                                    var date_value_ = SimbolsTable[index].value;
                                    var date_type_ = SimbolsTable[index].type;
                                    if (date_value_ != null)
                                    {
                                        resultado_ident = date_value_.Replace('"', ' ');
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
                                    resultado_ident = date_value.Replace('"', ' ');
                                }
                            }
                        }
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
        /// <param name="ambit">the actual ambit</param>
        /// <returns></returns>
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
        /// function to verify is the ident already exist in the table and in all ambits
        /// </summary>
        /// <param name="name">the name of the ident to search in the list of the table</param>
        /// <returns>true if exist and false if not</returns>
        private bool ExistInTableAssignation(string name)
        {
            var tempStack = ambit;
            foreach (var ambits in tempStack)
            {
                foreach (var item in SimbolsTable)
                {
                    if (item.name == name && item.ambit == ambits)
                    {
                        return true;
                    }
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
            if (!Directory.Exists(Path.Combine(CarpetaOut, "Analisis_semantico")))
            {
                Directory.CreateDirectory(Path.Combine(CarpetaOut, "Analisis_semantico"));
            }
            else
            {
                if (File.Exists(Path.Combine(CarpetaOut, "Analisis_semantico", "Analisis_semantico.out")))
                {
                    File.WriteAllText(Path.Combine(CarpetaOut, "Analisis_semantico", "Analisis_semantico.out"), string.Empty);
                }
            }
            using (var writeStream = new FileStream(Path.Combine(CarpetaOut, "Analisis_semantico", "Analisis_semantico.out"), FileMode.OpenOrCreate))
            {
                using (var write = new StreamWriter(writeStream))
                {
                    foreach (var item in SimbolsTable)
                    {
                        var valor = item.value;
                        if (item.value == null)
                        {
                            valor = "NULL";
                        }
                        var ambito = item.ambit;
                        if (item.ambit == null)
                        {
                            ambito = "NULL";
                        }
                        write.Write("TYPE:" + item.type + "| " + "NAME:" + item.name + "| " + "VALUE:" + valor + "|" + "AMBIT:"  + ambito);

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

        /// <summary>
        /// Method to validate if the assignments of a variable are correct
        /// </summary>
        /// <param name="position">the position after the equal</param>
        /// <param name="Type">The type of the variable that is being assigned </param>
        /// <returns>the result of the assigned</returns>
        private string ReturnValue(int position, string Type)
        {
            var resultado_int = 0;
            var resultado_double = 0.0;
            var resultado_string = "";
            var resultado_ident = "";
            var resultado_bool = true;
            var resultado = "";
            while (listTokens[position].Item1 != ")")
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
                            if (!listTokens[position].Item1.Contains("stringConstant") && !listTokens[position].Item1.Contains("boolConstant")
                                && !listTokens[position].Item1.Contains("doubleConstant"))
                            {
                                var tempData = split_value(listTokens[position + 1].Item2);
                                int n2;
                                bool isNumeric2 = int.TryParse(tempData, out n2);

                                if (date_value == "\"+\"")
                                {
                                    if (isNumeric2 == true)
                                    {
                                        resultado_int += Convert.ToInt32(tempData);
                                        return Convert.ToString(resultado_int);
                                    }
                                    else
                                    {
                                        if (ExistInTable(tempData, ambit.Peek()))
                                        {
                                            var index = SimbolsTable.FindIndex(c => c.name == tempData);
                                            var date_value_ = SimbolsTable[index].value;
                                            var date_type_ = SimbolsTable[index].type;
                                            if (date_value_ != null)
                                            {
                                                resultado_int += Convert.ToInt32(date_value_);
                                                // return Convert.ToString(resultado_int);
                                                position++;
                                            }
                                            if (Type != date_type_)
                                            {
                                                mistakes.Add($"No coinciden en terminos  {cordenadas}");
                                                return "";
                                            }
                                        }
                                        else
                                        {
                                                mistakes.Add($"No esta definida la variable  {cordenadas}");
                                                return "";
                                        }
                                    }
                                }
                                else if (date_value == "\"%\"")
                                {
                                    if (isNumeric2 == true)
                                    {
                                        resultado_int %= Convert.ToInt32(tempData);
                                        return Convert.ToString(tempData);
                                    }
                                     
                                }
                                else if (date_value == "\"*\"")
                                {
                                    if (isNumeric2 == true)
                                    {
                                        resultado_int *= Convert.ToInt32(tempData);
                                        return Convert.ToString(tempData);
                                    }
                                }
                                else if (date_value == "\"-\"")
                                {
                                    if (isNumeric2 == true)
                                    {
                                        resultado_int -= Convert.ToInt32(tempData);
                                        return Convert.ToString(tempData);
                                    }
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
                                            mistakes.Add($"No coinciden en terminos  {cordenadas}");
                                            return "";
                                        }
                                    }
                                    else
                                    {
                                        int n;
                                        bool isNumeric = int.TryParse(date_value,out n);
                                        if (isNumeric == true)
                                        {
                                            resultado_int = Convert.ToInt32(date_value);
                                        }
                                        else
                                        {
                                            mistakes.Add($"No existe la variable que desea retornar  {cordenadas}");
                                            return "";
                                        }
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
                        if (!listTokens[position].Item1.Contains("doubleConstant") && !listTokens[position].Item1.Contains("boolConstant")
                                && !listTokens[position].Item1.Contains("intConstant"))
                        {
                            if (date_value == "\"+\"")
                            {
                                resultado_string += split_value(listTokens[position + 1].Item2).Replace('"', ' ');
                                return resultado_string;
                            }
                            else
                            {
                                if (ExistInTable(date_value, ambit.Peek()))
                                {
                                    var index = SimbolsTable.FindIndex(c => c.name == date_value);
                                    var date_value_ = SimbolsTable[index].value;
                                    var date_type_ = SimbolsTable[index].type;
                                    if (date_value_ != null)
                                    {
                                        resultado_string = date_value_.Replace('"', ' ');
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
                                    resultado_string = date_value.Replace('"', ' ');
                                }

                            }
                        }
                        else
                        {
                            mistakes.Add($"Valor incorrecto declarado tipo {Type}  {cordenadas}");
                            return "";
                        }
                        break;
                    case "bool":
                        if (date_value.Contains("("))
                        {
                            resultado_bool = Convert.ToBoolean(true);
                            return Convert.ToString(resultado_bool);
                        }

                        else if (ExistInTable(date_value, ambit.Peek()))
                        {
                            var index = SimbolsTable.FindIndex(c => c.name == date_value);
                            var date_value_ = SimbolsTable[index].value;
                            var date_type_ = SimbolsTable[index].type;
                            if (date_value_ != null)
                            {
                                try
                                {
                                    resultado_bool = Convert.ToBoolean(date_value_);
                                }
                                catch (Exception)
                                {

                                }
                                
                            }
                            if (Type != date_type_)
                            {
                                mistakes.Add($"No coinciden en terminos  {cordenadas}");
                                return "";
                            }
                        }
                        else
                        {
                            if (date_value.Contains("true") || date_value.Contains("false"))
                            {
                                resultado_bool = Convert.ToBoolean(date_value);
                            }
                            else
                            {
                                mistakes.Add($"Valor incorrecto declarado tipo {Type}  {cordenadas}");
                                return "";
                            }
                        }
                        break;
                    case "double":
                        if (date_value == "New")
                        {
                            mistakes.Add($"Valor incorrecto declarado tipo New  {cordenadas}");
                            return resultado;
                        }
                        else
                        {
                            if (!listTokens[position].Item1.Contains("stringConstant") && !listTokens[position].Item1.Contains("boolConstant"))
                            {

                                if (date_value == "\"+\"")
                                {
                                    try
                                    {
                                        resultado_double += Convert.ToDouble(split_value(listTokens[position + 1].Item2));
                                        return Convert.ToString(resultado_double);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                                else if (date_value == "\"%\"")
                                {
                                    try
                                    {
                                        resultado_double %= Convert.ToDouble(split_value(listTokens[position + 1].Item2));
                                        return Convert.ToString(resultado_double);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                                else if (date_value == "\"*\"")
                                {
                                    try
                                    {
                                        resultado_double *= Convert.ToDouble(split_value(listTokens[position + 1].Item2));
                                        return Convert.ToString(resultado_double);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                                else if (date_value == "\"-\"")
                                {
                                    try
                                    {
                                        resultado_double -= Convert.ToDouble(split_value(listTokens[position + 1].Item2));
                                        return Convert.ToString(resultado_double);
                                    }
                                    catch (Exception)
                                    {

                                    }
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
                                            resultado_double = Convert.ToDouble(date_value_);
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
                                        resultado_double = Convert.ToDouble(date_value);
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
                    case "ident":

                        if (date_value == "New")
                        {
                            resultado = date_value;
                        }
                        if (!listTokens[position].Item1.Contains("doubleConstant") && !listTokens[position].Item1.Contains("boolConstant")
                                && !listTokens[position].Item1.Contains("intConstant"))
                        {
                            if (date_value == "\"+\"")
                            {
                                resultado_ident += split_value(listTokens[position + 1].Item2).Replace('"', ' ');
                                return resultado_ident;
                            }
                            else
                            {
                                if (ExistInTable(date_value, ambit.Peek()))
                                {
                                    var index = SimbolsTable.FindIndex(c => c.name == date_value);
                                    var date_value_ = SimbolsTable[index].value;
                                    var date_type_ = SimbolsTable[index].type;
                                    if (date_value_ != null)
                                    {
                                        resultado_ident = date_value_.Replace('"', ' ');
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
                                    resultado_ident = date_value.Replace('"', ' ');
                                }
                            }
                        }
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


    }
}
