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
        public string lreturn;
        public string lcontinue;
        public Stack<string> lbreaks;
        public Stack<string> lrecursives;
        public Dictionary<string, int> voids;
        public Queue<string> param_tmp;
        public string tmp_return;
        public Intermedio() 
        {
            this.size = 0;
            this.ls = "";
            this.lcontinue = "";
            this.lreturn = "";
            this.voids = new Dictionary<string, int>();
            this.lrecursives = new Stack<string>();
            this.lbreaks = new Stack<string>();
            this.tmp = new Temporales();
            this.label = new Etiquetas();
            this.param_tmp = new Queue<string>();
            this.tmp_return = "";
        }

        public string getVoid(string id){
            foreach (var v in this.voids) 
            {
                if (v.Key.Contains(id)) return v.Key;
            }
            return "";
        }

        public int getVoidSize(string id)
        {
            foreach (var v in this.voids)
            {
                if (v.Key.Contains(id)) return v.Value;
            }
            return 0;
        }
    }
}
