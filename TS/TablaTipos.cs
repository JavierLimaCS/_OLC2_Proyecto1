using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{
    class TablaTipos
    {
        public static Tipos[,] tipos = new Tipos[4, 4] {
            { Tipos.INT,    Tipos.ERROR,    Tipos.ERROR,    Tipos.ERROR},
            { Tipos.ERROR,  Tipos.BOOLEAN,  Tipos.ERROR,    Tipos.ERROR},
            { Tipos.ERROR,  Tipos.ERROR,    Tipos.STRING,   Tipos.ERROR},
            { Tipos.ERROR,  Tipos.ERROR,    Tipos.ERROR,    Tipos.REAL}
        };

        public static Tipos getTipo(Tipo izquierda, Tipo derecha)
        {
            return tipos[(int)izquierda.tipo, (int)derecha.tipo];
        }
    }
}
