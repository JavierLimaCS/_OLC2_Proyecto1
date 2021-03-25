using Proyecto1.Codigo3D;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.Interprete.Expresion
{
    abstract class Expresion 
    {
        public abstract Simbolo Evaluar(TabladeSimbolos ts);

        public abstract string generar3D(TabladeSimbolos ts, Intermedio c3d);
        
    }
}
