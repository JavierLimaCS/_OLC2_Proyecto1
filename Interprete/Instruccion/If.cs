using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class If : Instruccion
    {
        private Expresion.Expresion valor;
        private LinkedList<Instruccion> instrucciones;
        private Instruccion _else;

        public If(Expresion.Expresion valor, LinkedList<Instruccion> instrucciones, Instruccion _else)
        {
            this.valor = valor;
            this.instrucciones = instrucciones;
            this._else = _else;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            Simbolo valor = this.valor.Evaluar(ts);

            //TODO verificar errores
            if (valor.Tipo.tipo != Tipos.BOOLEAN)
                throw new Exception("El tipo no es booleano para el IF");

            if (bool.Parse(valor.Value.ToString()))
            {
                try
                {
                    foreach (var instruccion in instrucciones)
                    {
                        instruccion.Ejecutar(ts);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                if (_else != null) _else.Ejecutar(ts);
            }
            return null;
        }
    }
}
