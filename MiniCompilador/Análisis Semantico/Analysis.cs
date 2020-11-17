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
            SimbolsTable.Add(new TableElement {name = "Parse",value = null, type = "class",ambit = "1", isClass = true, isFunction = false });
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
            var dataListActions = listTokens[positionList];

            //declarcion de variaable
            if (dataListNext.Item1 == ";")
            {
                if (!ExistInTable(dataListActions.Item1))
                {

                }
                else
                {
                    mistakes.Add($"Error la variable :{dataListActions.Item1} ya fue definida con anterioridad");
                }
            }
            //validar si es una clase
            else if (dataListpreviously.Item1 == "class")
            {
                if (!ExistInTable(dataListActions.Item1))
                {

                }
                else
                {
                    mistakes.Add($"Error la variable :{dataListActions.Item1} ya fue definida con anterioridad");
                }
            }
            //asignacion de una variable
            else if (dataListNext.Item1 == "=")
            {
                if (ExistInTable(dataListActions.Item1))
                {

                }
                else
                {
                    mistakes.Add($"Error la variable :{dataListActions.Item1} no fue definida con anterioridad");
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
