using Proyecto1.TS;
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
        private char tipo;

        public Aritmetica(Expresion izquierda, Expresion derecha, char tipo)
        {
            this.izquierda = izquierda;
            this.derecha = derecha;
            this.tipo = tipo;
        }
        public override Simbolo Evaluar(TabladeSimbolos ts)
        {
            Simbolo izquierda = this.izquierda.Evaluar(ts);
            Simbolo derecha = this.derecha.Evaluar(ts);
            Simbolo resultado;
            Tipos tipoResultante = TablaTipos.getTipo(izquierda.Tipo, derecha.Tipo);

            //if (tipoResultante != Tipos.INT && tipo != '+')
              //  throw new Exception();

            switch (tipo)
            {
                case '+':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0);
                    resultado.Value = int.Parse(izquierda.Value.ToString()) + int.Parse(derecha.Value.ToString());
                    return resultado;
                case '-':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0);
                    resultado.Value = int.Parse(izquierda.Value.ToString()) - int.Parse(derecha.Value.ToString());
                    return resultado;
                case '*':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0);
                    resultado.Value = int.Parse(izquierda.Value.ToString()) * int.Parse(derecha.Value.ToString());
                    return resultado;
                case '/':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0);
                    resultado.Value = int.Parse(izquierda.Value.ToString()) / int.Parse(derecha.Value.ToString());
                    return resultado;
                default:
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0);
                    resultado.Value = int.Parse(izquierda.Value.ToString()) % int.Parse(derecha.Value.ToString());
                    return resultado;
            }
        }
    }
}
