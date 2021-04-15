using Proyecto1.Codigo3D;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.Interprete.Expresion
{
    class Aritmetica : Expresion
    {
        public Expresion izquierda;
        public Expresion derecha;
        public char tipo;

        public Aritmetica(Expresion izquierda, Expresion derecha, char tipo)
        {
            this.izquierda = izquierda;
            this.derecha = derecha;
            this.tipo = tipo;
        }
        public override Simbolo Evaluar(TabladeSimbolos ts)
        {
            Simbolo izquierda = this.izquierda.Evaluar(ts);
            Simbolo derecha = null;
            Simbolo resultado;
            Tipos tipoResultante;
            if (this.derecha != null)
            {
                derecha = this.derecha.Evaluar(ts);
                tipoResultante = TablaTipos.getTipo(izquierda.Tipo, derecha.Tipo);
            }
            else 
            {
                tipoResultante = TablaTipos.getTipo(izquierda.Tipo, izquierda.Tipo);
            }
            if ((int)tipoResultante==7)
                throw new Exception();

            switch (tipo)
            {
                case '+':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    switch (tipoResultante.ToString().ToLower()) 
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) + int.Parse(derecha.Value.ToString());
                            break;
                        case "string":
                            resultado.Value = izquierda.Value.ToString() + derecha.Value.ToString();
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) + decimal.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                case '-':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    if (this.derecha == null)
                    {
                        switch (tipoResultante.ToString().ToLower())
                        {
                            case "int":
                                resultado.Value = - int.Parse(izquierda.Value.ToString());
                                break;
                            case "real":
                                resultado.Value = - decimal.Parse(izquierda.Value.ToString());
                                break;
                        }

                    }
                    else 
                    {
                        switch (tipoResultante.ToString().ToLower())
                        {
                            case "int":
                                resultado.Value = int.Parse(izquierda.Value.ToString()) - int.Parse(derecha.Value.ToString());
                                break;
                            case "real":
                                resultado.Value = decimal.Parse(izquierda.Value.ToString()) - decimal.Parse(derecha.Value.ToString());
                                break;
                        }
                    }
                    return resultado;
                case '*':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) * int.Parse(derecha.Value.ToString());
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) * decimal.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                case '/':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) / decimal.Parse(derecha.Value.ToString());
                            resultado.Tipo = new Tipo(Tipos.REAL, "real");
                            break;
                    }
                    return resultado;
                case 'd':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) / int.Parse(derecha.Value.ToString());
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) / decimal.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                case 'm':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) % int.Parse(derecha.Value.ToString());
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) % decimal.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                default:
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) % int.Parse(derecha.Value.ToString());
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) % decimal.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
            }
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio c3d)
        {
            string code = "";
            string izquierdaval = "";
            string derechaval = "";
            if (this.derecha == null)
            {
                code += this.izquierda.generar3D(ts, c3d);
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
                    code += c3d.tmp.generarTemporal() + " = ";
                    code += izquierdaval;
                    code += this.tipo.ToString();
                    code += derechaval;
                }
                else if (this.izquierda is Primitivo & !(this.derecha is Primitivo))
                {
                    izquierdaval = this.izquierda.generar3D(ts, c3d);
                    if (izquierdaval.Contains("Heap") || izquierdaval.Contains("Stack"))
                    {
                        code += izquierdaval + "\n";
                        izquierdaval = c3d.tmp.getLastTemporal();
                    }
                    string tmp = "";
                    code += this.derecha.generar3D(ts, c3d);
                    tmp = c3d.tmp.getLastTemporal();
                    code += c3d.tmp.generarTemporal() + " = ";
                    code += izquierdaval;
                    code += this.tipo.ToString();
                    code += tmp;
                }
                else if (!(this.izquierda is Primitivo) & this.derecha is Primitivo)
                {
                    derechaval = this.derecha.generar3D(ts, c3d);
                    if (derechaval.Contains("Heap") || derechaval.Contains("Stack"))
                    {
                        code += derechaval + "\n";
                        derechaval = c3d.tmp.getLastTemporal();
                    }
                    string tmp = "";
                    code += this.izquierda.generar3D(ts, c3d);
                    tmp = c3d.tmp.getLastTemporal();
                    code += c3d.tmp.generarTemporal() + " = ";
                    code += tmp;
                    code += this.tipo.ToString();
                    code += derechaval;
                }
                else
                {
                    Aritmetica izq = (Aritmetica)this.izquierda;
                    Aritmetica der = (Aritmetica)this.derecha;
                    string tmpizq = "";
                    string tmpder = "";
                    if (izq.tipo == '*' || izq.tipo == '/' || izq.tipo == '%')
                    {
                        code += izq.generar3D(ts, c3d);
                        tmpizq = c3d.tmp.getLastTemporal();
                        code += der.generar3D(ts, c3d);
                        tmpder = c3d.tmp.getLastTemporal();
                    }
                    if (der.tipo == '*' || der.tipo == '/' || der.tipo == '%')
                    {
                        code += der.generar3D(ts, c3d);
                        tmpder = c3d.tmp.getLastTemporal();
                        code += izq.generar3D(ts, c3d);
                        tmpizq = c3d.tmp.getLastTemporal();
                    }
                    else 
                    {
                        code += der.generar3D(ts, c3d);
                        tmpder = c3d.tmp.getLastTemporal();
                        code += izq.generar3D(ts, c3d);
                        tmpizq = c3d.tmp.getLastTemporal();
                    }
                    code += c3d.tmp.generarTemporal() + " = " + tmpizq + this.tipo.ToString() + tmpder;
                    
                }
            }
            
            return code + ";\n";
        }
    }
}
