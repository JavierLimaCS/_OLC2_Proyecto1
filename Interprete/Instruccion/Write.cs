using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Proyecto1.Interprete.Instruccion
{
    class Write : Instruccion
    {
        private LinkedList<Expresion.Expresion> exp_list;
        RichTextBox rc;
        public Write(LinkedList<Expresion.Expresion> exp_list, RichTextBox rc)
        {
            this.rc = rc;
            this.exp_list = exp_list;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            object valor = "";
            foreach (var exp in this.exp_list)
            {
                valor += exp.Evaluar(ts).Value.ToString().Replace("'", "");
            }
            rc.Text = rc.Text + valor.ToString();
            return "";
        }
    }
}
