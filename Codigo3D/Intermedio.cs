using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Codigo3D
{
    class Intermedio
    {
        public Temporales tmp;
        public Etiquetas label;
        public int size;
        public string ls;
        public string lcontinue;
        public Stack<string> lbreaks;
        public Stack<string> lrecursives;
        public Dictionary<string, int> voids;
        public Intermedio() 
        {
            this.size = 0;
            this.ls = "";
            this.lcontinue = "";
            this.voids = new Dictionary<string, int>();
            this.lrecursives = new Stack<string>();
            this.lbreaks = new Stack<string>();
            this.tmp = new Temporales();
            this.label = new Etiquetas();
        }
    }
}
