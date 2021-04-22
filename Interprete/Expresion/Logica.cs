using Proyecto1.Codigo3D;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.Interprete.Expresion
{
    class Logica : Expresion
    {
        public Expresion izquierda;
        public Expresion derecha;
        public char op;

        public Logica(Expresion izquierda, Expresion derecha, char op)
        {
            this.izquierda = izquierda;
            this.derecha = derecha;
            this.op = op;
        }
        public override Simbolo Evaluar(TabladeSimbolos ts)
        {
            Simbolo izquierda = this.izquierda.Evaluar(ts);
            Simbolo derecha = null;
            Simbolo resultado;
            Tipos tipoResultante;
            if (this.derecha != null)
            {
                derecha = this.derecha.Evaluar(ts);
                tipoResultante = TablaTipos.getTipo(izquierda.Tipo, derecha.Tipo);
            }
            else
            {
                tipoResultante = TablaTipos.getTipo(izquierda.Tipo, izquierda.Tipo);
            }

            if ((int)tipoResultante == 7)
                throw new Exception();

            switch (op)
            {
                case 'a':
                    resultado = new Simbolo(null, new Tipo(Tipos.BOOLEAN, "boolean"), 0, 0, false);
                    resultado.Value = bool.Parse(izquierda.Value.ToString()) & bool.Parse(derecha.Value.ToString());
                    return resultado;
                case 'o':
                    resultado = new Simbolo(null, new Tipo(Tipos.BOOLEAN, "boolean"), 0, 0, false);
                    resultado.Value = bool.Parse(izquierda.Value.ToString()) || bool.Parse(derecha.Value.ToString());
                    return resultado;
                default:
                    resultado = new Simbolo(null, new Tipo(Tipos.BOOLEAN, "boolean"), 0, 0, false);
                    resultado.Value = !bool.Parse(izquierda.Value.ToString());
                    return resultado;
            }
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio c3d)
        {
            string code = "";
            if (this.derecha == null)
            {
                string lt = "";
                string lf = "";
                
                if (this.izquierda is Primitivo)
                {
                    code += "//Operacion logica NOT \n";
                    code += "if(" + this.izquierda.generar3D(ts, c3d) + "==1) goto " + c3d.label.generarLabel() + ";\n";
                    lf = c3d.label.getLastLabel();
                    code += c3d.tmp.generarTemporal() + " = 1;\n";
                    code += "goto " + c3d.label.generarLabel() + ";\n";
                    lt = c3d.label.getLastLabel();
                    code += lf + ":\n";
                    code += c3d.tmp.getLastTemporal() + " = 0;\n";
                    code += lt + ":\n";
                }
                else
                {
                    code += this.izquierda.generar3D(ts, c3d);
                    code += "//Operacion logica NOT \n";
                    code += "if(" + c3d.tmp.getLastTemporal()+ "==1) goto " + c3d.label.generarLabel() + ";\n";
                    lf = c3d.label.getLastLabel();
                    code += c3d.tmp.generarTemporal() + " = 1;\n";
                    code += "goto " + c3d.label.generarLabel() + ";\n";
                    lt = c3d.label.getLastLabel();
                    code += lf + ":\n";
                    code += c3d.tmp.getLastTemporal() + " = 0;\n";
                    code += lt + ":\n";
                }
            }
            else
            { ///FALTA implementar cuando hay primitivos con RElacionales o Logicas
                string lt = "";
                string lfa = "";
                List<string> lv = new List<string>();
                List<string> lf = new List<string>();
                if (this.izquierda is Primitivo && this.derecha is Primitivo)
                {
                    switch (this.op)
                    {
                        case 'a':
                            //-------------------------Lado Izquierdo
                            code += "//Operacion logica AND \n";
                            code += "if(" + this.izquierda.generar3D(ts, c3d) + "==1) goto " + c3d.label.generarLabel() + ";\n";
                            lv.Add(c3d.label.getLastLabel());
                            code += c3d.tmp.generarTemporal() + " = 0;\n";
                            code += "goto " + c3d.label.generarLabel() + ";\n";
                            lf.Add(c3d.label.getLastLabel());
                            code += lv[lv.Count - 1] + ":\n";
                            //--------------------------Lado Derecho
                            code += "if(" + this.derecha.generar3D(ts, c3d) + "==1) goto " + c3d.label.generarLabel() + ";\n";
                            lv.Add(c3d.label.getLastLabel());
                            code += c3d.tmp.getLastTemporal() + " = 0;\n";
                            code += "goto " + c3d.label.generarLabel() + ";\n";
                            lf.Add(c3d.label.getLastLabel());
                            code += lv[lv.Count - 1] + ":\n";
                            code += c3d.tmp.getLastTemporal() + " = 1;\n";
                            foreach (var f in lf)
                            {
                                code += f + ":\n\n";
                            }
                            break;
                        case 'o':
                            break;
                    }
                }
                else if (this.izquierda is Relacional && this.derecha is Relacional)
                {
                    string tmp_d = "";
                    string tmp_izq = "";
                    switch (this.op)
                    {
                        case 'a':
                            //-------------------------Lado Izquierdo
                            code += this.izquierda.generar3D(ts, c3d);
                            tmp_izq = c3d.tmp.getLastTemporal();
                            code += this.derecha.generar3D(ts, c3d);
                            tmp_d = c3d.tmp.getLastTemporal();
                            code += "//Operacion logica AND \n";
                            code += "if(" + tmp_izq + "==1) goto " + c3d.label.generarLabel() + ";\n";
                            lv.Add(c3d.label.getLastLabel());
                            code += "goto " + c3d.label.generarLabel() + ";\n";
                            lf.Add(c3d.label.getLastLabel());
                            code += lv[lv.Count - 1] + ":\n";
                            //--------------------------Lado Derecho
                            code += "if(" + tmp_d + "==1) goto " + c3d.label.generarLabel() + ";\n";
                            lv.Add(c3d.label.getLastLabel());
                            code += c3d.tmp.getLastTemporal() + " = 0;\n";
                            code += "goto " + c3d.label.generarLabel() + ";\n";
                            lf.Add(c3d.label.getLastLabel());
                            code += lv[lv.Count - 1] + ":\n";
                            code += c3d.tmp.getLastTemporal() + " = 1;\n";
                            foreach (var f in lf)
                            {
                                code += f + ":\n\n";
                            }
                            break;
                        case 'o':
                            break;
                    }
                }
                
                else
                {
                    string tmp_d = "";
                    string tmp_izq = "";
                    switch (this.op)
                    {
                        case 'a':
                            //-------------------------Lado Izquierdo
                            code += this.izquierda.generar3D(ts, c3d);
                            tmp_izq = c3d.tmp.getLastTemporal();
                            code += this.derecha.generar3D(ts, c3d);
                            tmp_d = c3d.tmp.getLastTemporal();
                            code += "//Operacion logica AND \n";
                            code += "if(" + tmp_izq + "==1) goto " + c3d.label.generarLabel() + ";\n";
                            lv.Add(c3d.label.getLastLabel());
                            code += "goto " + c3d.label.generarLabel() + ";\n";
                            lf.Add(c3d.label.getLastLabel());
                            code += lv[lv.Count - 1] + ":\n";
                            //--------------------------Lado Derecho
                            code += "if(" + tmp_d + "==1) goto " + c3d.label.generarLabel() + ";\n";
                            lv.Add(c3d.label.getLastLabel());
                            code += c3d.tmp.getLastTemporal() + " = 0;\n";
                            code += "goto " + c3d.label.generarLabel() + ";\n";
                            lf.Add(c3d.label.getLastLabel());
                            code += lv[lv.Count - 1] + ":\n";
                            code += c3d.tmp.getLastTemporal() + " = 1;\n";
                            foreach (var f in lf)
                            {
                                code += f + ":\n\n";
                            }
                            break;
                        case 'o':
                            break;
                    }
                }
            }
            return code + "\n";
        }
    }
}
