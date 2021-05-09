using Proyecto1.Codigo3D;
using Proyecto1.Interprete.Expresion;
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
        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string code = "//------- Sentencia Decision Case\n";
            string eval = "";
            string lcase = "";
            string primi = "";
            if (inter.ls.Equals("")) inter.ls = inter.label.generarLabel();
            if (this.cond is Primitivo)
            {
                primi = this.cond.generar3D(ts,inter);
                if (primi.Contains("Heap") || primi.Contains("Stack"))
                {
                    code += primi + "\n";
                }
                else {
                    Primitivo primitibo = (Primitivo)this.cond;
                    if (primitibo.tipo == 'L')
                    {
                        code += primi;
                    }
                    else
                    {
                        code += inter.tmp.generarTemporal() + " = " + primi + ";\n";
                    }
                }
            }
            else
            {
                code += this.cond.generar3D(ts, inter);
            }
            eval = inter.tmp.getLastTemporal();
            string primi2 = "";
            foreach (var caso in this.casos)
            {
                foreach (var cond in caso.Condiciones) 
                {
                    if (cond is Primitivo)
                    {
                        primi2 = cond.generar3D(ts, inter);
                        if (primi2.Contains("Heap") || primi2.Contains("Stack"))
                        {
                            code += primi2 + "\n";
                        }
                        else
                        {
                            Primitivo primitibo2 = (Primitivo)this.cond;
                            if (primitibo2.tipo == 'L')
                            {
                                code += primi2;
                            }
                            else
                            {
                                code += inter.tmp.generarTemporal() + " = " + primi2 + ";\n";
                            }
                        }
                    }
                    else
                    {
                        code += cond.generar3D(ts, inter);
                    }
                    code += "if (" + eval + "!= " + inter.tmp.getLastTemporal() +") goto " + inter.label.generarLabel() + ";\n";
                    lcase = inter.label.getLastLabel();
                    foreach (var sent in caso.Sentencias) 
                    {
                        code += sent.generar3D(ts, inter);
                    }
                    code += "goto " + inter.ls + ";\n";
                    code += lcase + ":\n";
                }
            }
            if (this._else != null)
            {
                code += this._else.generar3D(ts, inter);
            }
            if (!(inter.ls.Equals("")))
            {
                code += inter.ls + ":\n";
                inter.ls = "";
            }
            return code;
        }
    }
}
