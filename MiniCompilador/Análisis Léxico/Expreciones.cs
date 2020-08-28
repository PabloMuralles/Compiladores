using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MiniCompilador.Análisis_Léxico
{
    class Expreciones
    {      
        /// <summary>
        /// Varibalbes globales de las expresiones regulares
        /// </summary>
        private readonly Regex palabrasReservadas;
        private readonly Regex identificador;
        private readonly Regex booleanas;
        private readonly Regex entero;
        private readonly Regex hexadecimal;
        private readonly Regex doubles;
        private readonly Regex cadena;
        private readonly Regex caracteresSimples;
        private readonly Regex caracteresDobles;
        private readonly Regex signosPuntuacion;
        private readonly Regex llavesSimples;
        private readonly Regex llavesDobles;
        private readonly Regex espacioBlanco;
        private readonly Regex letras;
        private readonly Regex caracteres;
        private readonly Regex cadenaNulo;
       

        /// <summary>
        /// Propiedades para que se pueda acesar a las expresiones regulares desde otras clases sin que puedan modificarlas
        /// </summary>
        public Regex palabrasReservadas_ { get => palabrasReservadas; }
        public Regex identificador_ { get => identificador; }
        public Regex booleanas_ { get => booleanas; }
        public Regex entero_ { get => entero; }
        public Regex doubles_ { get => doubles; }
        public Regex hexadecimal_ { get => hexadecimal; }
        public Regex cadena_ { get => cadena; }
        public Regex caracteresSimples_ { get => caracteresSimples; }
        public Regex caracteresDobles_ { get => caracteresDobles; }
        public Regex signosPuntuacion_ { get => signosPuntuacion; }
        public Regex llavesSimples_ { get => llavesSimples; }
        public Regex llavesDobles_ { get => llavesDobles; }
        public Regex espacioBlanco_ { get => espacioBlanco; }
        public Regex letras_ { get => letras; }
        public Regex caracteres_ { get => caracteres; }
        public Regex cadenaNulo_ { get => cadenaNulo; }




        public Expreciones()
        {
            palabrasReservadas = new Regex(@"^(void|int|double|bool|string|class|interface|null|this|for|while|foreach|if|else|return|break|New|NewArray|Console|WriteLine)$");

            booleanas = new Regex("true|false");

            identificador = new Regex(@"^([A-Za-z][A-Za-z_0-9]*)*$");

            entero = new Regex(@"^([0-9]+)*$");

            hexadecimal = new Regex(@"^0(x|X)[0-9A-Fa-f]+$");

            doubles = new Regex(@"^(([0-9]+.[0-9]+|.[0-9]+)|([0-9]+.(E|e)(-|\+)?[0-9]+))$");

            cadena= new Regex("(\"[^\"\n]\")|(\"\")");

            cadenaNulo = new Regex("(\"[^\"\n\0]\")");

            caracteresSimples = new Regex(@"^(\+|-|\*|/|%|<|>|=)$");

            caracteresDobles = new Regex(@"^(<=|>=|==|!=|&&|\|\|)$");

            signosPuntuacion = new Regex(@"^(!|;|,|\.)$");

            llavesSimples = new Regex(@"^(\[|\]|\(|\)|{|})$");

            llavesDobles = new Regex(@"^(\[\]|\(\)|{})$");

            espacioBlanco = new Regex("\" \"");

            letras = new Regex(@"^([a-z]|[A-Z])$");

            caracteres = new Regex(@"^(\+|-|\*|/|%|<|<=|>|>=|=|==|!=|&&|\|\||!|;|,|\.|\[|\]|\(|\)|{|}|\[\]|\(\)|{})$");



            
        }


    }

}

