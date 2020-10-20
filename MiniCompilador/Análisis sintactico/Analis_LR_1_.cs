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
        Dictionary<int, Tuple<int, string>> grammar = new Dictionary<int, Tuple<int, string>>();
        Stack<int> pila = new Stack<int>();
        Stack<Tuple<string, string>> Simbolo = new Stack<Tuple<string, string>>();
        Queue<Tuple<string, string>> Entrada = new Queue<Tuple<string, string>>();
        /// <summary>
        /// Validar la entrada de cada tocken
        /// </summary>
        /// <param name="tokens_">Entrada</param>
        public void table(Queue<Tuple<string, string>> tokens_)
        {
            Upload.threadTable.Join();
            Upload.ReadTxtFile();

            tables_dictionary = Upload.table;
            grammar = Upload.grammar;

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

            if (symbol_Action.ContainsKey(symbol))
            {
                 if (symbol_Action[symbol].Contains("/"))
                {
                    var conflicts = symbol_Action[symbol].Split('/');
                    conflicto(conflicts);
                }
                else if (symbol_Action[symbol].Contains("s"))
                {
                    var Acction = Convert.ToInt32(symbol_Action[symbol].Substring(1));
                    pila.Push(Acction);
                    Simbolo.Push(new Tuple<string, string>(symbol, line));
                    Entrada.Dequeue();
                    search_symbol(pila.Peek(), Entrada.Peek()); // Avanzar al siguiente token en la entrada
                }
                else if (symbol_Action[symbol].Contains("r"))
                {
                    var Acction = Convert.ToInt32(symbol_Action[symbol].Substring(1));
                    var num_reducir = grammar[Acction].Item1;
                    for (int i = 0; i < num_reducir; i++)
                    {
                        Simbolo.Pop();
                    }
                    Simbolo.Push(new Tuple<string, string>(grammar[Acction].Item2, line));
                    pila.Pop();
                    search_symbol(pila.Peek(), Simbolo.Peek());
                }
                else if (symbol_Action[symbol].Contains("acc"))
                {
                    // salir 
                }
                else // num desplazamiento
                {
                    var Acction = Convert.ToInt32(symbol_Action[symbol]);
                    pila.Push(Acction);
                    search_symbol(pila.Peek(), Entrada.Peek());
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
