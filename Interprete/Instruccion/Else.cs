using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Else : Instruccion
    {
        private LinkedList<Instruccion> sentencias;
        object salida;
        public Else(LinkedList<Instruccion> sentencias)
        {
            this.sentencias = sentencias;
            this.salida = new List<object>();
            this.Semanticos = new List<Analisis.Error>();
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            foreach (var instruccion in sentencias)
            {
                if (instruccion != null)
                {
                    Object output = instruccion.Ejecutar(ts);
                    if (output is Break)
                    {
                        return output;
                    }
                    else if (output is Continue)
                    {
                        break;
                    }
                    else if (output is Exit)
                    {
                        return output;
                    }
                    else
                    {
                        this.salida = output;
                    }
                }
            }
            return this.salida;
        }
    }
}
