using Proyecto1.Codigo3D;
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

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string code = "";
            code += "//----- Declaracion de Arreglo \n";
            if (this.arr != null)
            {
                if (this.indices.Count == 2)
                {
                    code += inter.tmp.generarTemporal() + " = ";
                    code += this.indices[1] + " - " + this.indices[0] + ";      //size del arreglo \n";
                }
                else if (this.indices.Count == 4)
                {

                }
            }
            if (ts.alias.ToLower().Equals("global"))
            {
                code += inter.tmp.generarTemporal() + " = HP;  //referencia a arreglo global\n";
            }
            else 
            {
                code += inter.tmp.generarTemporal() + " = SP;  //referencia a arreglo local\n";
            }

            return code;
        }
    }
}
