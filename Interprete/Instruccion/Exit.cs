using Proyecto1.Codigo3D;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Exit : Instruccion
    {
        Expresion.Expresion exp;
        public object valor_exit;
        public Exit(Expresion.Expresion exp) 
        {
            this.exp = exp;
            this.valor_exit = null;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            if (this.exp == null) return this;
            this.valor_exit = this.exp.Evaluar(ts);
            return this;
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string code = "";
            if (this.exp != null)
            {
                this.valor_exit = this.exp.Evaluar(ts).Value;
                code += "Stack[(int)SP] = " + this.valor_exit + ";\n";
            }
            code += "goto " + inter.label.generarLabel() + ";\n";
            return code;
        }
    }
}
