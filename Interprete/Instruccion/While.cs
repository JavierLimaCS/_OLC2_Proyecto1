using Proyecto1.Codigo3D;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class While : Instruccion
    {
        private Expresion.Expresion valor;
        private LinkedList<Instruccion> instrucciones;
        object salida;
        public While(Expresion.Expresion valor, LinkedList<Instruccion> instrucciones)
        {
            this.valor = valor;
            this.instrucciones = instrucciones;
            this.salida = "";
            this.Semanticos = new List<Analisis.Error>();
        }

        public override object Ejecutar(TabladeSimbolos ts)
        {
            Simbolo valor = this.valor.Evaluar(ts);
            if (valor == null)
            {
                this.listaErrores.Add(new Analisis.Error("Semantico", "La condicion es incorrecta en sentencia WHILE", 0, 0));
                return this.salida;
            }
            if (valor.Tipo.tipo != Tipos.BOOLEAN)
            {
                this.listaErrores.Add(new Analisis.Error("Semantico", "La condicion no es BOOLEANA en sentencia WHILE", 0, 0));
                return this.salida;
            }

                while (bool.Parse(valor.Value.ToString())) 
            {
                try
                {
                    foreach (var instruccion in this.instrucciones)
                    {
                        Object output = instruccion.Ejecutar(ts);
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
                    valor = this.valor.Evaluar(ts);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return this.salida;
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string code = "//--- Sentencia Loop While ---//\n";
            string recursive_lbl = "";
            string lf = "";
            string lv = "";
            code += inter.label.generarLabel() + ":     //etiqueta recursiva \n";
            recursive_lbl = inter.label.getLastLabel();
            inter.lrecursives.Push(recursive_lbl);
            code += "//--- codigo de condicion \n";
            code += this.valor.generar3D(ts, inter);
            code += "//validacion de la condicion\n";
            code += "if (" + inter.tmp.getLastTemporal() + "==1) goto " + inter.label.generarLabel() + ";\n";
            lv = inter.label.getLastLabel();
            code += "goto " + inter.label.generarLabel() + ";\n";
            lf = inter.label.getLastLabel();
            int indice = inter.label.labels.Count;
            code += lv + ": \n";
            foreach (var inst in this.instrucciones)
            {
                code += inst.generar3D(ts, inter);
            }
            code += "goto " + recursive_lbl+ ";\n";
            code += lf + ":\n";
            inter.lrecursives.Pop();
            return code;
        }
    }
}
