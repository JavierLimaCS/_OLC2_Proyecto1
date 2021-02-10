using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.Interprete.Expresion
{
    class Aritmetica : Expresion
    {
        private Expresion izquierda;
        private Expresion derecha;
        private char op;

        public Aritmetica(Expresion izquierda, Expresion derecha, char op)
        {
            this.izquierda = izquierda;
            this.derecha = derecha;
            this.op = op;
        }
        public override Object Evaluar()
        {
            Object izquierda = this.izquierda.Evaluar();
            Object derecha = this.derecha.Evaluar();
            switch (op)
            {
                case '+':
                    return izquierda + derecha;
                case '-':
                    return izquierda - derecha;
                case '*':
                    return izquierda * derecha;
                case '/':
                    return izquierda / derecha;
                default:
                    return izquierda % derecha;
            }
        }

    }
}
