using MiniCompilador.Análisis_Léxico;
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
        private int contadorAux = 0;

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
                    if (lookahead == "$")
                    {
                        errores.Add($"Error sintactico: se esperaba ' {expectedToken} ' y ya no se tenian tokens.");
                    }
                    else
                    {
                        errores.Add($"Error sintactico: se esperaba ' {expectedToken} ' y se tenia ' {lookahead} '. {ObtenerUbicacion(tokens[contador == 0 ? contador : contador - 1].Item2)}");
                    }

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

            return Decl() && Program_P();

        }

        private bool Program_P()
        {
            if (lookahead != "$")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Decl()
        {
            return VariableDecl() || FunctionDecl();
        }

        private bool VariableDecl()
        {
            if (Variable())
            {
                MatchToken(";");
                return true;
            }
            else
            {
                //Error no cumple con la gramatica
                return false;
            }



        }
        private bool Variable()
        {
            if (Type())
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
                return TypeP();

            }
            else if (lookahead == "double")
            {
                MatchToken("int");
                return TypeP();
            }
            else if (lookahead == "string")
            {
                MatchToken("string");
                return TypeP();
            }
            else if (lookahead == "identificador")
            {
                MatchToken("identificador");
                return TypeP();

            }
            else
            {
                return false;
            }
        }

        private bool TypeP()
        {
            if (lookahead == "[]")
            {
                MatchToken("[]");
                TypeP();

                return true;
            }
            return true;
        }
        private bool FunctionDecl()
        {
            if (Type())
            {
                MatchToken("identificador");
                MatchToken("(");
                if (Formals())
                {
                    MatchToken(")");
                    Stmt();
                    return true;
                }

                MatchToken(")");
                Stmt();
                return true;

            }
            else if (lookahead == "void")
            {
                MatchToken("void");
                MatchToken("identificador");
                MatchToken("(");
                if (Formals())
                {
                    
                    MatchToken(")");
                    Stmt();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }



        private bool Formals()
        {
            if (Variable())
            {
                VariableP();
                MatchToken(",");
                return true;
            }
            else
            {
                return true;
            }

        }

        private bool VariableP()
        {
            if (Variable())
            {
                VariableP();
                return true;
            }
            return true;
        }

        private bool Stmt()
        {
            if (StmtP())
            {
                Stmt();
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool StmtP()
        {
            if (lookahead == "for")
            {

                return ForStmt();
            }
            else if (lookahead == "return")
            {
                return ReturnStmt();


            }
            else if (Expr())
            {
                // falta expr
                MatchToken(";");
                return true;

            }
            else
            {
                return false;

            }
        }

        private bool ForStmt()
        {
            MatchToken("for");
            MatchToken("(");

            if (ExprP())
            {
                MatchToken(";");
            }
            else { MatchToken(";"); }
            if (Expr())
            {
                MatchToken(";");
            }
            else { MatchToken(";"); }
            if (ExprP())
            {
                MatchToken(";");
            }
            else { MatchToken(";"); }
            MatchToken(")");
            if (Stmt())
            {
                return true;
            }
            else
            {
                return false;
            }



        }

        private bool ReturnStmt()
        {
            MatchToken("return");
            if (ExprP())
            {
                MatchToken(";");
                return true;
            }
            else
            {
                MatchToken(";");
                return true;
            }
        }
        private bool ExprP()
        {
            if (Expr())
            {
                return true;
            }
            else { return false; }

        }

        private bool Expr()
        {
            // para hacer backtraking
            contadorAux = contador;

            if (Lvalue())
            {
                if (lookahead == "=")
                {
                    MatchToken("=");
                    if (P())
                    {
                        return true;
                    }
                    else { return false; }
                }
                else
                {
                    contador = contadorAux;
                    return P();

                }

            }
            else
            {
                return P();
            }
        }

        private bool Lvalue()
        {
            if (lookahead == "identificador")
            {
                MatchToken("identificador");

                return true;
            }
            else
            {
                if (Expr())
                {
                    if (lookahead == ".")
                    {
                        MatchToken(".");
                        MatchToken("identificador");
                        return true;
                    }
                    else if (lookahead == "[")
                    {
                        MatchToken("[");
                        if (Expr())
                        {
                            if (lookahead == "]")
                            {
                                MatchToken("]");
                                return true;
                            }
                            else { return false; }
                        }
                        else { return false; }
                    }
                    else { return false; }
                }
                else { return false; }
            }
        }

        private bool P()
        {
            return T() && P();
        }

        private bool PP()
        {
            if (lookahead == "||")
            {
                MatchToken("||");
                return T() && PP();
            }
            else
            {
                return true;
            }
        }

        private bool T()
        {
            return H() && TP();
        }

        private bool TP()
        {
            if (lookahead == "&&")
            {
                MatchToken("&&");
                return H() && TP();
            }
            else
            {
                return true;
            }
        }

        private bool H()
        {
            return F() && HP();
        }

        private bool HP()
        {
            if (lookahead == "==")
            {
                MatchToken("==");
                F();
                HP();
                return true;
            }
            else if (lookahead == "!=")
            {
                MatchToken("!=");
                F();
                HP();
                return true;
            }
            else
            {
                return true;
            }
        }
        private bool F()
        {
            return L() && FP();
        }

        private bool FP()
        {
            if (lookahead == "<")
            {
                MatchToken("<");
                FP();
                return true;
            }
            else if (lookahead == ">")
            {
                MatchToken(">");
                FP();
                return true;

            }
            else if (lookahead == "<=")
            {
                MatchToken("<=");
                FP();
                return true;
            }
            else if (lookahead == ">=")
            {
                MatchToken(">=");
                FP();
                return true;
            }
            else
            {
                return true;
            }
        }
        private bool L()
        {
            return M() && LP();
        }
        private bool LP()
        {
            if (lookahead == "+")
            {
                MatchToken("+");
                LP();
                return true;
            }
            else if (lookahead == "-")
            {
                MatchToken("-");
                LP();
                return true;
            }
            else
            {
                return true;
            }
        }
        private bool M()
        {
            return N() && MP();
        }
        private bool MP()
        {
            if (lookahead == "*")
            {
                MatchToken("*");
                MP();
                return true;
            }
            else if (lookahead == "/")
            {
                MatchToken("/");
                MP();
                return true;
            }
            else
            {
                return true;
            }
        }
        private bool N()
        {
            if (lookahead == "-")
            {
                MatchToken("-");
                // duda si el return se podria poner expre
                Expr();
                return true;
            }
            else if (lookahead == "!")
            {
                MatchToken("!");
                // duda si el return se podria poner expre
                Expr();
                return true;
            }
            else
            {
                return G();
            }
        }
        private bool G()
        {

            if (lookahead == "(")
            {
                MatchToken("(");
                Expr();
                MatchToken(")");
                return true;
            }
            else if (lookahead == "this")
            {
                MatchToken("this");
                return true;
            }
            else if (lookahead == "New")
            {
                // duad sobre si se haria backtraking
                MatchToken("New");
                MatchToken("(");
                MatchToken("identificador");
                MatchToken(")");
                return true;
            }
            else
            {
                return Constant() || Lvalue();
            }
        }

        private bool Constant()
        {
            if (lookahead == "entero")
            {
                MatchToken("entero");
                return true;
            }
            else if (lookahead == "booleanas")
            {
                MatchToken("booleanas");
                return true;
            }
            else if (lookahead == "doubles")
            {
                MatchToken("doubles");
                return true;
            }
            else if (lookahead == "cadena")
            {
                MatchToken("cadena");
                return true;
            }
            else if (lookahead == "null")
            {
                // duda si null es así tal cual
                MatchToken("null");
                return true;
            }
            else
            {
                return false;
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
