using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Optimización
{
    abstract class  Instruccion3D
    {
        public List<ReglaM> Optimizaciones = new List<ReglaM>();
        public Dictionary<string,int> Etiquetas = new Dictionary<string, int>();
        public Dictionary<string,int> Saltos = new Dictionary<string,int>();
        public abstract string optimizar3d();
    }
}
