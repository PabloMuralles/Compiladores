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
        GUI.Cargar_Archivo cargar_Archivo = new GUI.Cargar_Archivo();

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

        private bool Program_()
        {
            if (lookahead != "$")
            {
             return Decl() && Program_P();
            }
            return false;
        }

        private bool Program_P()
        {
            if (Program_())
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        private bool Decl()
        {
            if (VariableDecl())
            {
                return true;
            }
            else if (FunctionDecl())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool VariableDecl()
        {

            Variable();
            return true;
        }


        private bool Variable()
        {
            Type();
            if (lookahead == "identificador")
            {
                MatchToken("identificador");
                return true;
            }
            else
            {

                return false;
            }


        }
        private bool Type()
        {
            if (lookahead == "int")
            {
                MatchToken("int");
                TypeP();
                return true;

            }
            else if (lookahead == "double")
            {
                MatchToken("int");
                TypeP();
                return true;
            }
            else if (lookahead == "string")
            {
                MatchToken("string");
                TypeP();
                return true;
            }
            else if (lookahead == "identificador")
            {
                MatchToken("identificador");
                TypeP();
                return true;
            }
            else
            {
                return false;
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
        private bool FunctionDecl()
        {
            if (lookahead == "void")
            {
                MatchToken("void");
                MatchToken("identificiador");
                MatchToken("(");
                if (lookahead != ")")
                {
                    Formals();
                    return true;
                }
                MatchToken(")");
                return true;
            }
            else
            {
                if (Type())
                {
                    MatchToken("identificiador");
                    MatchToken("(");
                    if (lookahead != ")")
                    {
                        Formals();

                    }

                    MatchToken(")");
                    return true;

                }
                else
                {
                    return false;
                }

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
            tokens_.Add(new Tuple<string, string>("$", ""));
            tokens = tokens_;
            lookahead = tokens[contador].Item1;
            Program_();
            Salida(errores);
            
        }

        private void Salida(List<string> mensaje)
        {
            string M_mostrar = string.Empty;
            if (!mensaje.Any())
            {
                M_mostrar = "condiciones perfectas";
            }
            else 
            { 
              foreach (var item in mensaje)
              {
                M_mostrar += item + " \n ";
              }            
            }
             cargar_Archivo.Mostrar_mensajelab(M_mostrar);
        }

    }
}
