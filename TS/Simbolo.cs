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
        private bool isParam;
        private string pos;

        public Simbolo(string n, Tipo ty, int l, int c)
        {
            this.id = n;
            this.Tipo = ty;
            this.line = l;
            this.column = c;
            this.pos = "";
            this.isParam = false;
        }

        public Simbolo(String n, Tipo ty, int l, int c, bool b) {
            this.id = n;
            this.type = ty;
            this.line = l;
            this.column = c;
            this.isConstant = b;
            this.pos = "";
            this.isParam = false;
        }

        public String Id { get => id; set => id = value; }
        public Tipo Tipo { get => type; set => type = value; }
        public int Linea { get => line; set => line = value; }
        public int Columna { get => column; set => column = value; }
        public Object Value { get => value; set => this.value = value; }

        public bool esConstante { get => isConstant; set => isConstant = value; }
        public bool esParametro { get => isParam; set => isParam = value; }
        public string Pos { get => pos; set => pos = value; }
    }
}
