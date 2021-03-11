using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{ 
    public enum Tipos
    {
        ERROR = 0,
        INT = 1,
        BOOLEAN = 2,
        STRING = 3,
        REAL = 4  ,
        FUNCTION =5,
        PARAM = 6,
        OBJ = 7
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
