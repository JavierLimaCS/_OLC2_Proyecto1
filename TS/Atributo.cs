using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{
    class Atributo
    {
        private String id;
        private Tipo tipo_atributo;
        private object valor_atributo;

        public Atributo(String id, Tipo tipo, object valor)
        {
            this.Id = id;
            this.Tipo_atributo = tipo;
            this.Valor_atributo = valor;
        }

        public string Id { get => id; set => id = value; }
        public object Valor_atributo { get => valor_atributo; set => valor_atributo = value; }
        internal Tipo Tipo_atributo { get => tipo_atributo; set => tipo_atributo = value; }
    }
}
