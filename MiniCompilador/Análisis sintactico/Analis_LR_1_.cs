﻿using System;
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
       
        /// <summary>
        /// Validar la entrada de cada tocken
        /// </summary>
        /// <param name="tokens_">Entrada</param>
        public void table(Queue<Tuple<string, string>> tokens_)
        {
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
