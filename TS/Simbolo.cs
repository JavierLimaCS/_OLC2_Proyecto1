using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.TS
{
    class Simbolo
    {
        private String id;
        private Tipo type;
        private String scope;
        private int line, column;
        private Object value;

        public Simbolo(String n, Tipo ty, String sc, int l, int c) {
            this.id = n;
            this.type = ty;
            this.scope = sc;
            this.line = l;
            this.column = c;
        }

        public String Id { get => id; set => id = value; }
        public Tipo Tipe { get => type; set => type = value; }
        public String Scope { get => scope; set => scope = value; }
        public int Linea { get => line; set => line = value; }
        public int Columna { get => column; set => column = value; }
        public Object Value { get => value; set => value = value; }

        public enum Tipo {
            INT,
            STR,
            REAL,
            BOOL,
            VOID,
            FUNC,
            TYPE,
            OBJ,
            ARR,
            NULL,
            ERROR
        }
    }
}
