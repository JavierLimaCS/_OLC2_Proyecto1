using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Declaracion : Instruccion
    {
        private string tipo;
        private 
        public Declaracion(String tipo, String id, object value) 
        { 
        }
        public override void Ejecutar(TabladeSimbolos TS, String ambito)
        {
            throw new NotImplementedException();
        }
    }
}
