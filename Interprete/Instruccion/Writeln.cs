using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Writeln : Instruccion
    {
        private Expresion.Expresion exp;
        public Writeln(Expresion.Expresion exp) 
        {
            this.exp = exp;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            object valor = this.exp.Evaluar(ts);
            return valor.ToString();   
        }
    }
}
