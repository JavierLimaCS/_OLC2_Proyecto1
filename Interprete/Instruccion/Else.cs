using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Else : Instruccion
    {
        private Instruccion sentencia;
        private LinkedList<Instruccion> sentencias;
        public Else(Instruccion sentencia) 
        {
            this.sentencia = sentencia;
        }

        public Else(LinkedList<Instruccion> sentencias)
        {
            this.sentencias = sentencias;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            throw new NotImplementedException();
        }
    }
}
