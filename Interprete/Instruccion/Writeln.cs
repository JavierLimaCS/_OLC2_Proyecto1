using Proyecto1.Codigo3D;
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
        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string code = "";
            Simbolo valor;
            foreach (var exp in this.exp_list)
            {
                valor = (Simbolo)exp.Evaluar(ts);
                switch (valor.Tipo.tipoAuxiliar) 
                {
                    case "string":
                        char[] mensaje = valor.Value.ToString().Replace("'", "").ToCharArray();
                        for (int i = 0; i < mensaje.Length; i ++) 
                        {
                            char tmp = mensaje[i];
                            code += "   printf(\"%c\", " + Convert.ToInt32(tmp) + "); \n";
                        }
                        break;
                    case "integer":
                        code += "   printf(\"%d\", "+valor.Value+"); \n";
                        break;
                    case "decimal":
                        code += "   printf(\"%f\", " +valor.Value+"); \n";
                        break;
                }
            }
            return code + "\n";
        }
    }
}
