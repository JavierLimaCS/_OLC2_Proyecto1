using Proyecto1.Analisis;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1.Interprete.Instruccion
{
    abstract class Instruccion
    {
        public List<Error> Semanticos;
        public abstract object Ejecutar(TabladeSimbolos ts);

        public List<Error> listaErrores
        {
            get
            {
                return Semanticos;
            }

            set
            {
                Semanticos = value;
            }
        }
    }
}
