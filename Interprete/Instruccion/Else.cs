using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Else : Instruccion
    {
        private LinkedList<Instruccion> sentencias;
        List<Object> salida;
        public Else(LinkedList<Instruccion> sentencias)
        {
            this.sentencias = sentencias;
            this.salida = new List<object>();
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            foreach (var instruccion in sentencias)
            {
                if (instruccion != null)
                {
                    Object output = instruccion.Ejecutar(ts);
                    if (output is List<Object>)
                    {
                        this.salida.AddRange((List<Object>)output);
                    }
                    else if (output is Break)
                    {
                        return salida;
                    }
                    else
                    {
                        this.salida.Add(output);
                    }
                }
            }
            return this.salida;
        }
    }
}
