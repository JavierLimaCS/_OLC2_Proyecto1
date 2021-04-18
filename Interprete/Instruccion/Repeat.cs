using Proyecto1.Codigo3D;
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

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string code = "//--- Sentencia Loop Repeat ---//\n";
            string recursive_lbl = "";
            string lf = "";
            string lv = "";
            code += inter.label.generarLabel() + ":     //etiqueta recursiva \n";
            recursive_lbl = inter.label.getLastLabel();
            foreach (var inst in this.instrucciones)
            {
                code += inst.generar3D(ts, inter);
            }
            code += "//--- codigo de condicion \n";
            code += this.condicion.generar3D(ts, inter);
            code += "//validacion de la condicion\n";
            code += "if (" + inter.tmp.getLastTemporal() + "==1) goto " + inter.label.generarLabel() + ";\n";
            lv = inter.label.getLastLabel();
            code += "goto " + inter.label.generarLabel() + ";\n";
            lf = inter.label.getLastLabel();
            code += lf + ": \n";
            code += "goto " + recursive_lbl + ";\n";
            code += lv + ":\n";
            return code;
        }
    }
}
