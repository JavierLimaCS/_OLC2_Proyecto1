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
        private int line, column;
        private Object value;
        private bool isConstant;
        private string n;
        private Tipo ty;
        private int l;
        private int c;

        public Simbolo(string n, Tipo ty, int l, int c)
        {
            this.n = n;
            this.ty = ty;
            this.l = l;
            this.c = c;
        }

        public Simbolo(String n, Tipo ty, int l, int c, bool b) {
            this.id = n;
            this.type = ty;
            this.line = l;
            this.column = c;
            this.isConstant = b;
        }

        public String Id { get => id; set => id = value; }
        public Tipo Tipo { get => type; set => type = value; }
        public int Linea { get => line; set => line = value; }
        public int Columna { get => column; set => column = value; }
        public Object Value { get => value; set => this.value = value; }

        public bool esConstante { get => isConstant; set => isConstant = value; }
    }
}
