using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Proyecto2.Optimización.Reglas
{
    class Condicional3D : Instruccion3D
    {
        string salto;
        Expresion3D exp;
        int line;

        public Condicional3D(string g1, Expresion3D exp, int l) 
        {
            this.exp = exp;
            this.salto = g1;
            this.line = l;
        }

        public override string optimizar3d(Dictionary<string, int> Etiquetas, Dictionary<string, int> Saltos)
        {
            string code_ant="", code_act="";
            bool esVerdadero = false;
            if (Regex.Match(this.exp.izquierda, @"^[0-9]+").Success && Regex.Match(this.exp.derecha, @"^[0-9]+").Success)
            {
                int izq = int.Parse(this.exp.izquierda);
                int der = int.Parse(this.exp.derecha);
                switch (this.exp.op)
                {
                    case "==":
                        esVerdadero = izq == der;
                        break;
                    case "!=":
                        esVerdadero = izq != der;
                        break;
                    case ">=":
                        esVerdadero = izq >= der;
                        break;
                    case "<=":
                        esVerdadero = izq <= der;
                        break;
                    case ">":
                        esVerdadero = izq > der;
                        break;
                    case "<":
                        esVerdadero = izq < der;
                        break;
                }
                if (esVerdadero)
                {
                    code_ant = "if(" + this.exp.optimizar3d(Etiquetas, Saltos) + ") goto " + this.salto + ";";
                    code_act = "goto " + this.salto + ";";
                    this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 3", code_ant, code_act, this.line));
                    string[] tmp = this.salto.Split('L');
                    int next = int.Parse(tmp[1]) + 1;
                    string nextLabel = "L" + next;

                    if (Saltos.ContainsKey(nextLabel))
                    {
                        code_ant = "goto " + nextLabel + ";";
                        code_act = "//se elimino salto";
                        this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 3", code_ant, code_act, Saltos[nextLabel]));
                    }
                }
                else
                {
                    code_ant = "if(" + this.exp.optimizar3d(Etiquetas, Saltos) + ") goto " + this.salto + ";";
                    code_act = "//se elimina condicional";
                    this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 4", code_ant, code_act, this.line));
                    string[] tmp = this.salto.Split('L');
                    int next = int.Parse(tmp[1]) + 1;
                    string nextLabel = "L" + next;

                    if (Saltos.ContainsKey(nextLabel))
                    {
                        code_ant = "goto " + nextLabel + ";";
                        code_act = "goto " + nextLabel + ";";
                        this.Optimizaciones.Add(new ReglaM("Mirilla", "Regla 4", code_ant, code_act, Saltos[nextLabel]));
                    }
                }
            }
            else {

            }
            return "";
        }
    }
}
