using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Repeat : Instruccion
    {
        object salida;
        Expresion.Expresion condicion;
        LinkedList<Instruccion> instrucciones;
        public Repeat(LinkedList<Instruccion> inst, Expresion.Expresion cond) 
        {
            this.instrucciones = inst;
            this.condicion = cond;
            this.Semanticos = new List<Analisis.Error>();
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            Simbolo cond;
            do
            {
                foreach (var inst in this.instrucciones) 
                {
                    Object output = inst.Ejecutar(ts);
                    if (output is Break)
                    {
                        return "";
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
                cond = this.condicion.Evaluar(ts);
            } 
            while (!bool.Parse(cond.Value.ToString()));
            return this.salida;
        }
    }
}
