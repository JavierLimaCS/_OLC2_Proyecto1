using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{
    class Objeto
    {
        private String id;
        private List<Atributo> attribs;
        private int linea, columna;
        public Objeto(String id, List<Atributo> att, int l, int c)
        {
            this.id = id;
            this.Attribs = att;
            this.linea = l;
            this.columna = c;
        }

        public string Id { get => id; set => id = value; }
        internal List<Atributo> Attribs { get => attribs; set => attribs = value; }
        public int Linea { get => linea; set => linea = value; }
        public int Columna { get => columna; set => columna = value; }
    }
}
