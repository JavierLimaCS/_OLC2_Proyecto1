using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Declaracion : Instruccion
    {
        private String tipo, id;
        private Object value;
        private int line, col;
        public Declaracion(String tipo, String id, Object value, int linea, int col) 
        {
            this.tipo = tipo;
            this.id = id;
            this.value = value;
            this.line = linea;
            this.col = col;
        }
        public override void Ejecutar(TabladeSimbolos TS, String ambito)
        {
            Simbolo.Tipo tipo_variable = Simbolo.Tipo.NULL;
            switch(this.tipo)
            {
                case "integer":
                    tipo_variable = Simbolo.Tipo.INT;
                    break;
                case "string":
                    tipo_variable = Simbolo.Tipo.STR;
                    break;
                case "real":
                    tipo_variable = Simbolo.Tipo.REAL;
                    break;
                case "boolean":
                    tipo_variable = Simbolo.Tipo.BOOL;
                    break;
            }

            Simbolo nuevo = new Simbolo(this.id, tipo_variable, ambito, this.line, this.col);
        }
    }
}
