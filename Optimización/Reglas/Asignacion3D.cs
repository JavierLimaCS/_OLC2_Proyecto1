using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Optimización.Reglas
{
    class Asignacion3D : Instruccion3D
    {
        string id;
        char tipo;
        Expresion3D exp;
        int line;
        public Asignacion3D(string id,char ty, Expresion3D exp, int fila) 
        {
            this.id = id;
            this.tipo = ty;
            this.exp = exp;
            this.line = fila;
        }
        public override string optimizar3d()
        {
            string codigo = "";
            string code_ant = "";
            string code_act = "";
            switch (this.exp.op)
            {
                case "+":
                    if (this.id.Equals(this.exp.izquierda) || this.id.Equals(this.exp.derecha))
                    {
                        if (this.exp.derecha.Equals("0") || this.exp.izquierda.Equals("0"))
                        {
                            code_ant = this.id + " = " + this.exp.optimizar3d() + ";";
                            code_act = "//se elimino instruccion";
                            this.Optimizaciones.Add(new Regla("Mirilla", "Regla 6", code_ant, code_act, this.line));
                        }
                    }
                    else 
                    {
                        code_ant = this.id + " = " + this.exp.optimizar3d() + ";";
                        code_act = this.id + " = ";
                        if (this.exp.izquierda.Contains("t")) code_act += this.exp.izquierda;
                        else if (this.exp.derecha.Contains("t")) code_act += this.exp.derecha;
                        code_act += ";";
                        this.Optimizaciones.Add(new Regla("Mirilla", "Regla 10", code_ant, code_act, this.line));
                    }
                    break;
                case "-":
                    break;
                case "/":
                    break;
                case "*":
                    if (this.id.Equals(this.exp.izquierda)) 
                    {
                    
                    }
                    break;
            }
            return codigo;
        }
    }
}
