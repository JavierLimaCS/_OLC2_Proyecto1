using Proyecto1.Codigo3D;
using Proyecto1.Interprete.Expresion;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class If : Instruccion
    {
        private Expresion.Expresion valor;
        private LinkedList<Instruccion> instrucciones;
        private Instruccion _else;
        object salida;
        public string lv;
        public string lf;
        public If(Expresion.Expresion valor, LinkedList<Instruccion> instrucciones, Instruccion _else)
        {
            this.valor = valor;
            this.instrucciones = instrucciones;
            this._else = _else;
            this.Semanticos = new List<Analisis.Error>();
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            Simbolo valor = this.valor.Evaluar(ts);
            if (valor == null || valor.Value==null) 
            {
                this.listaErrores.Add(new Analisis.Error("Semantico","La condicion es incorrecta en sentencia IF",0,0));
                return this.salida = "";
            }
            if (valor.Tipo.tipo != Tipos.BOOLEAN)
            {
                this.listaErrores.Add(new Analisis.Error("Semantico", "La condicion no es BOOLEANA en sentencia IF", 0, 0));
                return this.salida;
            }

            if (bool.Parse(valor.Value.ToString()))
            {
                try
                {
                    foreach (var instruccion in instrucciones)
                    {
                        if (instruccion != null)
                        {
                            Object output = instruccion.Ejecutar(ts);
                            if (output is Break)
                            {
                                return output;
                            }
                            else if (output is Continue)
                            {
                                break;
                            }
                            else if (output is Exit)
                            {
                                return output;
                            }
                            else
                            {
                                this.salida = output;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                if (_else != null) 
                {
                    this.salida = _else.Ejecutar(ts);
                }
            }
            return this.salida;
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string code = "//------- Sentencia Decision If \n";
            if (inter.ls.Equals("")) inter.ls = inter.label.generarLabel();
            if (this.valor is Primitivo)
            {
                code += inter.tmp.generarTemporal() + " = " + this.valor.generar3D(ts, inter) + ";\n";
            }
            else 
            {
                code += this.valor.generar3D(ts, inter);
            }
            code += "if (" + inter.tmp.getLastTemporal() + "==1) goto " + inter.label.generarLabel() + ";\n";
            lv = inter.label.getLastLabel();
            code += "goto " + inter.label.generarLabel() + ";\n";
            lf = inter.label.getLastLabel();
            code += lv + ":\n";
            foreach (var inst in this.instrucciones)
            {
                code += inst.generar3D(ts, inter);
            }
            code += "\ngoto " + inter.ls + ";\n";
            code += lf + ":\n";
            if (this._else != null) {
                if (this._else is If) code += "/----- Sentencia Else-If\n";
                code += this._else.generar3D(ts, inter);
            }
            if (!(inter.ls.Equals(""))) 
            {
                code += inter.ls + ":\n";
                inter.ls = "";
            }    
            return code;
        }
    }
}
