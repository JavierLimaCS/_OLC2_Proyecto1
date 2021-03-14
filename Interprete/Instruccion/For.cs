using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
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
            String print = "";
            for(int i = init; i<=end; i++)
            {
                ts.setVariableValor(this.id, init);
                foreach (var inst in this.instruccions) 
                {
                    Object output = inst.Ejecutar(ts);
                    if (output is List<object>)
                    {
                        this.salida.AddRange((List<object>)output);
                    }
                    else if (output is Break)
                    {
                        return "";
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
                if (init == end) break;
                init++;
            }
            return this.salida;
        }
    }
}
