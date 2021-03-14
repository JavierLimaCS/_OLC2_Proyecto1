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
        private Object valorPar;
        public Parametro(String id, Tipo tipo) 
        {
            this.id = id;
            this.tipo = tipo;
            this.refer = false;
            this.valorPar = null;
        }

        public String Id { get => id; set => id = value; }
        public Tipo Tipo { get => tipo; set => tipo = value; }
        public bool Referencia { get => refer; set => refer = value; }
        public Object ValorPar { get => valorPar; set => this.valorPar = value; }
    }
}
