using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Optimización
{
    class ReglaB : ReglaM
    {
        string instrucciones;
        string lv;
        string lf;
        public ReglaB(string tipo, string regla, string cod_anter, string cod_nuevo, int fila) : base(tipo, regla,cod_anter,cod_nuevo, fila) 
        { 
            
        
        }
    }
}
