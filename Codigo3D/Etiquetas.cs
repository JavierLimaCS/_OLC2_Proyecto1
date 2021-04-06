using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Codigo3D
{
    class Etiquetas
    {
        public int _numero_etiqueta;
        public LinkedList<string> labels;

        public Etiquetas()
        {
            this._numero_etiqueta = 0;
            this.labels = new LinkedList<string>();
        }

        public string generarLabel()
        {
            this._numero_etiqueta++;
            this.labels.AddLast("L" + _numero_etiqueta);
            return "L" + _numero_etiqueta;
        }

        public void resetLabel()
        {
            this._numero_etiqueta = 1;
        }

        public string getLastLabel() 
        {
            return this.labels.Last.Value;
        }
    }
}
