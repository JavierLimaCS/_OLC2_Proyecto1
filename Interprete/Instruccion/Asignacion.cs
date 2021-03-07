﻿using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Asignacion : Instruccion
    {
        public String id;
        Expresion.Expresion valor;
        public Asignacion(String id, Expresion.Expresion val) 
        {
            this.id = id;
            this.valor = val;
        }
        public override object Ejecutar(TabladeSimbolos ts) 
        {
            object nuevo_valor = this.valor.Evaluar(ts).Value;
            bool updated = ts.setVariableValor(this.id, nuevo_valor);
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
