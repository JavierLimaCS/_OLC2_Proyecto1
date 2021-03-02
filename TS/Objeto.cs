using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{
    class Objeto
    {
        private String id;
        private List<Atributo> attribs;
        public Objeto(String id, List<Atributo> att)
        {
            this.id = id;
            this.Attribs = att;
        }

        public string Id { get => id; set => id = value; }
        internal List<Atributo> Attribs { get => attribs; set => attribs = value; }
    }
}
