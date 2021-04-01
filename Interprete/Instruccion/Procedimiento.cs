using Proyecto1.Codigo3D;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Procedimiento : Instruccion
    {
        private Simbolo_Funcion proc;
        private LinkedList<Instruccion> instrucciones;
        private LinkedList<Instruccion> sentencias;
        public Procedimiento(Simbolo_Funcion proc, LinkedList<Instruccion> instrucciones, LinkedList<Instruccion> sentencias) 
        {
            this.proc = proc;
            this.instrucciones = instrucciones;
            this.sentencias = sentencias;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            proc.listaInst = this.instrucciones;
            proc.listaSent = this.sentencias;
            ts.declararFuncion(this.proc.Id, this.proc);
            return null;
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            return "";
        }
    }
}
