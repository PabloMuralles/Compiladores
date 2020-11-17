using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Minic.Análisis_Semantico
{
    class Analysis
    {
        private List<TableElement> SimbolsTable = new List<TableElement>();

        private List<Tuple<string, string>> listTokens = new List<Tuple<string, string>>();

        private Regex comparations = new Regex(@"^(<=|>=|==|!=)$");

        private List<string> mistakes = new List<string>();

        private int positionList = 0;
        public Analysis(List<Tuple<string, string>> tokens_)
        {
            listTokens = tokens_;
            IdentifyIdent();
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
        }

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
                //llamar a la funcion que hace el split return nombre

                var Name = Split_Name(dataListActual.Item2);
                if (!ExistInTable(Name))
                {
                    SimbolsTable.Add(new TableElement { name = Name, value = null, type = dataListpreviously.Item1, ambit = null, isClass = false, isFunction = false });
                }
                else
                {
                    mistakes.Add($"Error la variable :{dataListActual.Item1} ya fue definida con anterioridad");
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
                    mistakes.Add($"Error la variable :{dataListActual.Item1} ya fue definida con anterioridad");
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
                    SimbolsTable[index] = new TableElement { name = Name, value = value_, type = Type, ambit = null, isClass = true, isFunction = false };
     
                }
                else
                {
                    mistakes.Add($"Error la variable :{dataListActual.Item1} no fue definida con anterioridad");
                }
            }
            //creacion de una funcion o precedimiento
            else if (dataListNext.Item1 == "(")
            {
                var name = dataListActual.Item1;
                if (!ExistInTable(Split_Name(name)))
                {
                    SimbolsTable.Add(new TableElement { name = name, value = null, type = dataListpreviously.Item1, ambit = null, isClass = false, isFunction = true });
                }
                else
                {
                    mistakes.Add($"Error la Funcion :{dataListActual.Item1} ya fue declarada con anterioridad");
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

        private string defination_value(int position, string Type)
        {
           
            var resultado_int = 0;
            var rsultado_string = "";
            var resultado = "";
            while (listTokens[position].Item1 != ";")
            {
                var date_value = split_value(listTokens[position].Item2);
                switch (Type)
                {
                    case "int":
                        if (date_value != "\"+\"")
                        {
                            resultado_int += Convert.ToInt32(date_value);
                        }
                         
                        break;
                    default:
                        break;
                }

                position++;
            }
            if (Type== "int")
            {
                resultado = Convert.ToString(resultado_int);
            }
            return resultado;
        }

        private string split_value(string value)
        {
            var value_ = value.Split(',');
            return value_[2];
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
    }


}
