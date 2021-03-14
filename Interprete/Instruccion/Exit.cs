using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Exit : Instruccion
    {
        Expresion.Expresion exp;
        public object valor_exit;
        public Exit(Expresion.Expresion exp) 
        {
            this.exp = exp;
            this.valor_exit = null;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            this.valor_exit = this.exp.Evaluar(ts);
            return this;
        }
    }
}
