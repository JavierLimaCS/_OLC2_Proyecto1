using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.Interprete.Instruccion
{
    abstract class Instruccion
    {
        public abstract object Ejecutar(TabladeSimbolos ts);
    }
}
