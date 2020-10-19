using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Minic.Análisis_sintactico
{
    class Analis_LR_1_
    {
        Dictionary<int, Dictionary<string, string>> tables_dictionary = new Dictionary<int, Dictionary<string, string>>();
        private void Import_table()
        {
            tables_dictionary.Add(0, );             



        }
        public void table(List<Tuple<string, string>> tokens_)
        {
            Import_table();
            tokens_.Add(new Tuple<string, string>("$", "")); //Fin de linea
            Stack<int> pila = new Stack<int>();
            pila.Push(0);


            if (tables_dictionary.ContainsKey(pila.Peek()))
            {
                
            }
            else
            {

            }


        }
        private int search_symbol(string[] symbol)
        {
            ///buscar la accion que corresponde a cada simbolo

            return 0;
        }

        private void search_Accion()
        {

        }


    }
}
