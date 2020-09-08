using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
                       errores.Add($"Error sintactico: se esperaba {expectedToken} y se tenia {lookahead}. {ObtenerUbicacion(tokens[contador == 0 ? contador : contador - 1].Item2)}");
                    
                }
            }
            catch (Exception)
            {
                // se terminaron los tokens a leer 
                throw;
            }
        }

        private void Program_()
        {
            Decl();
            Program_P();
            
             
        }

        private void Program_P()
        {
            if (lookahead != "$")
            {
                Program_();

            } 
        }

        private void Decl()
        {
            VariableDecl();
            FunctionDecl();
        }

        private void VariableDecl()
        {
            Variable();
        }
         
        private void Variable()
        {
            Type();
            // ver como vamos a manejar las tuplas
            MatchToken("identificador");
        }
        private void Type()
        {
            if (lookahead == "int") 
            {
                MatchToken("int");
                TypeP();

            }
            else if (lookahead == "double")
            {
                MatchToken("int");
                TypeP();
            }
            else if (lookahead == "string")
            {
                MatchToken("string");
                TypeP();
            }
            else if (lookahead == "identificador")
            {
                MatchToken("identificador");
                TypeP();
            }
        }
        private void TypeP()
        {
            if (lookahead == "[]")
            {
                MatchToken("[]");
                TypeP();
            }
             
        }
        private void FunctionDecl()
        {
            if (lookahead == "void")
            {
                MatchToken("void");
                MatchToken("identificiador");
                MatchToken("(");
                if (lookahead != ")")
                {
                    Formals();
                }
                MatchToken(")");


            }
            else
            {
                Type();
                MatchToken("identificiador");
                MatchToken("(");
                if (lookahead != ")")
                {
                    Formals();
                }
                MatchToken(")");
                


            }

        }

        private void Formals()
        {
            VariableP();
            MatchToken(",");
        }

        private void VariableP()
        {
            Variable();
            VariableP();
        }

        private void Stmt()
        {
            StmtP();
            Stmt();
        }
        private void StmtP()
        {
            if (lookahead == "for")
            {
                ForStmt();
            }
            else if (lookahead == "return")
            {
                ReturnStmt();
            }
            else
            {
                // falta expr
                MatchToken(";");
            }
        }

        private void ForStmt()
        {
            MatchToken("for");
            MatchToken("(");
            if (lookahead != ")")
            {
                ExprP();
            }
            MatchToken(")");
            MatchToken(";");
            Expr();
            MatchToken(";");
            MatchToken("(");
            if (lookahead != ")")
            {
                ExprP();
            }
            Stmt();


        }

        private void ReturnStmt()
        {
            MatchToken("return");
            MatchToken("(");
            if (lookahead != ")")
            {
                ExprP();
            }
            MatchToken(")");
            MatchToken(";");
        }
        private void ExprP()
        {
            ExprP();
        }

        private void Expr()
        {
            // Lvalue = P | P 
        }

        private void Lvalue()
        {
            if (lookahead == "identificador")
            {
                MatchToken("identificador");
            }
            else
            {
                Expr();
                if (lookahead == ".")
                {
                    MatchToken(".");
                    MatchToken("identificador");
                }
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
            tokens_.Add(new Tuple<string, string>("$", " "));
            tokens = tokens_;
            lookahead = tokens[contador].Item1;
            Program_();
        }

    }
}
