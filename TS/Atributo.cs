using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{
    class Atributo : Simbolo
    {
        private String id;
        private Tipo tipo_atributo;
        private object valor_atributo;
        public int linea, col;

        public Atributo(String n, Tipo ty, object valor, int l, int c) : base(n, ty, l, c)
        {
            this.Id = n;
            this.Tipo_atributo = ty;
            this.Valor_atributo = valor;
            this.linea = l;
            this.col = c;
        }

        public string Id { get => id; set => id = value; }
        public object Valor_atributo { get => valor_atributo; set => valor_atributo = value; }
        public Tipo Tipo_atributo { get => tipo_atributo; set => tipo_atributo = value; }
    }
}
