using Proyecto1.Codigo3D;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class DeclaObjeto : Instruccion
    {
        String id;
        Objeto objectt;
        public DeclaObjeto(String id, Objeto objectt) 
        {
            this.id = id.ToLower();
            this.objectt = objectt;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
             ts.declararObjeto(id, objectt);
            foreach (var atributo in objectt.Attribs) {
                Simbolo attr = new Simbolo(atributo.Id, atributo.Tipo, atributo.Linea, atributo.Columna, false);
                ts.declararVariable(attr.Id, attr);
            }
            return null;
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            return "";
        }
    }
}
