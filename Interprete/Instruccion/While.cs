using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class While : Instruccion
    {
        private Expresion.Expresion valor;
        private LinkedList<Instruccion> instrucciones;
        List<Object> salida;
        public While(Expresion.Expresion valor, LinkedList<Instruccion> instrucciones)
        {
            this.valor = valor;
            this.instrucciones = instrucciones;
            this.salida = new List<Object>();
        }

        public override object Ejecutar(TabladeSimbolos ts)
        {
            Simbolo valor = this.valor.Evaluar(ts);
            if (valor.Tipo.tipo != Tipos.BOOLEAN)
                throw new Exception("El tipo no es booleano para el IF");

            while (bool.Parse(valor.Value.ToString())) 
            {
                try
                {
                    foreach (var instruccion in this.instrucciones)
                    {
                        this.salida.Add(instruccion.Ejecutar(ts));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return this.salida;
        }
    }
}
