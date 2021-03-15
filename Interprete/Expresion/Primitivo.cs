using Proyecto1.Interprete.Instruccion;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Expresion
{
    class Primitivo : Expresion
    {

        public char tipo;
        public object valor;

        public Primitivo(char tipo, Object valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }

        public override Simbolo Evaluar(TabladeSimbolos ts)
        {
            Simbolo primitivo = null; 
            switch(this.tipo)
            {
                case 'A':
                    break;
                case 'N':
                    primitivo = new Simbolo("primitivo", new Tipo(Tipos.INT, "integer"), 0, 0, false);
                    primitivo.Value = this.valor;
                    break;
                case 'S':
                    primitivo = new Simbolo("primitivo", new Tipo(Tipos.STRING, "string"), 0, 0, false);
                    string val = this.valor.ToString();
                    val = val.Replace("'","");
                    primitivo.Value = val;
                    break;
                case 'B':
                    primitivo = new Simbolo("primitivo", new Tipo(Tipos.BOOLEAN, "boolean"), 0, 0, false);
                    primitivo.Value = this.valor;
                    break;
                case 'R':
                    primitivo = new Simbolo("primitivo", new Tipo(Tipos.REAL, "real"), 0, 0, false);
                    primitivo.Value = this.valor;
                    break;
                case 'I':
                    primitivo = ts.getVariableValor(this.valor.ToString());
                    break;
                case 'O':
                    if (this.valor is AccesoObjeto) 
                    {
                        AccesoObjeto tmp = (AccesoObjeto)this.valor;
                        object new_valor = tmp.Ejecutar(ts);
                        primitivo = (Simbolo)new_valor;
                    }
                    break;
                case 'L':
                    Simbolo_Funcion funct = null;
                    List<object> salida = new List<object>(); 
                    if (valor is Llamada) 
                    {
                        object output;
                        Llamada llamadita = (Llamada)valor;
                        output = llamadita.Ejecutar(ts);
                        funct = ts.getFuncion(llamadita.id);
                    }
                    return funct;
            }
            return primitivo;
        }

        Simbolo_Funcion encontrarRetorno(List<object> salida)
        {
            foreach (var nodo in salida) {
                if (nodo is Simbolo_Funcion) 
                    return (Simbolo_Funcion)nodo;
            }
            return null;
        }
    }
}
