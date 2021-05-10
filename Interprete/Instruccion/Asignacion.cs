using Proyecto1.Codigo3D;
using Proyecto1.Interprete.Expresion;
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
            this.Semanticos = new List<Analisis.Error>();
        }
        public Asignacion(List<string> ids, Expresion.Expresion val) 
        {
            this.accesos = ids;
            this.valor = val;
            this.Semanticos = new List<Analisis.Error>();
        }
        public Asignacion(String id, Expresion.Expresion index, Expresion.Expresion val)
        {
            this.id = id.ToLower();
            this.indice = index;
            this.valor = val;
            this.Semanticos = new List<Analisis.Error>();
        }
        public override object Ejecutar(TabladeSimbolos ts) 
        {
            bool updated = false;
            object nuevo_valor = this.valor.Evaluar(ts).Value;
            if (this.id.Equals("") & this.accesos != null)
            {
                updated = ts.setValorAccesos(this.accesos, nuevo_valor);
            }
            else if(this.indice != null)
            {
                object index = this.indice.Evaluar(ts).Value;
                updated = ts.setValorIndiceArreglo(int.Parse(index.ToString()), nuevo_valor, this.id);
                if (updated) 
                {
                    return ts.getArray(this.id);
                }
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
                if (updated) 
                    return ts.getFuncion(this.id);
            }
            return valor;
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string nuevo_valor = "";
            string code = "";
            if (this.accesos != null)
            {
            }
            else 
            {
                if (this.indice != null)
                {

                }
                else 
                {
                    if (ts.esFuncion(this.id)) 
                    {
                        if (this.valor is Primitivo)
                        {
                            nuevo_valor = this.valor.generar3D(ts, inter);
                            if (nuevo_valor.Contains("Llamada")) {

                                code += this.valor.generar3D(ts, inter);
                                nuevo_valor = inter.tmp.getLastTemporal();
                            }
                        }
                        else
                        {
                            code += this.valor.generar3D(ts, inter);
                            nuevo_valor = inter.tmp.getLastTemporal();
                        }
                        code += "Stack[(int)SP] = " + nuevo_valor + ";\n";
                        if (inter.lreturn.Equals(""))
                            inter.lreturn = inter.label.generarLabel();
                        code += "goto " + inter.lreturn + ";\n";
                    }
                    else
                    {
                        code += "//asignacion de nuevo valor la variable "+this.id+"\n";
                        string varval = ts.getVariablePos(this.id);
                        if (varval != null) 
                        {
                            code += inter.tmp.generarTemporal() + " = "; 
                            string[] search = varval.Split(':');
                            code += search[0] + ";\n";
                            string valu = "";
                            string tmpasig = inter.tmp.getLastTemporal();
                            if (this.valor is Primitivo)
                            {
                                valu = this.valor.generar3D(ts,inter);
                                if (valu.Contains("Llamada"))
                                {
                                    code += this.valor.generar3D(ts, inter);
                                    valu = inter.tmp.getLastTemporal();
                                }
                            }
                            else 
                            {
                                code += this.valor.generar3D(ts, inter);
                                valu = inter.tmp.getLastTemporal();
                            }
                            if (search[1].ToLower().Equals("global"))
                            {
                                code += "Heap[(int)" + tmpasig + "] = " + valu + ";\n";
                            }
                            else if (search[1].Equals("param"))
                            {
                                if (ts.isReferencia(search[2]))
                                {
                                    code += ts.getReferencia(search[2]) + " = " + valu + ";\n";
                                }
                                else 
                                {
                                    code += inter.tmp.generarTemporal() + " = SP + " + search[0] + "; //posicion de parametro " + search[2] + "\n";
                                    string tmp_param = inter.tmp.getLastTemporal();
                                    code += "Stack[(int)" + tmp_param + "] = " + valu + ";\n";
                                }
                            }
                            else
                            {
                                code += "Stack[(int)" + tmpasig + "] = " + valu + ";\n";
                            }
                        }
                    }
                }
            }
            
            return code;
        }
    }
}
