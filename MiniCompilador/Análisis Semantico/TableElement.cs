using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minic.Análisis_Semantico
{
    class TableElement
    {
        public string name { get; set; }
        public string value { get; set; }
        public string type { get; set; }
        public string ambit { get; set; }
        public bool isFunction { get; set; }
        public bool isClass { get; set; }


    }
}
