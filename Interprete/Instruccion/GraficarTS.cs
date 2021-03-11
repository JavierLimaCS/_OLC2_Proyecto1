using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class GraficarTS : Instruccion
    {
        public GraficarTS() 
        {
            
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            ts.generarTS();
            return null;
        }
    }
}
