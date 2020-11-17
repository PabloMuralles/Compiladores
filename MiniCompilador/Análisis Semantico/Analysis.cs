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
                    var value_ = defination_value(positionList + 2);
                    var index = SimbolsTable.FindIndex(c => c.name == Name);
                    SimbolsTable[index] = new TableElement { name = Name, value = value_, type = dataListpreviously.Item1, ambit = null, isClass = true, isFunction = false };

                }
                else
                {
                    mistakes.Add($"Error la variable :{dataListActual.Item1} no fue definida con anterioridad");
                }
            }
            //creacion de una funcion o precedimiento
            else if (dataListNext.Item1 == "(")
            {


            }
        }

        private void ValidateType()
        {

        }

        private string Split_Name(string Name)
        {
            var Name_ = Name.Split(',');
            return Name_[2];
        }

        private string defination_value(int position)
        {
            var resultado = "";
            while (listTokens[position].Item1 != ";")
            {
                var date_value = split_value(listTokens[position].Item2);
                if (date_value == "+")
                {

                }
                else
                {
                    resultado += date_value;
                }
                position++;
            }
            return resultado;
        }

        private string split_value(string value)
        {
            var value_ = value.Split(',');
            return value_[2];
        }

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
