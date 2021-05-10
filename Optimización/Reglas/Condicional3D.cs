using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Proyecto2.Optimización.Reglas
{
    class Condicional3D : Instruccion3D
    {
        string saltov;
        string saltof;
        Expresion3D exp;
        string azpattern = "[0-9]+";

        public Condicional3D(string g1, string g2, Expresion3D exp) 
        {
            this.exp = exp;
            this.saltov = g1;
            this.saltof = g2;
        }

        public override string optimizar3d()
        {
            string codigo ="";
            bool esVerdadero = false;
            if (Regex.Match(this.exp.izquierda,azpattern).Success && Regex.Match(this.exp.derecha, azpattern).Success) 
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
            }
            if (esVerdadero)
            {

            }
            else 
            {
            
            }

            return codigo;
        }
    }
}
