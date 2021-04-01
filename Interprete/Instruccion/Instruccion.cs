using Proyecto1.Analisis;
using Proyecto1.Codigo3D;
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
        public List<Error> Semanticos = new List<Error>();
        public abstract object Ejecutar(TabladeSimbolos ts);

        public abstract string generar3D(TabladeSimbolos ts, Intermedio c3d);

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
