using _OLC2_Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2_Proyecto1.Interprete.Instruccion
{
    abstract class Instruccion
    {
        public void Ejecutar(TabladeSimbolos TS, string ambito) {}
    }
}
