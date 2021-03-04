using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class For : Instruccion
    {
        String id;
        Expresion.Expresion inicio;
        Expresion.Expresion final;
        LinkedList<Instruccion> instruccions;
        List<object> salida;
        public For(String id, Expresion.Expresion inicio, Expresion.Expresion final, LinkedList<Instruccion> inst) 
        {
            this.id = id;
            this.inicio = inicio;
            this.final = final;
            this.instruccions = inst;
            this.salida = new List<object>();
        }

        public override object Ejecutar(TabladeSimbolos ts)
        {
            int init = int.Parse(this.inicio.Evaluar(ts).Value.ToString());
            int end = int.Parse(this.final.Evaluar(ts).Value.ToString());
            for (int i = init; i <= end; i++) 
            {
                foreach (var inst in this.instruccions) 
                {
                    if (init == end) break;
                    ts.setVariableValor(this.id, i);
                    Object output = inst.Ejecutar(ts);
                    if (output is List<Object>)
                    {
                        this.salida.AddRange((List<Object>)output);
                    }
                    else
                    {
                        this.salida.Add(output);
                    }
                }
            }

            return salida;
        }
    }
}
