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
                case 91:
                    symbol_action.Add("ident","r56");
                    symbol_action.Add(";", "r56");
                    symbol_action.Add("(", "r56");
                    symbol_action.Add(")", "r56");
                    symbol_action.Add(",", "r56");
                    symbol_action.Add("&&", "s8");
                    symbol_action.Add("double", "s9");
                    symbol_action.Add("bool", "s10");
                    symbol_action.Add("string", "s11");
                    symbol_action.Add("Program", "");
                    symbol_action.Add("Decl", "");
                    symbol_action.Add("Reserved", "");
                    symbol_action.Add("", "");
                    break;
                case 92:
                    break;
                case 93:
                    break;
                case 94:
                    break;
                case 95:
                    break;
                case 96:
                    break;
                case 97:
                    break;
                case 98:
                    break;
                case 99:
                    break;
                case 100:
                    break;
                case 101:
                    break;
                case 102:
                    break;
                case 103:
                    break;
                case 104:
                    break;
                case 105:
                    break;
                case 106:
                    break;
                case 107:
                    break;
                case 108:
                    break;
                case 109:
                    break;
                case 110:
                    break;
                case 111:
                    break;
                case 112:
                    break;
                case 113:
                    break;
                case 114:
                    break;
                case 115:
                    break;
                case 116:
                    break;
                case 117:
                    break;
                case 118:
                    break;
                case 119:
                    break;
                case 120:
                    break;
                case 121:
                    break;
                case 122:
                    break;
                case 123:
                    break;
                case 124:
                    break;
                case 125:
                    break;
                case 126:
                    break;
                case 127:
                    break;
                case 128:
                    break;
                case 129:
                    break;
                case 130:
                    break;
                case 131:
                    break;
                case 132:
                    break;
                case 133:
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
