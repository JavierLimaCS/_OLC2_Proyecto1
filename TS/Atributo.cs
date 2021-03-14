using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{
    class Atributo : Simbolo
    {

        public Atributo(String n, Tipo ty, object valor, int l, int c) : base(n, ty, l, c)
        {
            this.Id = n;
            this.Tipo = ty;
            this.Value = valor;
            this.Linea = l;
            this.Columna = c;
            this.esConstante = false;
        }
    }
}
