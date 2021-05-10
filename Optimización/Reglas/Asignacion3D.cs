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
        bool isOpt;
        public Asignacion3D(string id,char ty, Expresion3D exp, int fila) 
        {
            this.id = id;
            this.tipo = ty;
            this.exp = exp;
            this.line = fila;
            this.isOpt = true;
        }

        public Asignacion3D() 
        {
            this.id = "";
            this.tipo = 'f';
            this.exp = null;
            this.line = 0;
            this.isOpt = false;
        }
        public override string optimizar3d(Dictionary<string, int> Etiquetas, Dictionary<string, int> Saltos)
        {
            string codigo = "";
            if (isOpt) 
            {
                string code_ant = "";
                string code_act = "";
                switch (this.exp.op)
                {
                    case "+":
                        if (this.id.Equals(this.exp.izquierda) || this.id.Equals(this.exp.derecha))
                        {
                            if (this.exp.derecha.Equals("0") || this.exp.izquierda.Equals("0"))
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas,Saltos) + ";";
                                code_act = "//se elimino instruccion";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 6", code_ant, code_act, this.line));
                            }
                        }
                        else if (this.exp.izquierda.Contains("t") || this.exp.derecha.Contains("t") || this.exp.izquierda.Contains("SP") || this.exp.derecha.Contains("SP") || this.exp.izquierda.Contains("HP") || this.exp.derecha.Contains("HP")) 
                        {
                            if (this.exp.derecha.Equals("0") || this.exp.izquierda.Equals("0"))
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = this.id + "=";
                                if (this.exp.izquierda.Contains("t")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("t")) code_act += this.exp.derecha;
                                if (this.exp.izquierda.Contains("SP")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("SP")) code_act += this.exp.derecha;
                                if (this.exp.izquierda.Contains("HP")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("HP")) code_act += this.exp.derecha;

                                code_act += ";";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 10", code_ant, code_act, this.line));
                            }
                        }
                        break;
                    case "-":
                        if (this.id.Equals(this.exp.izquierda) || this.id.Equals(this.exp.derecha))
                        {
                            if (this.exp.derecha.Equals("0") || this.exp.izquierda.Equals("0"))
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = "//se elimino instruccion";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 7", code_ant, code_act, this.line));
                            }
                        }
                        else if (this.exp.izquierda.Contains("t") || this.exp.derecha.Contains("t") || this.exp.izquierda.Contains("SP") || this.exp.derecha.Contains("SP") || this.exp.izquierda.Contains("HP") || this.exp.derecha.Contains("HP"))
                        {
                            if (this.exp.derecha.Equals("0") || this.exp.izquierda.Equals("0"))
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = this.id + "=";
                                if (this.exp.izquierda.Contains("t")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("t")) code_act += this.exp.derecha;
                                if (this.exp.izquierda.Contains("SP")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("SP")) code_act += this.exp.derecha;
                                if (this.exp.izquierda.Contains("HP")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("HP")) code_act += this.exp.derecha;
                                code_act += ";";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 11", code_ant, code_act, this.line));
                            }   
                        }
                        break;
                    case "/":
                        if (this.id.Equals(this.exp.izquierda) || this.id.Equals(this.exp.derecha))
                        {
                            if (this.exp.derecha.Equals("1") || this.exp.izquierda.Equals("1"))
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = "//se elimino instruccion";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 9", code_ant, code_act, this.line));
                            }
                            else if (this.exp.izquierda.Equals("0")) 
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = this.id + "=0;";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 16", code_ant, code_act, this.line));
                            }
                        }
                        else if (this.exp.izquierda.Contains("t") || this.exp.derecha.Contains("t") || this.exp.izquierda.Contains("SP") || this.exp.derecha.Contains("SP") || this.exp.izquierda.Contains("HP") || this.exp.derecha.Contains("HP"))
                        {
                            if (this.exp.derecha.Equals("1") || this.exp.izquierda.Equals("1"))
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = this.id + "=";
                                if (this.exp.izquierda.Contains("t")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("t")) code_act += this.exp.derecha;
                                if (this.exp.izquierda.Contains("SP")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("SP")) code_act += this.exp.derecha;
                                if (this.exp.izquierda.Contains("HP")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("HP")) code_act += this.exp.derecha;
                                code_act += ";";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 13", code_ant, code_act, this.line));
                            }
                            else if (this.exp.izquierda.Equals("0"))
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = this.id + "=0;";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 16", code_ant, code_act, this.line));
                            }

                        }
                        break;
                    case "*":
                        if (this.id.Equals(this.exp.izquierda) || this.id.Equals(this.exp.derecha))
                        {
                            if (this.exp.derecha.Equals("1") || this.exp.izquierda.Equals("1"))
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = "//se elimino instruccion";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 8", code_ant, code_act, this.line));
                            }
                            else if (this.exp.derecha.Equals("0") || this.exp.izquierda.Equals("0"))
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = this.id + "=0;";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 15", code_ant, code_act, this.line));
                            }
                            else if (this.exp.derecha.Equals("2") || this.exp.izquierda.Equals("2"))
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = this.id + "=";
                                if (this.exp.izquierda.Contains("t")) code_act += this.exp.izquierda + "+" + this.exp.izquierda;
                                else if (this.exp.derecha.Contains("t")) code_act += this.exp.derecha + "+" + this.exp.derecha;
                                if (this.exp.izquierda.Contains("SP")) code_act += this.exp.izquierda + "+" + this.exp.izquierda;
                                else if (this.exp.derecha.Contains("SP")) code_act += this.exp.derecha + "+" + this.exp.derecha;
                                if (this.exp.izquierda.Contains("HP")) code_act += this.exp.izquierda + "+" + this.exp.izquierda;
                                else if (this.exp.derecha.Contains("HP")) code_act += this.exp.derecha + "+" + this.exp.derecha;
                                code_act += ";";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 14", code_ant, code_act, this.line));
                            }
                        }
                        else if (this.exp.izquierda.Contains("t") || this.exp.derecha.Contains("t") || this.exp.izquierda.Contains("SP") || this.exp.derecha.Contains("SP") || this.exp.izquierda.Contains("HP") || this.exp.derecha.Contains("HP"))
                        {
                            if (this.exp.derecha.Equals("1") || this.exp.izquierda.Equals("1")) 
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = this.id + "=";
                                if (this.exp.izquierda.Contains("t")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("t")) code_act += this.exp.derecha;
                                if (this.exp.izquierda.Contains("t")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("t")) code_act += this.exp.derecha;
                                if (this.exp.izquierda.Contains("SP")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("SP")) code_act += this.exp.derecha;
                                if (this.exp.izquierda.Contains("HP")) code_act += this.exp.izquierda;
                                else if (this.exp.derecha.Contains("HP")) code_act += this.exp.derecha;
                                code_act += ";";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 12", code_ant, code_act, this.line));
                            }
                            else if (this.exp.derecha.Equals("0") || this.exp.izquierda.Equals("0"))
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = this.id + "=0;";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 15", code_ant, code_act, this.line));
                            }
                            if (this.exp.derecha.Equals("2") || this.exp.izquierda.Equals("2"))
                            {
                                code_ant = this.id + "=" + this.exp.optimizar3d(Etiquetas, Saltos) + ";";
                                code_act = this.id + "=";
                                if (this.exp.izquierda.Contains("t")) code_act += this.exp.izquierda +"+"+ this.exp.izquierda;
                                else if (this.exp.derecha.Contains("t")) code_act += this.exp.derecha + "+" + this.exp.derecha;
                                if (this.exp.izquierda.Contains("SP")) code_act += this.exp.izquierda + "+" + this.exp.izquierda;
                                else if (this.exp.derecha.Contains("SP")) code_act += this.exp.derecha + "+" + this.exp.derecha;
                                if (this.exp.izquierda.Contains("HP")) code_act += this.exp.izquierda + "+" + this.exp.izquierda;
                                else if (this.exp.derecha.Contains("HP")) code_act += this.exp.derecha + "+" + this.exp.derecha;
                                code_act += ";";
                                this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 14", code_ant, code_act, this.line));
                            }
                        }
                        break;
                }
            }
            return codigo;
        }
    }
}
