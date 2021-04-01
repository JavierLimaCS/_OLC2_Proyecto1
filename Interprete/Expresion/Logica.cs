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
                if (this.izquierda is Primitivo & this.derecha is Primitivo)
                {
                    code += c3d.tmp.generarTemporal() + " = ";
                    code += this.izquierda.generar3D(ts, c3d);
                    code += this.op.ToString();
                    code += this.derecha.generar3D(ts, c3d);
                }
                else if (this.izquierda is Primitivo & !(this.derecha is Primitivo))
                {
                    string tmp = "";
                    code += this.derecha.generar3D(ts, c3d);
                    tmp = c3d.tmp.getLastTemporal();
                    code += c3d.tmp.generarTemporal() + " = ";
                    code += this.izquierda.generar3D(ts, c3d);
                    code += this.op.ToString();
                    code += tmp;
                }
                else if (!(this.izquierda is Primitivo) & this.derecha is Primitivo)
                {
                    string tmp = "";
                    code += this.izquierda.generar3D(ts, c3d);
                    tmp = c3d.tmp.getLastTemporal();
                    code += c3d.tmp.generarTemporal() + " = ";
                    code += tmp;
                    code += this.op.ToString();
                    code += this.derecha.generar3D(ts, c3d);
                }
                else
                {
                    Logica izq = (Logica)this.izquierda;
                    Logica der = (Logica)this.derecha;
                    if (izq.op == '*' || izq.op == '/' || izq.op == '%')
                    {
                        code += izq.generar3D(ts, c3d);
                        code += der.generar3D(ts, c3d);
                    }
                    if (der.op == '*' || der.op == '/' || der.op == '%')
                    {
                        code += der.generar3D(ts, c3d);
                        code += izq.generar3D(ts, c3d);
                    }
                }
            }

            return code + ";\n";
        }
    }
}
