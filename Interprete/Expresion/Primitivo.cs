using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Expresion
{
    class Primitivo : Expresion
    {

        private char tipo;
        private object valor;

        public Primitivo(char tipo, object valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }

        public override Simbolo Evaluar(TabladeSimbolos ts)
        {
            Simbolo primitivo = new Simbolo("primitivo", new Tipo(Tipos.INT, "integer"), 0, 0); ;
            if (this.tipo == 'N')
            {
                primitivo = new Simbolo("primitivo", new Tipo(Tipos.INT, "integer"), 0, 0);
            }
            primitivo.Value = this.valor;
            return primitivo;
        }
    }
}
