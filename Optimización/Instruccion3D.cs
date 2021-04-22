using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Optimización
{
    abstract class  Instruccion3D
    {
        public List<Regla> Optimizaciones = new List<Regla>();

        public abstract string optimizar3d();
    }
}
