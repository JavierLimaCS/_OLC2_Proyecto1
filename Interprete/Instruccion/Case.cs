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
        object salida;
        Instruccion _else;
        public Case(Expresion.Expresion cond, List<Caso> casos, Instruccion elsito) 
        {
            this.cond = cond;
            this.casos = casos;
            this._else = elsito;
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
            if (_else != null)
            {
                this.salida = _else.Ejecutar(ts);
            }
            return this.salida;
        }
    }
}
