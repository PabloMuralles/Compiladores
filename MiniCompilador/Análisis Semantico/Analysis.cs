using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minic.Análisis_Semantico
{
    class Analysis
    {

        private List<Tuple<string, string>> listTokens = new List<Tuple<string, string>>();
        public Analysis(Queue<Tuple<string, string>> tokens_)
        {
            listTokens = tokens_.ToList();
        }
    }
}
