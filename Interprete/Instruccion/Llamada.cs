using Proyecto1.Codigo3D;
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
        public Llamada(String id, List<Expresion.Expresion> exp_list) 
        {
            this.id = id.ToLower();
            this.exp_list = exp_list;
            this.Semanticos = new List<Analisis.Error>();
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            Simbolo_Funcion funcion = ts.getFuncion(this.id);
            if (funcion == null)
            {
                this.Semanticos.Add(new Analisis.Error("Semantico", "La funcion" + this.id + "no existe.", 0, 0));
                return "";
            }
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
                    Simbolo tmp_param = new Simbolo(par.Value.Id, par.Value.Tipo, 0, 0, par.Value.Referencia);
                    tmp_param.Value = par.Value.ValorPar;
                    tmp_param.Scope = ts_funcion.alias;
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
            foreach (var sent in funcion.listaSent) 
            {
                if (sent != null)
                {
                    if (sent is Asignacion)
                    {
                        Asignacion posible_return = (Asignacion)sent;
                        retorno = sent.Ejecutar(ts_funcion);
                        if (posible_return.id == this.id) 
                        {
                            retorno = sent.Ejecutar(ts_funcion);
                            return retorno;
                        }
                            
                    }
                    else 
                    {
                        object output = sent.Ejecutar(ts_funcion);
                        if (output is Break)
                        {
                            return "";
                        }
                        else if (output is Continue)
                        {
                            break;
                        }
                        else if (output is Exit)
                        {
                            Exit returnf = (Exit)output;
                            Simbolo nuevo = (Simbolo)returnf.valor_exit;
                            ts.setFuncionValor(this.id, nuevo.Value);
                            return ts.getFuncion(this.id);
                        }
                        else
                        {
                            this.retorno = output;
                        }
                    }
                }
            }
            if (funcion.Tipo != null) {
                return this.retorno;
            }
            return "";
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string void_name = inter.getVoid(this.id);
            string code = "//---> Inicio de Llamada de void " + this.id + "\n";
            if (this.exp_list != null)
            {
                foreach (var exp in this.exp_list)
                {
                    if (!(exp is Primitivo))
                    {
                        code += exp.generar3D(ts, inter);
                        inter.param_tmp.Enqueue(inter.tmp.getLastTemporal());
                    }
                    else
                    {
                        Primitivo primi = (Primitivo)exp;
                        if (primi.tipo =='L') 
                        {
                            inter.auxiliar = inter.param_tmp;
                            inter.param_tmp.Clear();
                            code += exp.generar3D(ts,inter);
                            inter.param_tmp = inter.auxiliar;
                            inter.auxiliar.Clear();
                        }
                        else 
                        {
                            if (primi.tipo =='I') 
                            {
                                
                            }
                            Simbolo tmp = exp.Evaluar(ts);
                            if (tmp != null)
                            {
                                switch (tmp.Tipo.tipoAuxiliar)
                                {
                                    case "string":
                                        code += exp.generar3D(ts, inter);
                                        break;
                                    default:
                                        string valor = exp.generar3D(ts, inter);
                                        if (valor.Contains("Heap") || valor.Contains("Stack"))
                                        {
                                            code += valor + "\n";
                                        }
                                        else
                                        {
                                            code += inter.tmp.generarTemporal() + " = " + valor + ";\n";
                                        }
                                        break;
                                }
                                inter.param_tmp.Enqueue(inter.tmp.getLastTemporal());
                            }
                        }
                        
                    }
                }
            }
            else 
            {
                code += "//no hay parametros en la llamada \n"; 
            }
            
            code += "SP = SP + 1;         //cambio de ambito simulado\n";
            code += "//-----> se guardan temporales\n";
            inter.tmp.generarTemporal();
            string tmp_guardado = inter.tmp.getLastTemporal();
            for (int i = 0; i < inter.tmp.tmpStorage.Count; i++) 
            {
                code += tmp_guardado + " = SP + " + i + ";\n" ;
                code += "Stack[(int)" + tmp_guardado + "] = " + inter.tmp.tmpStorage.ElementAt(i)+";\n";
            }
            string recuperacion_tmp = "//-----> se recuperan los temporales\n";
            for (int i = 0; i < inter.tmp.tmpStorage.Count; i++) 
            {
                recuperacion_tmp += tmp_guardado + " = SP + " + i + ";\n";
                recuperacion_tmp += inter.tmp.tmpStorage.ElementAt(i) + " = Stack[(int)" + tmp_guardado + "];\n";
            }
            code += "//----------------------------- \n";
            code += "SP = SP + " + inter.getVoidSize(this.id) + ";\n";
            code += "//-------Paso de Parametros \n";
            for (int j = 0; j < inter.param_tmp.Count; j++)
            {
                code += inter.tmp.generarTemporal() + " = SP + " + (j + 1)  + ";\n";
                code += "Stack[(int)" + inter.tmp.getLastTemporal() + "] = " + inter.param_tmp.ElementAt(j) + ";\n";
            }
            code += void_name + "();\n";
            Simbolo_Funcion funcion = ts.getFuncion(this.id);
            if (funcion.Tipo != null) 
            {
                code += inter.tmp.generarTemporal() + " = Stack[(int)SP];\n";
                inter.tmp_return = inter.tmp.getLastTemporal();
            }
            code += "SP = SP - " + inter.getVoidSize(this.id) + ";\n";
            code += recuperacion_tmp;
            code += "//---- fin de Recuperacion\n";
            code += "SP = SP - 1;\n";
            inter.param_tmp.Clear();
            return code;
        }
    }
}
