using Proyecto1.Interprete.Expresion;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Llamada : Instruccion
    {
        public String id;
        private List<Expresion.Expresion> exp_list;
        public object retorno;
        List<Object> salida;
        public Llamada(String id, List<Expresion.Expresion> exp_list) 
        {
            this.id = id;
            this.exp_list = exp_list;
            this.salida = new List<object>();
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            Simbolo_Funcion funcion = ts.getFuncion(this.id);
            TabladeSimbolos ts_funcion = new TabladeSimbolos(ts, funcion.Id);
            if (this.exp_list != null) 
            {
                if (funcion.Params.Count == this.exp_list.Count)
                {
                    for (int i = 0; i < funcion.Params.Count; i++)
                    {
                        funcion.Params[i].Value = this.exp_list[i].Evaluar(ts).Value;
                    }
                }
                foreach (var par in funcion.Params)
                {
                    Simbolo tmp_param = new Simbolo(par.Value.Id, par.Value.Tipo, 0, 0);
                    tmp_param.Value = par.Value.Value;
                    ts_funcion.declararVariable(par.Value.Id, tmp_param);
                }
            } 
            
            foreach (var inst in funcion.listaInst) 
            {
                if (inst != null) 
                {
                    inst.Ejecutar(ts_funcion);
                }
            }
            Object result = null;
            foreach (var sent in funcion.listaSent) 
            {
                if (sent != null)
                {
                    result = sent.Ejecutar(ts_funcion);
                    if (result is List<Object>)
                    {
                        this.salida.AddRange((List<Object>)result);
                    }
                    else
                    {
                        this.salida.Add(result);
                    }
                    if (sent is Asignacion) 
                    {
                        Asignacion posible_return = (Asignacion)sent;
                        if(posible_return.id == this.id) retorno = sent.Ejecutar(ts_funcion);
                    }
                }
            }
            if (funcion.Tipo != null) {
                this.salida.Add(retorno);
            }
            return this.salida;
        }
    }
}
