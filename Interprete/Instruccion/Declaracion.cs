using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Declaracion : Instruccion
    {
        private String id;
        private Tipo type;
        private Object value;
        private int line, col;
        public Declaracion(Tipo tipo, String id, Object value, int linea, int col) 
        {
            this.type = tipo;
            this.id = id;
            this.value = value;
            this.line = linea;
            this.col = col;
        }
        public override void Ejecutar(TabladeSimbolos TS)
        {
            Tipo tipo_variable =  this.type;
            Simbolo nuevo = new Simbolo(this.id, tipo_variable, this.line, this.col);
            TS.declararVariable(this.id, nuevo);
        }
    }
}
