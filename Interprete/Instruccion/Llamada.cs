using Proyecto1.Interprete.Expresion;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
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
            this.id = id.ToLower();
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
                    int i = 0;
                    foreach(var par in funcion.Params)
                    {
                        par.Value.ValorPar = this.exp_list[i].Evaluar(ts).Value;
                        i++;
                    }
                }
                foreach (var par in funcion.Params)
                {
                    Simbolo tmp_param = new Simbolo(par.Value.Id, par.Value.Tipo, 0, 0, false);
                    tmp_param.Value = par.Value.ValorPar;
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
            String print = "";
            foreach (var sent in funcion.listaSent) 
            {
                if (sent != null)
                {
                    if (sent is Asignacion)
                    {
                        Asignacion posible_return = (Asignacion)sent;
                        if (posible_return.id == this.id)
                            retorno = sent.Ejecutar(ts_funcion);
                    }
                    else 
                    {
                        result = sent.Ejecutar(ts_funcion);
                    }
                    if (result is List<Object>)
                    {
                        this.salida = this.salida.Union((List<Object>)result).ToList();
                    }
                    else
                    {
                        if (result != null)
                            print += result.ToString();
                        if (result is Simbolo_Funcion) 
                        {
                            retorno = result;
                            Simbolo_Funcion tmp = (Simbolo_Funcion)retorno;
                            ts.setFuncionValor(tmp.Id, tmp.Value);
                        }
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
