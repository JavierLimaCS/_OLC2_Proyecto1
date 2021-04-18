using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Codigo3D
{
    class Intermedio
    {
        public Temporales tmp;
        public Etiquetas label;
        public string ls;
        public Intermedio() 
        {
            this.ls = "";
            this.tmp = new Temporales();
            this.label = new Etiquetas();
        }
    }
}
