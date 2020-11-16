using System;
using System.Collections.Generic;
using System.Linq;
using MiniCompilador.GUI;

namespace Minic.Análisis_sintactico
{
    class Analis_LR_1_
    {
        Dictionary<int, Dictionary<string, string>> tables_dictionary = new Dictionary<int, Dictionary<string, string>>();
        Dictionary<int, Tuple<int, string>> grammar = new Dictionary<int, Tuple<int, string>>();
        Stack<int> pila = new Stack<int>();
        Stack<Tuple<string, string>> Simbolo = new Stack<Tuple<string, string>>();
        Queue<Tuple<string, string>> Entrada = new Queue<Tuple<string, string>>();
        List<string> Errores = new List<string>();
        public static Dictionary<string, List<string>> follows = new Dictionary<string, List<string>>();
        Cargar_Archivo Cargar_Archivo = new Cargar_Archivo();
        /// <summary>
        /// Validar la entrada de cada tocken
        /// </summary>
        /// <param name="tokens_">Entrada</param>
        public void table(Queue<Tuple<string, string>> tokens_)
        {
            var objUpload = new Upload();

            tables_dictionary = objUpload.ReadTxtFileAnalysisTable();
            grammar = objUpload.ReadTxtFileGrammar();

            tokens_.Enqueue(new Tuple<string, string>("$", ",")); //Fin de linea
            pila.Push(0);
            Entrada = tokens_;
            search_symbol(pila.Peek(), tokens_.Peek());

            var objAnalysis = new Análisis_Semantico.Analysis(tokens_);

        }
        /// <summary>
        /// Method to find the symbols that belonhs to a state
        /// </summary>
        /// <param name="state">state of the grammar to find symbols</param>
        /// <param name="token_">tokens queue to analyze</param>
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
        /// <summary>
        /// Method to find the action that belongs to a symbol
        /// </summary>
        /// <param name="symbol_Action">All the action that have the state</param>
        /// <param name="symbol">The symbol of the tokens to analyze anal</param>
        /// <param name="line">The ubication line of the token to analyze </param>
        private void search_Accion(Dictionary<string, string> symbol_Action, string symbol, string line)
        {

            if (symbol_Action.ContainsKey(symbol))
            {
                // solo existe un conflicto que es con el . por lo que este if solo va funcionar para este conflicto
                if (symbol_Action[symbol].Contains("/"))
                {
                    var element1 = Entrada.ElementAt(1);
                    var element2 = Entrada.ElementAt(2);
                    var action = symbol_Action[symbol];
                    var splitAction = action.Split('/');

                    if (element1.Item1 == "ident" && element2.Item1 == "")
                    {
                        var Acction = Convert.ToInt32(splitAction[1].Trim().Substring(1));
                        pila.Push(Acction);
                        Simbolo.Push(new Tuple<string, string>(symbol, line));
                        Entrada.Dequeue();
                        search_symbol(pila.Peek(), Entrada.Peek()); // Avanzar al siguiente token en la entrada
                    }
                    else
                    {
                        var Acction = Convert.ToInt32(splitAction[1].Trim().Substring(1));
                        var num_reducir = grammar[Acction].Item1;
                        for (int i = 0; i < num_reducir; i++)
                        {
                            Simbolo.Pop();
                            pila.Pop();
                        }
                        Simbolo.Push(new Tuple<string, string>(grammar[Acction].Item2, line));
                        search_symbol(pila.Peek(), Simbolo.Peek());
                    }

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
                        pila.Pop();
                    }
                    Simbolo.Push(new Tuple<string, string>(grammar[Acction].Item2, line));
                    search_symbol(pila.Peek(), Simbolo.Peek());
                }
                else if (symbol_Action[symbol].Contains("acc"))
                {

                    if (!Errores.Any())
                    {
                        Errores.Add("Success");
                    }
                    if (Entrada.Count() == 1)
                    {
                        Exit_Analyze();
                    }
                    else
                    {
                        search_symbol(pila.Peek(), Entrada.Peek());
                    }
                }
                else // num desplazamiento (irA)
                {
                    var Acction = Convert.ToInt32(symbol_Action[symbol]);
                    pila.Push(Acction);
                    search_symbol(pila.Peek(), Entrada.Peek());
                }

            }
            else
            {
                if (symbol_Action.ContainsKey("ε"))
                {
                    symbol = "ε";
                    if (symbol_Action[symbol].Contains("s"))
                    {
                        var Acction = Convert.ToInt32(symbol_Action[symbol].Substring(1));
                        pila.Push(Acction);
                        Simbolo.Push(new Tuple<string, string>(symbol, " "));
                        search_symbol(pila.Peek(), Entrada.Peek());
                    }
                    else if (symbol_Action[symbol].Contains("r"))
                    {
                        var Acction = Convert.ToInt32(symbol_Action[symbol].Substring(1));
                        var num_reducir = grammar[Acction].Item1;
                        for (int i = 0; i < num_reducir; i++)
                        {
                            Simbolo.Pop();
                            pila.Pop();
                        }
                        Simbolo.Push(new Tuple<string, string>(grammar[Acction].Item2, line));
                        search_symbol(pila.Peek(), Simbolo.Peek());
                    }
                }
                else
                {
                    //error
                    var datos_errores = line.Split(',');
                    Errores.Add($"Error tocken: {symbol} linea: {datos_errores[0]} columna: {datos_errores[1]} ");
                    // Reset
                    string last_line = datos_errores[0];
                    reset(last_line);
                }

            }
        }
        private void reset(string last_line)
        {
        Go_back:
            var date = Entrada.Peek().Item2.Split(',');
            if (date[0] == last_line)
            {
                Entrada.Dequeue();
                goto Go_back;
            }
            pila = new Stack<int>();
            Simbolo = new Stack<Tuple<string, string>>();
            pila.Push(0);
            if (Entrada.Count() == 1)
            {
                Exit_Analyze();
            }
            else
            {
                search_symbol(pila.Peek(), Entrada.Peek());
            }
        }

        private void Exit_Analyze()
        {
            string msg_Analyze = string.Empty;
            foreach (var item in Errores)
            {
                msg_Analyze += item + " \n ";
            }
            Cargar_Archivo.msg_Analyze_syntactic(msg_Analyze);
            tables_dictionary = new Dictionary<int, Dictionary<string, string>>();
            grammar = new Dictionary<int, Tuple<int, string>>();
            follows = new Dictionary<string, List<string>>();
            pila = new Stack<int>();
            Simbolo = new Stack<Tuple<string, string>>();
            Entrada = new Queue<Tuple<string, string>>();
        }
    }
}
