using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class AccesoObjeto : Instruccion
    {
        public string var_id;
        public string var_obj_attr;
        public AccesoObjeto(string id, string atr) 
        {
            this.var_id = id;
            this.var_obj_attr = atr;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            string error = "";
            Simbolo variable = ts.getVariableValor(this.var_id);
            Objeto var_objeto = null;
            if (variable.Value != null)
            {
                var_objeto = (Objeto)variable.Value;
                foreach (var atrs in var_objeto.Attribs)
                {
                    if (atrs.Id == this.var_obj_attr) return atrs;
                }
            }
            else 
            {
                
            }
            return "ERROR:" ;
        }
    }
}
