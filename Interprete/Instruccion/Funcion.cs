using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Funcion : Instruccion
    {
        private Simbolo_Funcion funcion;
        private LinkedList<Instruccion> instrucciones;
        private LinkedList<Instruccion> sentencias;
        public Funcion(Simbolo_Funcion funcion, LinkedList<Instruccion> instrucciones, LinkedList<Instruccion> sentencias)
        {
            this.funcion = funcion;
            this.instrucciones = instrucciones;
            this.sentencias = sentencias;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            funcion.listaInst = this.instrucciones;
            funcion.listaSent = this.sentencias;
            ts.declararFuncion(this.funcion.Id, this.funcion);
            return null;
        }
    }
}
