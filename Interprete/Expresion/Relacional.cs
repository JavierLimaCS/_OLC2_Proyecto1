using Proyecto1.Codigo3D;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Expresion
{
    class Relacional : Expresion
    {
        private Expresion izquierda;
        private Expresion derecha;
        public char tipo;
        public Relacional(Expresion izquierda, Expresion derecha, char tipo) 
        {
            this.izquierda = izquierda;
            this.derecha = derecha;
            this.tipo = tipo;
        }
        public override Simbolo Evaluar(TabladeSimbolos ts)
        {
            Simbolo izquierda = this.izquierda.Evaluar(ts);
            Simbolo derecha = this.derecha.Evaluar(ts);
            Simbolo resultado;
            Tipos tipoResultante = TablaTipos.getTipo(izquierda.Tipo, derecha.Tipo);

            //if (tipoResultante != Tipos.INT && tipo != '+')
            //  throw new Exception();

            switch (tipo)
            {
                case '>':
                    resultado = new Simbolo(null, new Tipo(Tipos.BOOLEAN,"boolean"), 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) > int.Parse(derecha.Value.ToString());
                            break;
                        case "string":
                            resultado.Value = izquierda.Value.ToString().Length > derecha.Value.ToString().Length;
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) > decimal.Parse(derecha.Value.ToString());
                            break;
                        case "boolean":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) > int.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                case '<':
                    resultado = new Simbolo(null, new Tipo(Tipos.BOOLEAN, "boolean"), 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) < int.Parse(derecha.Value.ToString());
                            break;
                        case "string":
                            resultado.Value = izquierda.Value.ToString().Length < derecha.Value.ToString().Length;
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) < decimal.Parse(derecha.Value.ToString());
                            break;
                        case "boolean":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) < int.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                case '!':
                    resultado = new Simbolo(null, new Tipo(Tipos.BOOLEAN, "boolean"), 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) != int.Parse(derecha.Value.ToString());
                            break;
                        case "string":
                            resultado.Value = !izquierda.Value.ToString().Equals(derecha.Value.ToString());
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) != decimal.Parse(derecha.Value.ToString());
                            break;
                        case "boolean":
                            resultado.Value = bool.Parse(izquierda.Value.ToString()) != bool.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                case 'm':
                    resultado = new Simbolo(null, new Tipo(Tipos.BOOLEAN, "boolean"), 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) >= int.Parse(derecha.Value.ToString());
                            break;
                        case "string":
                            resultado.Value = izquierda.Value.ToString().Length >= derecha.Value.ToString().Length;
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) >= decimal.Parse(derecha.Value.ToString());
                            break;
                        case "boolean":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) >= int.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                case 'i':
                    resultado = new Simbolo(null, new Tipo(Tipos.BOOLEAN, "boolean"), 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) <= int.Parse(derecha.Value.ToString());
                            break;
                        case "string":
                            resultado.Value = izquierda.Value.ToString().Length <= derecha.Value.ToString().Length;
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) <= decimal.Parse(derecha.Value.ToString());
                            break;
                        case "boolean":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) <= int.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                default:
                    resultado = new Simbolo(null, new Tipo(Tipos.BOOLEAN, "boolean"), 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) == int.Parse(derecha.Value.ToString());
                            break;
                        case "string":
                            resultado.Value = izquierda.Value.ToString().Equals(derecha.Value.ToString());
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) == decimal.Parse(derecha.Value.ToString());
                            break;
                        case "boolean":

                            resultado.Value = bool.Parse(izquierda.Value.ToString()) == bool.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
            }
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio c3d)
        {
            string code = "";
            string tmpglobal = "";
            string lv = "";
            string lf = "";
            string izquierdaval = "";
            string derechaval = "";
            string operador = "";
            switch (this.tipo) 
            {
                case '!':
                    operador = "!=";
                    break;
                case 'i':
                    operador = "<=";
                    break;
                case 'm':
                    operador = ">=";
                    break;
                case '<':
                    operador = "<";
                    break;
                case '>':
                    operador = ">";
                    break;
                default:
                    operador = "==";
                    break;
            }
            if (this.derecha == null)
            {
                
            }
            else
            {
                if (this.izquierda is Primitivo & this.derecha is Primitivo)
                {
                    izquierdaval = this.izquierda.generar3D(ts, c3d);
                    if (izquierdaval.Contains("Heap") || izquierdaval.Contains("Stack")) 
                    {
                        code += izquierdaval + "\n";
                        izquierdaval = c3d.tmp.getLastTemporal();
                    }
                    derechaval = this.derecha.generar3D(ts, c3d);
                    if (derechaval.Contains("Heap") || derechaval.Contains("Stack")) 
                    {
                        code += derechaval + "\n";
                        derechaval = c3d.tmp.getLastTemporal();
                    } 
                    code += "if(";
                    code += izquierdaval + operador + derechaval;
                    code += ") goto " + c3d.label.generarLabel() + ";\n";
                    lv = c3d.label.getLastLabel();
                    code += c3d.tmp.generarTemporal() + " = 0;\n";
                    tmpglobal = c3d.tmp.getLastTemporal();
                    code += "goto " + c3d.label.generarLabel() + ";\n";
                    lf = c3d.label.getLastLabel();
                    code += lv + ":\n";
                    code += tmpglobal + " = 1;\n";
                    code += lf + ":\n\n";
                }
                else if (this.izquierda is Primitivo & !(this.derecha is Primitivo))
                {
                    izquierdaval = this.izquierda.generar3D(ts, c3d);
                    if (izquierdaval.Contains("Heap") || izquierdaval.Contains("Stack"))
                    {
                        code += izquierdaval + "\n";
                        izquierdaval = c3d.tmp.getLastTemporal();
                    }
                    derechaval = this.derecha.generar3D(ts, c3d);
                    code += derechaval + "\n";
                    derechaval = c3d.tmp.getLastTemporal();
                    code += "if(";
                    code += izquierdaval + operador + derechaval;
                    code += ") goto " + c3d.label.generarLabel() + ";\n";
                    lv = c3d.label.getLastLabel();
                    code += c3d.tmp.generarTemporal() + " = 0;\n";
                    tmpglobal = c3d.tmp.getLastTemporal();
                    code += "goto " + c3d.label.generarLabel() + ";\n";
                    lf = c3d.label.getLastLabel();
                    code += lv + ":\n";
                    code += tmpglobal + " = 1;\n";
                    code += lf + ":\n\n";
                }
                else if (!(this.izquierda is Primitivo) & this.derecha is Primitivo)
                {
                    izquierdaval = this.derecha.generar3D(ts, c3d);
                    code += izquierdaval + "\n";
                    izquierdaval = c3d.tmp.getLastTemporal();
                    if (derechaval.Contains("Heap") || derechaval.Contains("Stack"))
                    {
                        code += derechaval + "\n";
                        derechaval = c3d.tmp.getLastTemporal();
                    }
                    code += "if(";
                    code += izquierdaval + operador + derechaval;
                    code += ") goto " + c3d.label.generarLabel() + ";\n";
                    lv = c3d.label.getLastLabel();
                    code += c3d.tmp.generarTemporal() + " = 0;\n";
                    tmpglobal = c3d.tmp.getLastTemporal();
                    code += "goto " + c3d.label.generarLabel() + ";\n";
                    lf = c3d.label.getLastLabel();
                    code += lv + ":\n";
                    code += tmpglobal + " = 1;\n";
                    code += lf + ":\n\n";
                }
                else
                {
                    Relacional izq = (Relacional)this.izquierda;
                    Relacional der = (Relacional)this.derecha;
                    code += izq.generar3D(ts, c3d);
                    code += der.generar3D(ts, c3d);
                }
            }

            return code;
        }
    }
}
