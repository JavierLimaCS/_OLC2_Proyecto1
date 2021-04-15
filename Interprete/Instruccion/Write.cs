using Proyecto1.Codigo3D;
using Proyecto1.Interprete.Expresion;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Proyecto1.Interprete.Instruccion
{
    class Write : Instruccion
    {
        private LinkedList<Expresion.Expresion> exp_list;
        RichTextBox rc;
        public Write(LinkedList<Expresion.Expresion> exp_list, RichTextBox rc)
        {
            this.rc = rc;
            this.exp_list = exp_list;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            object valor = "";
            foreach (var exp in this.exp_list)
            {
                valor += exp.Evaluar(ts).Value.ToString().Replace("'", "");
            }
            rc.Text = rc.Text + valor.ToString();
            return "";
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string code = "";
            Simbolo valor;
            foreach (var exp in this.exp_list)
            {
                valor = (Simbolo)exp.Evaluar(ts);
                if (valor != null)
                {
                    switch (valor.Tipo.tipoAuxiliar)
                    {
                        case "string":
                            if (exp is Primitivo)
                            {
                                Primitivo primi = (Primitivo)exp;
                                if (primi.tipo == 'I')
                                {
                                    code += inter.tmp.generarTemporal() + " =" + exp.generar3D(ts, inter) + ";\n";
                                }
                                else
                                {
                                    code += exp.generar3D(ts, inter);
                                }
                                string tmp = inter.tmp.getLastTemporal();
                                string label_inicio = "";
                                string label_salida = "";
                                code += "//impresion de una cadea \n";
                                code += inter.label.generarLabel() + ":\n";
                                label_inicio = inter.label.getLastLabel();
                                code += inter.tmp.generarTemporal() + " = Heap[(int)" + tmp + "];\n";
                                code += "if(" + inter.tmp.getLastTemporal() + "==-1) goto " + inter.label.generarLabel() + ";\n";
                                label_salida = inter.label.getLastLabel();
                                code += "printf(\"%c\", (char)" + inter.tmp.getLastTemporal() + "); \n";
                                code += tmp + " = " + tmp + " + 1;\n";
                                code += "goto " + label_inicio + ";\n";
                                code += label_salida + ":\n";
                            }
                            break;
                        case "integer":
                            if (exp is Primitivo)
                            {
                                string busquedaID2 = exp.generar3D(ts, inter);
                                if (busquedaID2.Contains("Heap") || busquedaID2.Contains("Stack"))
                                {
                                    code += busquedaID2 + "\n";
                                }
                                else
                                {
                                    code += inter.tmp.generarTemporal() + "=";
                                    code += exp.generar3D(ts, inter) + ";\n";
                                }
                            }
                            else
                            {
                                code += exp.generar3D(ts, inter);
                            }
                            code += "printf(\"%d\", (int)" + inter.tmp.getLastTemporal() + "); \n";
                            break;
                        case "real":
                            if (exp is Primitivo)
                            {
                                string busquedaID2 = exp.generar3D(ts, inter);
                                if (busquedaID2.Contains("Heap") || busquedaID2.Contains("Stack"))
                                {
                                    code += busquedaID2 + "\n";
                                }
                                else
                                {
                                    code += inter.tmp.generarTemporal() + "=";
                                    code += exp.generar3D(ts, inter) + ";\n";
                                }
                            }
                            else
                            {
                                code += exp.generar3D(ts, inter);
                            }
                            code += "printf(\"%f\", (float)" + inter.tmp.getLastTemporal() + "); \n";
                            break;
                    }
                }

            }
            return code;
        }
    }
}
