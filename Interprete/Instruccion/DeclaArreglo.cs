using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class DeclaArreglo : Instruccion
    {
        string id;
        Arreglo arr;
        List<int> indices;
        public DeclaArreglo(string id,  Arreglo arr, List<int> indices)
        {
            this.id = id.ToLower();
            this.arr = arr;
            this.indices = indices;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            if (this.arr != null)
            {
                if (this.indices.Count == 2)
                {
                    for (int i = this.indices[0]; i<= this.indices[1]; i++) 
                    {
                        this.arr.Elementos.Add(i, null);
                    }
                }
                else if (this.indices.Count == 4)
                {
                    
                }
                ts.declararArreglo(this.id, this.arr);
            }
            return null;
        }
    }
}
