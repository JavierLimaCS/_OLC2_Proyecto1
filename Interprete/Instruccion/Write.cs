using Proyecto1.Codigo3D;
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

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string code = "";
            Simbolo valor;
            foreach (var exp in this.exp_list)
            {
                valor = (Simbolo)exp.Evaluar(ts);
                code += exp.generar3D(ts, inter);
                switch (valor.Tipo.tipoAuxiliar)
                {
                    case "string":
                        char[] mensaje = valor.Value.ToString().Replace("'", "").ToCharArray();
                        for (int i = 0; i < mensaje.Length; i++)
                        {
                            char tmp = mensaje[i];
                            code += "printf(\"%c\", " + Convert.ToInt32(tmp) + "); \n";
                        }
                        break;
                    case "integer":
                        code += "printf(\"%d\", " + inter.tmp.getLastTemporal() + "); \n";
                        break;
                    case "decimal":
                        code += "printf(\"%f\", " + inter.tmp.getLastTemporal() + "); \n";
                        break;
                }
            }
            return code + "\n";
        }
    }
}
