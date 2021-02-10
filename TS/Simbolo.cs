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
        private int line;
        private int column;
        private Object value;

        public Simbolo(String n, Tipo ty, String sc, int l, int c) {
            this.id = n;
            this.type = ty;
            this.scope = sc;
            this.line = l;
            this.column = c;
        }

        public String getId() 
        {
            return this.id;
        }

        public void setId(string id) 
        {
            this.id = id;
        }

        public Tipo getType()
        {
            return this.type;
        }

        public void setType(Tipo tipo)
        {
            this.type = tipo;
        }

        public String getScope()
        {
            return this.scope;
        }

        public void setScope(string s)
        {
            this.scope = s;
        }


        public Object getValue()
        {
            return this.value;
        }

        public void setValue(Object val)
        {
            this.value = val;
        }

        public int getLine()
        {
            return this.line;
        }

        public void setLine(int line)
        {
            this.line = line;
        }

        public int getColumn()
        {
            return this.column;
        }

        public void setColumn(int col)
        {
            this.column = col;
        }


        public enum Tipo { 
            INT,    
            STR,
            REAL,
            BOOL,
            VOID,
            FUNC,
            TYPE,
            OBJ,
            ARR
        }
    }
}
