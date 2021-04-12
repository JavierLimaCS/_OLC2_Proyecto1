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
        private Expresion izquierda;
        private Expresion derecha;
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

            }
            else
            {
                string firstcond = "";
                string secondcond = "";
                List<string> lv = new List<string>();
                List<string> lf = new List<string>();
                switch (this.op)
                {
                    case 'a':
                        if (this.izquierda is Relacional)
                        {
                            Relacional rel_izq = (Relacional)this.izquierda;
                            firstcond += rel_izq.generar3D(ts, c3d);
                            firstcond += "if(" + c3d.tmp.getLastTemporal() + ") goto " + c3d.label.generarLabel() + ";\n";
                            lv.Add(c3d.label.getLastLabel());
                            firstcond += "goto " + c3d.label.generarLabel() + ";\n";
                            lf.Add(c3d.label.getLastLabel());
                        }
                        else
                        {
                            firstcond += this.izquierda.generar3D(ts, c3d);
                            lv.Add(c3d.label.getLastLabel());
                        }
                        if (this.derecha is Relacional)
                        {
                            Relacional rel_der = (Relacional)this.derecha;
                            secondcond += lv[lv.Count - 1] + ":\n";
                            secondcond += rel_der.generar3D(ts, c3d);
                            secondcond += "if(" + c3d.tmp.getLastTemporal() + ") goto " + c3d.label.generarLabel() + ";\n";
                            lv.Add(c3d.label.getLastLabel());
                            secondcond += "goto " + c3d.label.generarLabel() + ";\n";
                            lf.Add(c3d.label.getLastLabel());
                        }
                        else
                        {
                            secondcond = this.derecha.generar3D(ts, c3d);
                            lv.Add(c3d.label.getLastLabel());
                        }

                        break;

                    case 'o':
                        if (this.izquierda is Relacional)
                        {
                            Relacional rel_izq = (Relacional)this.izquierda;
                            firstcond += rel_izq.generar3D(ts, c3d);
                            firstcond += "if(" + c3d.tmp.getLastTemporal() + ") goto " + c3d.label.generarLabel() + ";\n";
                        }
                        if (this.derecha is Relacional)
                        {
                            Relacional rel_der = (Relacional)this.derecha;
                            secondcond += rel_der.generar3D(ts, c3d);
                            secondcond += "if(" + c3d.tmp.getLastTemporal() + ") goto " + c3d.label.generarLabel() + ";\n";
                        }
                        break;
                }
                code += firstcond + secondcond;
                code += lv[lv.Count - 1] + ":\n";
                code += c3d.tmp.generarTemporal() + " = 1;\n";
                foreach (var etiqueta in lf)
                {
                    code += etiqueta + ",";
                }
                code = code.TrimEnd(',');
                code += ":\n";

            }


            return code + "\n";
        }
    }
}
