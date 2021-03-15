using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{
    class Array : Simbolo
    {
        List<int> indices;

        public Array(String n, Tipo ty, object valor, int l, int c) : base(n, ty, l, c)
        { }

    }
}
