using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Codigo3D
{
    class Temporales
    {
        public int _numero_temporal;
        public LinkedList<string> temporales;

        public Temporales()
        {
            this._numero_temporal = 0;
            this.temporales = new LinkedList<string>();
        }

        public string generarTemporal()
        {
            this._numero_temporal++;
            this.temporales.AddLast("t" + this._numero_temporal);
            return "t" + _numero_temporal;
        }

        public void resetTemporal()
        {
            this._numero_temporal = 1;
        }

        public string getLastTemporal() 
        {
            return this.temporales.Last.Value;
        }
    }
}
