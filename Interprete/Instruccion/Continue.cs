using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Continue : Instruccion
    {
        public override object Ejecutar(TabladeSimbolos ts)
        {
            return this;
        }
    }
}
