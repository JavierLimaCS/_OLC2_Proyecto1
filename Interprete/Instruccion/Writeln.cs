using Proyecto1.Codigo3D;
using Proyecto1.Interprete.Expresion;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Proyecto1.Interprete.Instruccion
{
    class Writeln : Instruccion
    {
        private LinkedList<Expresion.Expresion> exp_list;
        RichTextBox rc;
        public Writeln(LinkedList<Expresion.Expresion> exp_list, RichTextBox rt) 
        {
            this.exp_list = exp_list;
            this.rc = rt;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            object valor = "";
            foreach (var exp in this.exp_list) 
            {
                valor += exp.Evaluar(ts).Value.ToString().Replace("'","");
            }
            valor += "\n";
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
                if(valor != null) 
                {
                    switch (valor.Tipo.tipoAuxiliar)
                    {
                        case "string":
                            if (exp is Primitivo)
                            {
                                Primitivo primi = (Primitivo)exp;
                                if (primi.tipo == 'I')
                                {
                                    code += exp.generar3D(ts, inter) + "\n";
                                }
                                else
                                {
                                    code += exp.generar3D(ts, inter);
                                }
                                string tmp = inter.tmp.getLastTemporal();
                                string label_inicio = "";
                                string label_salida = "";
                                code += "//impresion de una cadena \n";
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
                                string busquedaID = exp.generar3D(ts, inter);
                                if (busquedaID.Contains("Heap") || busquedaID.Contains("Stack")) 
                                {
                                    code += busquedaID + "\n";
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
                        case "boolean":
                            if (valor.Value.ToString().ToLower().Equals("true"))
                            {
                                code += inter.tmp.generarTemporal() + "= HP; //se guarda referencia de inicio de cadena\n";
                                code += "Heap[(int)HP] = 84;  //T \n";
                                code += "HP = HP + 1;\n";
                                code += "Heap[(int)HP] = 82;  //R \n";
                                code += "HP = HP + 1; \n";
                                code += "Heap[(int)HP] = 85;  //U \n";
                                code += "HP = HP + 1;\n";
                                code += "Heap[(int)HP] = 69;  //E\n";
                                code += "HP = HP + 1;\n";
                                code += "Heap[(int)HP] = -1;\n";
                                code += "HP = HP + 1;\n";
                            }
                            else 
                            {
                                code += inter.tmp.generarTemporal() + "= HP; //se guarda referencia de inicio de cadena\n";
                                code += "Heap[(int)HP] = 70;  //F \n";
                                code += "HP = HP + 1;\n";
                                code += "Heap[(int)HP] = 65;  //A \n";
                                code += "HP = HP + 1; \n";
                                code += "Heap[(int)HP] = 76;  //L \n";
                                code += "HP = HP + 1;\n";
                                code += "Heap[(int)HP] = 83;  //S \n";
                                code += "HP = HP + 1;\n";
                                code += "Heap[(int)HP] = 69;  //E\n";
                                code += "HP = HP + 1;\n";
                                code += "Heap[(int)HP] = -1;\n";
                                code += "HP = HP + 1;\n";
                            }
                            string tmp_bool = inter.tmp.getLastTemporal();
                            string label_inicio_bool = "";
                            string label_salida_bool = "";
                            code += "//impresion de una cadena \n";
                            code += inter.label.generarLabel() + ":\n";
                            label_inicio_bool = inter.label.getLastLabel();
                            code += inter.tmp.generarTemporal() + " = Heap[(int)" + tmp_bool + "];\n";
                            code += "if(" + inter.tmp.getLastTemporal() + "==-1) goto " + inter.label.generarLabel() + ";\n";
                            label_salida_bool = inter.label.getLastLabel();
                            code += "printf(\"%c\", (char)" + inter.tmp.getLastTemporal() + "); \n";
                            code += tmp_bool + " = " + tmp_bool + " + 1;\n";
                            code += "goto " + label_inicio_bool + ";\n";
                            code += label_salida_bool + ":\n";
                            break;
                    }
                }
                
            }

            code += "printf(\"%c\", (char)10);\n";
            return code;
        }
    }
}
