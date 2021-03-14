using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{ 
    public enum Tipos
    {
        INT = 0,
        BOOLEAN = 1,
        STRING = 2,
        REAL = 3,
        FUNCTION =4,
        PARAM = 5,
        OBJ = 6,
        ERROR = 7
    }
    class Tipo
    {
        public Tipos tipo;
        public string tipoAuxiliar;

        public Tipo(Tipos tipo, string tipoAuxiliar)
        {
            this.tipo = tipo;
            this.tipoAuxiliar = tipoAuxiliar;
        }
    }
}
