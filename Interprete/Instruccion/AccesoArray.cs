using Proyecto1.Codigo3D;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class AccesoArray : Instruccion
    {
        int indice;
        string id;
        public AccesoArray(string id, int indice) 
        {
            this.id = id;
            this.indice = indice;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            Arreglo tmp = ts.getArray(this.id);
            if (tmp != null) 
            {
                foreach (var element in tmp.Elementos)
                {
                    if (element.Key == indice) return element.Value;
                }
            }
            return "";
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            return "";
        }
    }
}
