using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Asignacion : Instruccion
    {
        public String id = "";
        public List<string> accesos;
        Expresion.Expresion indice;
        Expresion.Expresion valor;
        public Asignacion(String id, Expresion.Expresion val) 
        {
            this.id = id;
            this.valor = val;
        }
        public Asignacion(List<string> ids, Expresion.Expresion val) 
        {
            this.accesos = ids;
            this.valor = val;
        }
        public Asignacion(String id, Expresion.Expresion index, Expresion.Expresion val)
        {
            this.id = id.ToLower();
            this.indice = index;
            this.valor = val;
        }
        public override object Ejecutar(TabladeSimbolos ts) 
        {
            bool updated = false;
            object nuevo_valor = this.valor.Evaluar(ts).Value;
            if (this.id.Equals("") & this.accesos != null)
            {
                updated = ts.setValorAccesos(this.accesos, nuevo_valor);
            }
            else 
            {
                Simbolo variable = ts.getVariableValor(this.id);
                if (variable != null)
                    if (variable.esConstante) 
                        return variable.Value;
                updated = ts.setVariableValor(this.id, nuevo_valor);
            }
            if (updated)
            {
                return ts.getVariableValor(this.id);
            }
            else
            {
                updated = ts.setFuncionValor(this.id, nuevo_valor);
                if (updated) return ts.getFuncion(this.id);
            }
            return valor;
        }
    }
}
