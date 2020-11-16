using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minic.Análisis_Semantico
{
    class Analysis
    {
        private List<TableElement> SimbolsTable = new List<TableElement>();

        private List<Tuple<string, string>> listTokens = new List<Tuple<string, string>>();


        private int positionList = 0; 
        public Analysis(List<Tuple<string, string>> tokens_)
        {
            listTokens = tokens_;
            IdentifyIdent();
        }

        private void IdentifyIdent()
        {
            SimbolsTable.Add(new TableElement {name = "Parse",value = null, type = "class",ambit = "1", isClass = true, isFunction = false });
            foreach (var item in listTokens)
            {
                if (item.Item1 == "ident")
                {

                }
                positionList++;
            }
       
        }

        private void ClassifyIdent()
        {
            var dataListNext = listTokens[positionList + 1];
            var dataListpreviously = listTokens[positionList - 1];

            //declarcion de variaable
            if (dataListNext.Item1 == ";")
            {
                //if (listTokens.Contains())
                //{

                //}
            }
            //validar si es una clase
            else if (dataListpreviously.Item1 == "class")
            {

            }
            //asignacion de una variable
            else if (dataListNext.Item1 == "=")
            {

            }
            //creacion de una funcion o precedimiento
            else if (dataListNext.Item1 == "(")
            {

            }
        }
    }

     
}
