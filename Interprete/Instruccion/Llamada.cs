using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Llamada : Instruccion
    {
        private String id;
        private List<Expresion.Expresion> exp_list;
        private object retorno;
        public Llamada(String id, List<Expresion.Expresion> exp_list) 
        {
            this.id = id;
            this.exp_list = exp_list;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            Simbolo_Funcion funcion = ts.getFuncion(this.id);
            foreach (var inst in funcion.listaInst) 
            {
                inst.Ejecutar(ts);
            }
            foreach (var sent in funcion.listaSent) 
            {
                sent.Ejecutar(ts);
            }
            return null;
        }
    }
}
