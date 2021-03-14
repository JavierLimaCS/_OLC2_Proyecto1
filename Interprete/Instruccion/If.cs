using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class If : Instruccion
    {
        private Expresion.Expresion valor;
        private LinkedList<Instruccion> instrucciones;
        private Instruccion _else;
        List<object> salida;
        public If(Expresion.Expresion valor, LinkedList<Instruccion> instrucciones, Instruccion _else)
        {
            this.valor = valor;
            this.instrucciones = instrucciones;
            this._else = _else;
            this.salida = new List<object>();
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            Simbolo valor = this.valor.Evaluar(ts);
            if (valor.Tipo.tipo != Tipos.BOOLEAN)
                throw new Exception("El tipo no es booleano para el IF");

            if (bool.Parse(valor.Value.ToString()))
            {
                try
                {
                    foreach (var instruccion in instrucciones)
                    {
                        if (instruccion != null)
                        {
                            Object output = instruccion.Ejecutar(ts);
                            if (output is List<object>)
                            {
                                this.salida.AddRange((List<object>)output);
                            }
                            else if (output is Break)
                            {
                                return output;
                            }
                            else if (output is Continue)
                            {
                                break;
                            }
                            else if (output is Exit)
                            {

                            }
                            else
                            {
                                this.salida.Add(output);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                if (_else != null) 
                {
                    this.salida.AddRange((List<object>)_else.Ejecutar(ts));
                }
            }
            return this.salida;
        }
    }
}
