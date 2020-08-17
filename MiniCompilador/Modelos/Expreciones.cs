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
        private readonly Expreciones palabrasReservadas;
        private readonly Expreciones identificador;
        private readonly Expreciones digitos;
        private readonly Expreciones caracteres;
        private readonly Expreciones comentariosLinea;
        private readonly Expreciones comentariosMultiples;

        /// <summary>
        /// Propiedades para que se pueda acesar a las expresiones regulares desde otras clases sin que puedan modificarlas
        /// </summary>
        public Expreciones palabrasReservadas_ { get => palabrasReservadas; }
        public Expreciones identificador_ { get => identificador; }
        public Expreciones digitos_ { get => digitos; }
        public Expreciones caracteres_ { get => caracteres; }
        public Expreciones comentariosLinea_ { get => comentariosLinea; }
        public Expreciones comentariosMultiples_ { get => comentariosMultiples; }
        
        public Expreciones()
        {
            palabrasReservadas = @"^(TOKEN)(\t |\s)*[0 - 9] + (\t |\s)*= (\t |\s)*(\s |\*|\+|\?|\(|\)|\|| '.' |[A - Z] +|{|})+(\t |\s)*$";



        }
        
        
    }

}

