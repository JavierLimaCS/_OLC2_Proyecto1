using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Codigo3D
{
    class Etiquetas
    {
        public int _numero_etiqueta;

        public Etiquetas()
        {
            this._numero_etiqueta = 1;
        }

        public string generarTemporal()
        {
            this._numero_etiqueta++;
            return "t" + _numero_etiqueta;
        }

        public void resetTemporal()
        {
            this._numero_etiqueta = 1;
        }
    }
}
