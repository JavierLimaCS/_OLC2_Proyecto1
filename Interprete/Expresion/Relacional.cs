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
                            resultado.Value = int.Parse(izquierda.Value.ToString()) != int.Parse(derecha.Value.ToString());
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
                            resultado.Value = int.Parse(izquierda.Value.ToString()) == int.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
            }
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio c3d)
        {
            string code = "";
            string operador = this.tipo.ToString();
            if (this.derecha == null)
            {

            }
            else
            {
                if (operador.Equals("=")) operador = "==";
                if (this.izquierda is Primitivo & this.derecha is Primitivo)
                {
                    code += c3d.tmp.generarTemporal() + " = ";
                    code += this.izquierda.generar3D(ts, c3d);
                    code += operador;
                    code += this.derecha.generar3D(ts, c3d);
                }
                else if (this.izquierda is Primitivo & !(this.derecha is Primitivo))
                {
                    string tmp = "";
                    code += this.derecha.generar3D(ts, c3d);
                    tmp = c3d.tmp.getLastTemporal();
                    code += c3d.tmp.generarTemporal() + " = ";
                    code += this.izquierda.generar3D(ts, c3d);
                    code += operador;
                    code += tmp;
                }
                else if (!(this.izquierda is Primitivo) & this.derecha is Primitivo)
                {
                    string tmp = "";
                    code += this.izquierda.generar3D(ts, c3d);
                    tmp = c3d.tmp.getLastTemporal();
                    code += c3d.tmp.generarTemporal() + " = ";
                    code += tmp;
                    code += operador;
                    code += this.derecha.generar3D(ts, c3d);
                }
                else
                {
                    Relacional izq = (Relacional)this.izquierda;
                    Relacional der = (Relacional)this.derecha;
                    if (izq.tipo == '*' || izq.tipo == '/' || izq.tipo == '%')
                    {
                        code += izq.generar3D(ts, c3d);
                        code += der.generar3D(ts, c3d);
                    }
                    if (der.tipo == '*' || der.tipo == '/' || der.tipo == '%')
                    {
                        code += der.generar3D(ts, c3d);
                        code += izq.generar3D(ts, c3d);
                    }
                }
            }

            return code + ";\n";
        }
    }
}
