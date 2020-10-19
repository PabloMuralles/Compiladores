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
        Stack<int> pila = new Stack<int>();
        Stack<string> Simbolo = new Stack<string>();
        Queue<Tuple<string, string>> Entrada = new Queue<Tuple<string, string>>();
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
                    symbol_action.Add("ident", "s12");
                    symbol_action.Add("class", "s5");
                    symbol_action.Add("interface", "s6");
                    symbol_action.Add("const", "s7");
                    symbol_action.Add("void", "s13");
                    symbol_action.Add("int", "s8");
                    symbol_action.Add("double", "s9");
                    symbol_action.Add("bool", "s10");
                    symbol_action.Add("string", "s11");
                    symbol_action.Add("Program", "1");
                    symbol_action.Add("Decl", "2");
                    symbol_action.Add("Reserved", "4");
                    symbol_action.Add("Type", "3");
                    break;
                case 1:
                    symbol_action.Add("$", "acc");
                    break;
                case 2:
                    symbol_action.Add("ident", "s12");
                    symbol_action.Add("class", "s5");
                    symbol_action.Add("interface", "s6");
                    symbol_action.Add("const", "s7");
                    symbol_action.Add("void", "s13");
                    symbol_action.Add("int", "s8");
                    symbol_action.Add("double", "s9");
                    symbol_action.Add("bool", "s10");
                    symbol_action.Add("string", "s11");
                    symbol_action.Add("$", "r2");
                    symbol_action.Add("Program", "14");
                    symbol_action.Add("Decl", "2");
                    symbol_action.Add("Reserved", "4");
                    symbol_action.Add("Type", "3");
                    break;
                case 3:
                    symbol_action.Add("ident", "s15/r8");
                    symbol_action.Add("(", "r8");
                    symbol_action.Add("[]", "s16");
                    break;
                case 4:
                    symbol_action.Add("ident", "s17");
                    break;
                case 5:
                    symbol_action.Add("ident", "s18");
                    break;
                case 6:
                    symbol_action.Add("ident", "s19");
                    break;
                case 7:
                    symbol_action.Add("int", "s21");
                    symbol_action.Add("double", "s22");
                    symbol_action.Add("bool", "s23");
                    symbol_action.Add("string", "s24");
                    symbol_action.Add("ConstType", "20");
                    break;
                case 8:
                    symbol_action.Add("ident", "r10");
                    symbol_action.Add("(", "r10");
                    symbol_action.Add("[]", "r10");
                    break;
                case 9:
                    symbol_action.Add("ident", "r11");
                    symbol_action.Add("(", "r11");
                    symbol_action.Add("[]", "r11");
                    break;
                case 10:
                    symbol_action.Add("ident", "r12");
                    symbol_action.Add("(", "r12");
                    symbol_action.Add("[]", "r12");
                    break;
                case 11:
                    symbol_action.Add("ident", "r13");
                    symbol_action.Add("(", "r13");
                    symbol_action.Add("[]", "r13");
                    break;
                case 12:
                    symbol_action.Add("ident", "r14");
                    symbol_action.Add("(", "r14");
                    symbol_action.Add("[]", "r14");
                    break;
                case 13:
                    symbol_action.Add("ident", "r9");
                    symbol_action.Add("(", "r9");
                    break;
                case 14:
                    symbol_action.Add("$", "r1");
                    break;
                case 15:
                    symbol_action.Add(";", "s25");
                    break;
                case 16:
                    symbol_action.Add("ident", "r15");
                    symbol_action.Add("(", "r15");
                    symbol_action.Add("[]", "r15");
                    break;
                case 17:
                    symbol_action.Add("(", "s26");
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
                default:
                    break;
            }

            return symbol_action;
        }
        /// <summary>
        /// Validar la entrada de cada tocken
        /// </summary>
        /// <param name="tokens_">Entrada</param>
        public void table(Queue<Tuple<string, string>> tokens_)
        {
            Import_table();
            tokens_.Enqueue(new Tuple<string, string>("$", "")); //Fin de linea
            pila.Push(0);
            Entrada = tokens_;
            search_symbol(pila.Peek(), tokens_.Peek());
        }
        private void search_symbol(int state, Tuple<string, string> token_)
        {
            if (tables_dictionary.ContainsKey(state))
            {
                var symbol = token_.Item1;
                var line = token_.Item2;
                var symbol_Action = tables_dictionary[state];
                search_Accion(symbol_Action, symbol, line);
            }
            else
            {
                // Error el estado a buscar no existe
            }
        }

        private void search_Accion(Dictionary<string, string> symbol_Action, string symbol, string line)
        {
            //s desplazarce
            // r reduccion
            // num desplazamiento
            if (symbol_Action.ContainsKey(symbol))
            {
                if (symbol_Action[symbol].Contains("s"))
                {
                    var Acction = Convert.ToInt32(symbol_Action[symbol].Substring(1));
                    pila.Push(Acction);
                    Simbolo.Push(symbol);
                    Entrada.Dequeue();
                    search_symbol(pila.Peek(), Entrada.Peek()); // Avanzar al siguiente token en la entrada
                }
                else if (symbol_Action[symbol].Contains("r"))
                {
                    //Accion de reducir
                }
                else if (symbol_Action[symbol].Contains("acc"))
                {
                    // salir 
                }
                else if (symbol_Action[symbol].Contains("/"))
                {
                    var conflicts = symbol_Action[symbol].Split('/');
                    conflicto(conflicts);
                }
                else
                {
                    //Error simbolo en linea tal: line
                }

            }
            else
            {
                //validar si ya paso por un conflicto de reduccion
                // si si regresar de donde salio y probar con otro lado

                // ERROR el simbolo no coinside con el estado presente
            }
        }
        private void conflicto(string[] conflicts)
        {
            if (conflicts[0].Contains("r") && conflicts[1].Contains("r")) //r/r
            {
                // si causa un error de 
            }
            else//d/r
            {

            }
        }

    }
}
