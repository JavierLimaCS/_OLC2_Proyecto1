using Proyecto1.Codigo3D;
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
        bool reverse;
        object salida;
        public For(String id, Expresion.Expresion inicio, Expresion.Expresion final, LinkedList<Instruccion> inst, bool r) 
        {
            this.id = id;
            this.inicio = inicio;
            this.final = final;
            this.instruccions = inst;
            this.reverse = r;
            this.Semanticos = new List<Analisis.Error>();
        }

        public override object Ejecutar(TabladeSimbolos ts)
        {
            int init = int.Parse(this.inicio.Evaluar(ts).Value.ToString());
            int end = int.Parse(this.final.Evaluar(ts).Value.ToString());
            if (!reverse)
            {
                for (int i = init; i <= end; i++)
                {
                    ts.setVariableValor(this.id, init);
                    foreach (var inst in this.instruccions)
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
                    if (init == end) break;
                    init++;
                }

            }
            else 
            {
                for (int i = init; i >= end; i--)
                {
                    ts.setVariableValor(this.id, init);
                    foreach (var inst in this.instruccions)
                    {
                        Object output = inst.Ejecutar(ts);
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
                    if (init == end) break;
                    init--;
                }
            }
            return this.salida;
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            return "";
        }
    }
}
