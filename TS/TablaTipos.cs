using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{
    class TablaTipos
    {
        public static Tipos[,] tipos = new Tipos[2, 2] {
            { Tipos.INT, Tipos.ERROR },
            { Tipos.ERROR, Tipos.BOOLEAN }
        };

        public static Tipos getTipo(Tipo izquierda, Tipo derecha)
        {
            return tipos[(int)izquierda.tipo, (int)derecha.tipo];
        }
    }
}
