using Proyecto1.Codigo3D;
using Proyecto1.Interprete.Expresion;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Declaracion : Instruccion
    {
        private List<String> id; 
        private Tipo type;
        private Expresion.Expresion value;
        private int line, col;
        bool constant;
        public Declaracion(Tipo tipo, List<String> id, Expresion.Expresion value, int linea, int col, bool c) 
        {
            this.type = tipo;
            this.id = id;
            this.value = value;
            this.line = linea;
            this.col = col;
            this.constant = c;
        }
        public override object Ejecutar(TabladeSimbolos TS)
        {
            Tipo tipo_variable =  this.type;
            Simbolo nuevo = null;
            Object valor = null;
            String ids = "";
            foreach (var variable in this.id) 
            {
                nuevo = new Simbolo(variable.ToLower(), tipo_variable, this.line, this.col, this.constant);
                ids += " |" + variable +"| ";
                if (this.value != null)
                {
                    valor = this.value.Evaluar(TS).Value;
                    nuevo.Value = valor;
                }
                else 
                {
                    String tipo = this.type.tipoAuxiliar;
                    switch (tipo) 
                    {
                        case "integer":
                            nuevo.Value = 0;
                            break;
                        case "string":
                            nuevo.Value = "";
                            break;
                        case "real":
                            nuevo.Value = 0.0;
                            break;
                        case "boolean":
                            nuevo.Value = false;
                            break;
                        default:
                            Objeto nuevo_objeto = TS.getObjeto(this.type.tipoAuxiliar);
                            if (nuevo_objeto != null) 
                            {
                                foreach (var attr in nuevo_objeto.Attribs)
                                {
                                    switch (attr.Tipo.tipoAuxiliar)
                                    {
                                        case "integer":
                                            attr.Value = 0;
                                            break;
                                        case "boolean":
                                            attr.Value = false;
                                            break;
                                        case "string":
                                            attr.Value = "";
                                            break;
                                        case "real":
                                            attr.Value = 0.0;
                                            break;
                                        default:
                                            Objeto objeto_attr = TS.getObjeto(attr.Tipo.tipoAuxiliar);
                                            if (objeto_attr != null) 
                                            {
                                                foreach (var atrib in objeto_attr.Attribs)
                                                {
                                                    switch (atrib.Tipo.tipoAuxiliar)
                                                    {
                                                        case "integer":
                                                            atrib.Value = 0;
                                                            break;
                                                        case "boolean":
                                                            atrib.Value = false;
                                                            break;
                                                        case "string":
                                                            atrib.Value = "";
                                                            break;
                                                        case "real":
                                                            atrib.Value = 0.0;
                                                            break;
                                                        default:
                                                            Objeto objeto_attr2 = TS.getObjeto(attr.Tipo.tipoAuxiliar);
                                                            attr.Value = objeto_attr;
                                                            break;
                                                    }
                                                }
                                            }
                                            attr.Value = objeto_attr;
                                            break;
                                    }
                                }
                            }
                           
                            nuevo.Value = nuevo_objeto;
                            break;
                    }
                }
                TS.declararVariable(variable, nuevo);
            }
            //return "Variables " + ids  + " se ha insertado en los TS\n";
            return null;
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio inter)
        {
            string code = "//Declaracion de Variable \n";
            string noval = "";
            foreach (var variable in this.id) 
            {
                if (this.value != null)
                {
                    if (this.value is Primitivo)
                    {
                        code += variable + " = " + this.value.generar3D(ts, inter) + ";\n";
                        continue;
                    }
                    else 
                    {
                        code += this.value.generar3D(ts, inter);
                    }
                }
                else
                {
                    String tipo = this.type.tipoAuxiliar;
                    switch (tipo)
                    {
                        case "integer":
                            noval += 0;
                            break;
                        case "string":
                            noval += "\"\"";
                            break;
                        case "real":
                            noval += 0.0;
                            break;
                        case "boolean":
                            noval += false;
                            break;
                    }
                    code += variable + " = " + noval + ";\n";
                    continue;
                }
                code += variable + " = " + inter.tmp.getLastTemporal() + "; \n";
            }

            return code + "\n";
        }
    }
}
