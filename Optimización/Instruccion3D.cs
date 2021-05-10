using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Optimización
{
    abstract class  Instruccion3D
    {
        public List<ReglaM> Optimizaciones = new List<ReglaM>();
        public abstract string optimizar3d(Dictionary<string, int> Etiquetas, Dictionary<string, int> Saltos);
    }
}
