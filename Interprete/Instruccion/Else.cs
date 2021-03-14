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
        List<object> salida;
        public Else(LinkedList<Instruccion> sentencias)
        {
            this.sentencias = sentencias;
            this.salida = new List<object>();
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            String print = "";
            foreach (var instruccion in sentencias)
            {
                if (instruccion != null)
                {
                    Object output = instruccion.Ejecutar(ts);
                    if (output is List<object>)
                    {
                        this.salida.AddRange((List<object>)output);
                    }
                    else if (output is Break)
                    {
                        return output;
                    }
                    else if (output is Continue)
                    {
                        break;
                    }
                    else if (output is Exit)
                    {

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
