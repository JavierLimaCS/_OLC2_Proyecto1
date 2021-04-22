using Proyecto1.Codigo3D;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class AccesoArray : Instruccion
    {
        Expresion.Expresion indice;
        public string id;
        public AccesoArray(string id, Expresion.Expresion indice) 
        {
            this.id = id;
            this.indice = indice;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            Arreglo tmp = ts.getArray(this.id);
            Simbolo i = this.indice.Evaluar(ts);
            int index = int.Parse(i.Value.ToString());
            if (tmp != null) 
            {
                foreach (var element in tmp.Elementos)
                {
                    if (element.Key == index) 
                        return element.Value;
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
