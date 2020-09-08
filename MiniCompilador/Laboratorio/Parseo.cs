using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompilador.Laboratorio
{
    class Parseo
    {


        private List<Tuple<string, string>> tokens = new List<Tuple<string, string>>();

        private string lookahead = string.Empty;

        private int contador = 0;

        private List<string> errores = new List<string>();

        

        private void MatchToken(string expectedToken)
        {
            try
            {
                if (lookahead == expectedToken)
                {
                    contador++;
                    lookahead = tokens[contador].Item1;
                }
                else
                {
                    errores.Add($"Error sintactico: se esperaba {expectedToken} y se tenia {lookahead}. {ObtenerUbicacion(tokens[contador == 0 ? contador : contador -1].Item2)}");
                }
            }
            catch (Exception)
            {
                // se terminaron los tokens a leer 
                throw;
            }
        }

        /// <summary>
        /// Metodo para poder mostrar la linea y la columna donde se encuentra el error
        /// </summary>
        /// <param name="cadena">el item 2 de la lista de tuplas de tokens</param>
        /// <returns>la linea y columnas donde se encuentra el error</returns>
        private string ObtenerUbicacion(string cadena)
        {
            var split = cadena.Split(',');

            var newCadena = string.Empty;

            for (int i = 1; i < split.Length; i++)
            {
                newCadena += split[i] + " ";
            }

            return newCadena;
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="tokens_">Lista de tuplas generada en el analisis lexico</param>
        public Parseo(List<Tuple<string, string>> tokens_)
        {
            tokens = tokens_;
            lookahead = tokens[contador].Item1;
        }

    }
}
