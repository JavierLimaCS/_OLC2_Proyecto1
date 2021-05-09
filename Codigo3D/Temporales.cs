using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Codigo3D
{
    class Temporales
    {
        public int _numero_temporal;
        public LinkedList<string> temporales;
        public LinkedList<string> tmpStorage;
        public bool delete = true;

        public Temporales()
        {
            this._numero_temporal = 0;
            this.temporales = new LinkedList<string>();
            this.tmpStorage = new LinkedList<string>();
        }

        public string generarTemporal()
        {
            this._numero_temporal++;
            this.temporales.AddLast("t" + this._numero_temporal);
            this.tmpStorage.AddLast("t" + this._numero_temporal);
            return "t" + _numero_temporal;
        }

        public void resetTemporal()
        {
            this.temporales.Clear();
            this.tmpStorage.Clear();
            this._numero_temporal = 1;
        }

        public string getLastTemporal() 
        {
            if (this.tmpStorage.Count > 0 && delete) this.tmpStorage.RemoveLast();
            return this.temporales.Last.Value;
        }

    }
}
