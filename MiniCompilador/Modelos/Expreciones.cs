using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MiniCompilador.Modelos
{
    class Expreciones
    {
        /// <summary>
        /// Varibalbes globales de las expresiones regulares
        /// </summary>
        private readonly Regex palabrasReservadas;
        private readonly Regex identificador;
        private readonly Regex digitos;
        private readonly Regex caracteres;
        private readonly Regex comentariosLinea;
        private readonly Regex comentariosMultiples;

        /// <summary>
        /// Propiedades para que se pueda acesar a las expresiones regulares desde otras clases sin que puedan modificarlas
        /// </summary>
        public Regex palabrasReservadas_ { get => palabrasReservadas; }
        public Regex identificador_ { get => identificador; }
        public Regex digitos_ { get => digitos; }
        public Regex caracteres_ { get => caracteres; }
        public Regex comentariosLinea_ { get => comentariosLinea; }
        public Regex comentariosMultiples_ { get => comentariosMultiples; }
        
        public Expreciones()
        {
            palabrasReservadas = new Regex(@"^(TOKEN)(\t |\s)*[0 - 9] + (\t |\s)*= (\t |\s)*(\s |\*|\+|\?|\(|\)|\|| '.' |[A - Z] +|{|})+(\t |\s)*$");



        }
        
        
    }

}

