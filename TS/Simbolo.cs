using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Proyecto1.TS.Tipo;

namespace Proyecto1.TS
{
    class Simbolo
    {
        private String id;
        private Tipo type;
        private String scope;
        private int line, column;
        private Object value;

        public Simbolo(String n, Tipo ty, int l, int c) {
            this.id = n;
            this.type = ty;
            this.line = l;
            this.column = c;
        }

        public String Id { get => id; set => id = value; }
        public Tipo Tipo { get => type; set => type = value; }
        public int Linea { get => line; set => line = value; }
        public int Columna { get => column; set => column = value; }
        public Object Value { get => value; set => this.value = value; }
    }
}
