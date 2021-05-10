﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Optimización.Reglas
{
    class Salto3D : Instruccion3D
    {
        string label;
        int fila;
        public Salto3D(string l, int f) 
        {
            this.label = l;
            this.fila = f;
        }
        public override string optimizar3d()
        {
            this.Saltos.Add(this.label, this.fila);
            return "";
        }
    }
}
