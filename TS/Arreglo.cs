using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{
    class Arreglo : Simbolo
    {
        private int tipoArreglo;
        private Dictionary<int, Object> elementos;

        public Arreglo(String n, Tipo ty, int l, int c, int tipo) : base(n, ty, l, c)
        {
            this.Id = n;
            this.Tipo = ty;
            this.Linea = l;
            this.Columna = c;
            this.tipoArreglo = tipo;
            this.elementos = new Dictionary<int, Object>();
        }

        public int TipoArreglo { get => tipoArreglo; set => tipoArreglo = value; }
        internal Dictionary<int, Object> Elementos { get => elementos; set => elementos = value; }


    }
}
