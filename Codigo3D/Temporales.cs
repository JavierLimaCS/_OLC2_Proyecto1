using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Codigo3D
{
    class Temporales
    {
        public int _numero_temporal;

        public Temporales() 
        {
            this._numero_temporal = 1;
        }

        public string generarTemporal() 
        {
            this._numero_temporal++;
            return "t"+_numero_temporal;
        }

        public void resetTemporal() 
        {
            this._numero_temporal = 1;
        }
    }
}
