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
            for (int i = 0; i <= 180; i++)
            {
              tables_dictionary.Add(i, ReturnDictionary(i));
            }
        }
        private Dictionary<string, string> ReturnDictionary(int state)
         {
            Dictionary<string, string> symbol_action = new Dictionary<string, string>();            
            switch (state)
            {
                case 0:
                    symbol_action.Add("ident","s12");
                    symbol_action.Add("class", "s5");
                    symbol_action.Add("interface", "s6");
                    symbol_action.Add("const", "s7");
                    symbol_action.Add("void", "s13");
                    symbol_action.Add("int", "s8");
                    symbol_action.Add("double", "s9");
                    symbol_action.Add("bool", "s10");
                    symbol_action.Add("string", "s11");
                    symbol_action.Add("Program", "");
                    symbol_action.Add("Decl", "");
                    symbol_action.Add("Reserved", "");
                    symbol_action.Add("", "");
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
                    break;
                case 12:
                    break;
                case 13:
                    break;
                case 15:
                    break;
                case 16:
                    break;
                case 17:
                    break;
                case 18:
                    break;
                case 19:
                    break;
                case 20:
                    break;
                case 21:
                    break;
                case 22:
                    break;
                case 23:
                    break;
                case 24:
                    break;
                case 25:
                    break;
                case 26:
                    break;
                case 27:
                    break;
                case 28:
                    break;
                case 29:
                    break;
                case 30:
                    break;
                case 31:
                    break;
                case 32:
                    break;
                case 33:
                    break;
                case 34:
                    break;
                case 35:
                    break;
                case 36:
                    break;
                case 37:
                    break;
                case 38:
                    break;
                case 39:
                    break;
                case 40:
                    break;
                case 41:
                    break;
                case 42:
                    break;
                case 43:
                    break;
                default:
                    break;
            }

            return symbol_action;
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
