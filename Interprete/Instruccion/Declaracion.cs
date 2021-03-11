using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Declaracion : Instruccion
    {
        private List<String> id; 
        private Tipo type;
        private Expresion.Expresion value;
        private int line, col;
        public Declaracion(Tipo tipo, List<String> id, Expresion.Expresion value, int linea, int col) 
        {
            this.type = tipo;
            this.id = id;
            this.value = value;
            this.line = linea;
            this.col = col;
        }
        public override object Ejecutar(TabladeSimbolos TS)
        {
            Tipo tipo_variable =  this.type;
            Simbolo nuevo = null;
            Object valor = null;
            String ids = "";
            foreach (var variable in this.id) 
            {
                nuevo = new Simbolo(variable, tipo_variable, this.line, this.col);
                ids += " |" + variable +"| ";
                if (this.value != null)
                {
                    valor = this.value.Evaluar(TS).Value;
                    nuevo.Value = valor;
                }
                else 
                {
                    String tipo = this.type.tipoAuxiliar;
                    switch (tipo) 
                    {
                        case "integer":
                            nuevo.Value = 0;
                            break;
                        case "string":
                            nuevo.Value = "";
                            break;
                        case "real":
                            nuevo.Value = 0.0;
                            break;
                        case "boolean":
                            nuevo.Value = false;
                            break;
                        default:
                            nuevo.Value = TS.getObjeto(this.type.tipoAuxiliar);
                            System.Diagnostics.Debug.WriteLine(this.type.tipoAuxiliar);
                            break;
                    }
                }
                TS.declararVariable(variable, nuevo);
            }
            //return "Variables " + ids  + " se ha insertado en los TS\n";
            return null;
        }
    }
}
