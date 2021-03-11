using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.Interprete.Expresion
{
    class Logica : Expresion
    {
        private Expresion izquierda;
        private Expresion derecha;
        private char op;

        public Logica(Expresion izquierda, Expresion derecha, char op)
        {
            this.izquierda = izquierda;
            this.derecha = derecha;
            this.op = op;
        }
        public override Simbolo Evaluar(TabladeSimbolos ts)
        {
            Simbolo izquierda = this.izquierda.Evaluar(ts);
            Simbolo derecha = this.derecha.Evaluar(ts);
            Simbolo resultado;
            Tipos tipoResultante = TablaTipos.getTipo(izquierda.Tipo, derecha.Tipo);

            //if (tipoResultante != Tipos.INT && tipo != '+')
            //  throw new Exception();

            switch (op)
            {
                case 'a':
                    resultado = new Simbolo(null, new Tipo(Tipos.BOOLEAN, "boolean"), 0, 0);
                    resultado.Value = bool.Parse(izquierda.Value.ToString()) & bool.Parse(derecha.Value.ToString());
                    return resultado;
                case 'o':
                    resultado = new Simbolo(null, new Tipo(Tipos.BOOLEAN, "boolean"), 0, 0);
                    resultado.Value = bool.Parse(izquierda.Value.ToString()) || bool.Parse(derecha.Value.ToString());
                    return resultado;
                default:
                    resultado = new Simbolo(null, new Tipo(Tipos.BOOLEAN, "boolean"), 0, 0);
                    resultado.Value = !bool.Parse(izquierda.Value.ToString());
                    return resultado;
            }
        }
    }
}
