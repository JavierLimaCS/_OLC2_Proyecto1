using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Case : Instruccion
    {
        Expresion.Expresion cond;
        List<Caso> casos;
        List<object> salida;
        public Case(Expresion.Expresion cond, List<Caso> casos) 
        {
            this.cond = cond;
            this.casos = casos;
            this.salida = new List<object>();
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            Object condicion = this.cond.Evaluar(ts).Value.ToString();
            Object casesito;
            foreach(var caso in this.casos) 
            {
                foreach (var cond in caso.Condiciones) 
                {
                    casesito = cond.Evaluar(ts).Value.ToString();
                    if (condicion.Equals(casesito)) 
                    {
                        foreach (var inst in caso.Sentencias) {
                            if (inst != null)
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
            return this.salida;
        }
    }
}
