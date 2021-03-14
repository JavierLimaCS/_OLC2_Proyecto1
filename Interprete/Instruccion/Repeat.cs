using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Repeat : Instruccion
    {
        List<object> salida;
        Expresion.Expresion condicion;
        LinkedList<Instruccion> instrucciones;
        public Repeat(LinkedList<Instruccion> inst, Expresion.Expresion cond) 
        {
            this.instrucciones = inst;
            this.condicion = cond;
            this.salida = new List<object>();
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            Simbolo cond;
            do
            {
                foreach (var inst in this.instrucciones) 
                {
                    Object output = inst.Ejecutar(ts);
                    if (output is List<Object>)
                    {
                        this.salida.AddRange((List<Object>)output);
                    }
                    else if (output is Break)
                    {
                        return salida;
                    }
                    else if (output is Continue)
                    {
                    }
                    else
                    {
                        this.salida.Add(output);
                    }
                }
                cond = this.condicion.Evaluar(ts);
            } 
            while (!bool.Parse(cond.Value.ToString()));
            return this.salida;
        }
    }
}
