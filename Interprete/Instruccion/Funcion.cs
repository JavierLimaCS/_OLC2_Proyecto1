﻿using Proyecto1.Codigo3D;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Funcion : Instruccion
    {
        private Simbolo_Funcion funcion;
        public LinkedList<Instruccion> instrucciones;
        private LinkedList<Instruccion> sentencias;
        public Funcion(Simbolo_Funcion funcion, LinkedList<Instruccion> instrucciones, LinkedList<Instruccion> sentencias)
        {
            this.funcion = funcion;
            this.instrucciones = instrucciones;
            this.sentencias = sentencias;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            funcion.listaInst = this.instrucciones;
            funcion.listaSent = this.sentencias;
            ts.declararFuncion(this.funcion.Id, this.funcion);
            return null;
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string code = "//--- Funcion \n";
            string parametros = "";
            string void_name = "";
            int size = 1;
            if (this.funcion.Params.Count > 0)
            {
                foreach (var param in this.funcion.Params)
                {
                    parametros += param.Key.ToString() + "_";
                    size++;
                }
                void_name = this.funcion.Id + "_" + parametros;
                void_name = void_name.TrimEnd('_');
                inter.voids.Add(void_name, size);
                code += "void " + void_name;
                code += "() { \n";
            }
            else
            {
                void_name = this.funcion.Id;
                inter.voids.Add(void_name, size);
                code += "void " + void_name + "() { \n";
            }
            TabladeSimbolos ts_proc = new TabladeSimbolos(ts, ts.alias + this.funcion.Id);
            if (this.funcion.Params.Count > 0)
            {
                foreach (var par in funcion.Params)
                {
                    Simbolo tmp_param = new Simbolo(par.Value.Id, par.Value.Tipo, 0, 0, par.Value.Referencia);
                    switch (par.Value.Tipo.tipoAuxiliar.ToLower())
                    {
                        case "integer":
                            tmp_param.Value = 0;
                            break;
                        case "real":
                            tmp_param.Value = 0.0;
                            break;
                        case "boolean":
                            tmp_param.Value = false;
                            break;
                        case "string":
                            tmp_param.Value = "";
                            break;
                    }
                    ts_proc.declararVariable(par.Value.Id, tmp_param);
                }
            }
            if (this.instrucciones != null)
            {
                foreach (var inst in this.instrucciones)
                {
                    inst.Ejecutar(ts_proc);
                    code += inst.generar3D(ts_proc, inter);
                }
            }
            if (this.sentencias != null)
            {
                foreach (var sent in this.sentencias)
                {
                    code += sent.generar3D(ts_proc, inter);
                }
            }
            code += "return;\n } \n";
            ts.alias = "global";
            return code;
        }
    }
}
