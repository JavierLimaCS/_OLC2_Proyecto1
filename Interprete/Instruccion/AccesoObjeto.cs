using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class AccesoObjeto : Instruccion
    {
        List<string> ids;
        public AccesoObjeto(List<string> ids) 
        {
            this.ids = ids;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            string error = "";
            Simbolo variable = ts.getVariableValor(this.ids.ElementAt(0));
            Objeto var_objeto = null;
            if (variable.Value != null)
            {
                var_objeto = (Objeto)variable.Value;
                foreach (var atrs in var_objeto.Attribs)
                {
                    if (atrs.Id.ToLower() == this.ids.ElementAt(1).ToLower()) 
                    {
                        if (atrs.Value is Objeto)
                        {
                            Objeto tmp = (Objeto)atrs.Value;
                            foreach (var atribs in tmp.Attribs)
                            {
                                if (atribs.Id.ToLower() == this.ids.ElementAt(2).ToLower()) return atribs;
                            }
                        }
                        else
                        {
                            return atrs;
                        }
                    }
                }
            }
            else 
            {
                error = "No existe";  
            }
            return "ERROR: " + error;
        }
    }
}
