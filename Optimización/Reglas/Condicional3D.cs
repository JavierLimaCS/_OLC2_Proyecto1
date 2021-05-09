using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Optimización.Reglas
{
    class Condicional3D : Instruccion3D
    {
        string saltov;
        string saltof;
        Expresion3D exp;

        public Condicional3D(string g1, string g2, Expresion3D exp) 
        {
            this.exp = exp;
            this.saltov = g1;
            this.saltof = g2;
        }

        public override string optimizar3d()
        {
            string codigo ="";

            return codigo;
        }
    }
}
