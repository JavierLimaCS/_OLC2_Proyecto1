using Proyecto1.Codigo3D;
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
            ts.generarTS(ts.alias);
            return null;
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            return "";
        }
    }
}
