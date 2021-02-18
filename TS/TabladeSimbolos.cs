using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.TS
{
    class TabladeSimbolos 
    {
        TabladeSimbolos padre;
        Dictionary<string, Simbolo> variables;
        Dictionary<string, Simbolo_Funcion> funciones;
        Dictionary<string, object> structs;
        public TabladeSimbolos(TabladeSimbolos padre)
        {
            this.padre = padre;
            this.variables = new Dictionary<string, Simbolo>();
            this.funciones = new Dictionary<string, Simbolo_Funcion>();
        }

        public Simbolo getVariableValor(String id)
        {
            TabladeSimbolos actual = this;
            while (actual != null)
            {
                if (actual.variables[id] != null)
                    return actual.variables[id];
                actual = actual.padre;
            };
            return null;
        }

        public void declararVariable(string id, Simbolo variable)
        {
            if (this.variables[id] != null)
            {
                this.variables.Add(id, variable);
            }
            else
            {
                throw new Exception("La variable " + id + " ya existe en este ambito");
            }
        }        

    }
}
