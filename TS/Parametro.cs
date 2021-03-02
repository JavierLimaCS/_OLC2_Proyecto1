using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{
    class Parametro
    {
        private String id;
        private Tipo tipo;
        private bool refer;
        private Object valor;
        public Parametro(String id, Tipo tipo) 
        {
            this.id = id;
            this.tipo = tipo;
            this.refer = false;
            this.valor = null;
        }

        public String Id { get => id; set => id = value; }
        public Tipo Tipo { get => tipo; set => tipo = value; }
        public bool Referencia { get => refer; set => refer = value; }
        public Object Value { get => valor; set => this.valor = value; }
    }
}
