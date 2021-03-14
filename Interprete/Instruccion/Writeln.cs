using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Proyecto1.Interprete.Instruccion
{
    class Writeln : Instruccion
    {
        private LinkedList<Expresion.Expresion> exp_list;
        RichTextBox rc;
        public Writeln(LinkedList<Expresion.Expresion> exp_list, RichTextBox rt) 
        {
            this.exp_list = exp_list;
            this.rc = rt;
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            object valor = "";
            foreach (var exp in this.exp_list) 
            {
                valor += exp.Evaluar(ts).Value.ToString().Replace("'","");
            }
            valor += "\n";
            rc.Text = rc.Text + valor.ToString();
            return "";   
        }
    }
}
