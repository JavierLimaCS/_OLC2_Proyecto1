using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Optimización.Reglas
{
    class Expresion3D : Instruccion3D
    {
        public string izquierda;
        public string derecha;
        public string op;
        public Expresion3D(string izq, string der, string op)
        {
            this.op = op;
            this.izquierda = izq;
            this.derecha = der;
        }
        public Expresion3D(string izq)
        {
            this.op = "";
            this.izquierda = izq;
            this.derecha = "";
        }

        public Expresion3D() 
        {
            this.op = "";
            this.izquierda = "";
            this.derecha = "";
        }

        public override string optimizar3d()
        {
            return this.izquierda + " " + this.op + " " + this.derecha;
        }
    }
}
