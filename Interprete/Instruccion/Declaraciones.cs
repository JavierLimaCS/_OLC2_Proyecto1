﻿using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Declaraciones : Instruccion
    {
        private LinkedList<Declaracion> declaracions;
        public Declaraciones(LinkedList<Declaracion> decla) 
        {
            this.declaracions = decla;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            foreach (var decla in this.declaracions)
            {
                decla.Ejecutar(ts);
            }
            return null;
        }
    }
}
