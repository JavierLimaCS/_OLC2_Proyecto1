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
        private string scope;
        private string referencia;
        private bool esReferencia;

        public Simbolo(string n, Tipo ty, int l, int c)
        {
            this.id = n;
            this.Tipo = ty;
            this.line = l;
            this.column = c;
            this.pos = "";
            this.isParam = false;
            this.scope = "";
            this.referencia = "";
            this.esReferencia = false;
        }

        public Simbolo(String n, Tipo ty, int l, int c, bool b) {
            this.id = n;
            this.type = ty;
            this.line = l;
            this.column = c;
            this.isConstant = b;
            this.pos = "";
            this.isParam = false;
            this.scope = "";
            this.esReferencia = false;
        }

        public String Id { get => id; set => id = value; }
        public Tipo Tipo { get => type; set => type = value; }
        public int Linea { get => line; set => line = value; }
        public int Columna { get => column; set => column = value; }
        public Object Value { get => value; set => this.value = value; }

        public bool esConstante { get => isConstant; set => isConstant = value; }
        public bool esParametro { get => isParam; set => isParam = value; }
        public string Pos { get => pos; set => pos = value; }
        public string Scope { get => scope; set => scope = value; }
        public string Referencia { get => referencia; set => referencia = value; }
        public bool isReferencia { get => esReferencia; set => esReferencia = value; }
    }
}
