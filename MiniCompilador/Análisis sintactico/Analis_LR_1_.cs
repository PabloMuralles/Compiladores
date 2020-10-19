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
        Dictionary<int, string> tables_dictionary = new Dictionary<int, string>();
        private void Import_table()
        {
            tables_dictionary = new Dictionary<int, string>()
            {
               {0,"ident☺class☺interface☺const☺void☺int☺double☺bool☺string☺Program☺Decl☺Reserved☺Type"},
              {1,"$"},
{2,"ident☺class☺interface☺const☺void☺int☺double☺bool☺string☺$☺Program☺Decl☺Reserved☺Type"},
{3,"ident☺[]"},
{4,"ident"},
{5,"ident"},
{6,"ident"},
{7,"int☺double☺bool☺string☺ConstType"},
{8,"ident☺[]"},
{9,"ident☺[]"},
{10,"ident☺[]"},
{11,"ident☺[]"},
{12,"ident☺[]"},
{13,"ident"},
{14,"$"},
{15,";"},
{16,"ident☺[]"},
{17,"("},
{18,"ident☺class☺{☺interface☺const☺void☺int☺double☺bool☺string☺,☺:☺$☺Id"},
{19,"{"},
{20,"ident"},
{21,"ident"},
{22,"ident"},
{23,"ident"},
{24,"ident"},
{25,"ident☺class☺interface☺const☺void☺int☺double☺bool☺string☺$"},
{26,"ident☺int☺double☺bool☺string☺Type☺Formals"},
{27,"{☺,☺Id’"},
{28,"ident"},
{29,"ident☺}☺void☺int☺double☺bool☺string☺Type☺Prototype’☺Prototype"},
{30,";"},
{31,")"},
{32,"ident☺[]"},
{33,"{"},
{34,"ident"},
{35,"ident☺class☺{☺interface☺const☺void☺int☺double☺bool☺string☺,☺$"},
{36,"}"},
{37,"ident☺}☺void☺int☺double☺bool☺string☺Type☺Prototype’☺Prototype"},
{38,"ident☺[]"},
{39,"ident"},
{40,"ident☺class☺interface☺const☺void☺int☺double☺bool☺string☺$"},
{41,"{☺StmtBlock"},
{42,")☺,☺"},
{43,"ident☺}☺const☺void☺int☺double☺bool☺string☺Reserved☺Type☺Field’☺Field"},
{44,"{☺,☺Id’"},
{45,"ident☺class☺interface☺const☺void☺int☺double☺bool☺string☺$"},
{46,"}"},
{47,"("},
{48,"("},
{49,"ident☺class☺interface☺const☺void☺int☺double☺bool☺string☺$"},
{50,"ident☺;☺(☺class☺{☺}☺interface☺const☺void☺int☺double☺bool☺string☺if☺while☺for☺break☺return☺Console☺-☺!☺this☺New☺intConstant☺doubleConstant☺boolConstant☺stringConstant☺null☺$☺Type☺VariableDecl’"},
{51,"ident☺int☺double☺bool☺string☺Type☺Formals"},
{52,"}"},
{53,"ident☺}☺const☺void☺int☺double☺bool☺string☺Reserved☺Type☺Field’☺Field"},
{54,"ident☺(☺[]"},
{55,"("},
{56,"int☺double☺bool☺string☺ConstType"},
{57,"ident☺(☺[]"},
{58,"ident☺(☺[]"},
{59,"ident☺(☺[]"},
{60,"ident☺(☺[]"},
{61,"ident☺(☺[]"},
{62,"("},
{63,"{"},
{64,"ident☺int☺double☺bool☺string☺Type☺Formals"},
{65,"ident☺int☺double☺bool,string☺Type☺Formals"},
{66,"ident☺;☺(☺class☺{☺}☺interface☺const☺void☺int☺double☺bool☺string☺if☺while☺for☺break☺return☺Console☺-☺!☺this☺New☺intConstant☺doubleConstant☺boolConstant☺stringConstant☺null☺$☺ConstDecl’"},
{67,"ident☺[]"},
{68,")"},
{69,"ident☺class☺interface☺const☺void☺int☺double☺bool☺string☺$"},
{70,"}"},
{71,";"},
{72,"ident☺(☺[]"},
{73,"ident☺int☺double☺bool☺string☺Type☺Formals☺"},
{74,"ident"},
{75,")"},
{76,")"},
{77,"ident☺;☺(☺{☺}☺if☺while☺for☺break☺return☺Console☺-☺!☺this☺New☺intConstant☺doubleConstant☺boolConstant☺stringConstant☺null☺StmtBlock☺Stmt’☺Stmt☺Expr☺ConditionAnd☺Equality☺Relational☺Additive☺Multiplicative☺Unary☺Primary☺Terminal"},
{78,"int☺double☺bool☺string☺ConstType"},
{79,";"},
{80,"ident☺}☺const☺void☺int☺double☺bool☺string"},
{81,")"},
{82,";"},
{83,";"},
{84,";"},
            };
        }
        public void table(List<Tuple<string, string>> tokens_)
        {
            Import_table();
            tokens_.Add(new Tuple<string, string>("$", "")); //Fin de linea
            Stack<int> pila = new Stack<int>();
            pila.Push(0);


            if (tables_dictionary.ContainsKey(pila.Peek()))
            {
                var array_symbol = tables_dictionary[pila.Peek()].Split('☺');
                search_symbol(array_symbol);
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
