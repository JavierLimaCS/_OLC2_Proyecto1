using Proyecto1.Codigo3D;
using Proyecto1.Interprete.Expresion;
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
            string code = "";
            string lrec = "";
            string lasign = "";
            string lcond = "";
            string linst = "";
            string lv = "", lf = "";
            string var ="", exp_ini ="", exp_fin = "";
            bool param = false;
            string[] varpos = ts.getVariablePos(this.id).Split(':');
            if (varpos[1].Equals("global"))
            {
                var = "Heap[(int)" + varpos[0] +"]";
            }
            else if (varpos[1].Equals("param"))
            {
                var = inter.tmp.generarTemporal() + " = SP + " + varpos[0] + "; //posicion de parametro " + varpos[2] + "\n";
                string tmp_param = inter.tmp.getLastTemporal();
                var += "Stack[(int)" + tmp_param + "]";
                param = true;
            }
            else 
            {
                var = "Stack[(int)" + varpos[0] + "]";
            }
            code += "//--- Sentencia Loop For ---//\n";
            code += "//-- Expresion de  Inicio \n";
            if (this.inicio is Primitivo)
            {
                exp_ini += this.inicio.generar3D(ts, inter);
                if (exp_ini.Contains("Stack") || exp_ini.Contains("Heap"))
                {
                    code += exp_ini;
                    code += var + " = " + inter.tmp.getLastTemporal() + ";\n";
                }
                else 
                {
                    code += var + " = " + exp_ini + ";\n";
                }
            }
            else 
            {
                code += this.inicio.generar3D(ts, inter);
            }
            string tmp_ini = inter.tmp.generarTemporal();
            if (param) 
            {
                string [] parame = var.Split('\n');
                var = parame[1].Trim('=');
                code += tmp_ini + " = " + var + ";\n";
            }
            else 
            {
                code += tmp_ini + " = " + var + ";\n";
            }
            string tmp_fin = "";
            if (this.final is Primitivo)
            {
                exp_fin += this.final.generar3D(ts, inter);
                if (exp_fin.Contains("Stack") || exp_fin.Contains("Heap"))
                {
                    code += "//-- Expresion de Final \n";
                    code += exp_fin + "\n";
                    tmp_fin = inter.tmp.getLastTemporal();
                }
                else 
                {
                    tmp_fin = exp_fin;
                }
            }
            else
            {
                code += "//-- Expresion de Final \n";
                code += this.final.generar3D(ts, inter);
                tmp_fin += inter.tmp.getLastTemporal();
            }
            lrec = inter.label.generarLabel();
            inter.lrecursives.Push(lrec);
            code += lrec + ": //etiqueta recursiva\n";
            if (this.reverse)
            {
                lv = inter.label.generarLabel();
                code += "if(" + tmp_ini + ">=" + tmp_fin + ") goto " + lv + ";\n";
            }
            else 
            {
                lv = inter.label.generarLabel();
                code += "if(" + tmp_ini + "<=" + tmp_fin + ") goto " + lv + ";\n";
            }
            lf = inter.label.generarLabel();
            inter.lbreaks.Push(lf);
            code += "goto " + lf + ";\n";
            lasign = inter.label.generarLabel();
            code += lasign + ":\n";
            if (this.reverse) 
            {
                code += tmp_ini + " = " + tmp_ini + " - 1; \n";
                code += var + " = " + tmp_ini + ";\n"; 
            }
            else 
            {
                code += tmp_ini + " = " + tmp_ini + " + 1; \n";
                code += var + " = " + tmp_ini + ";\n";
            }
            code += "goto " + lrec + ";\n";
            code += lv + ":\n";
            foreach (var i in this.instruccions) 
            {
                code += i.generar3D(ts, inter);
            }
            code += "goto " + lasign + ";\n";
            code += lf + ":\n";
            inter.lrecursives.Pop();
            inter.lbreaks.Pop();
            return code + "\n";
        }
    }
}
