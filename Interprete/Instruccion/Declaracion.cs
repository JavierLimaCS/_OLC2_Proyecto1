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
                nuevo.Scope = TS.alias;
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
            string tipovar = "";
            if (this.constant)
            {
                tipovar = "constante";
            }
            else
            {
                tipovar = "variable";
            }
            string code = "//--- Declaracion de "+tipovar+"---//\n";
            string noval = "";
            foreach (var variable in this.id) 
            {
                if (this.value != null)
                {
                    if (this.value is Primitivo)
                    {
                        noval = this.value.generar3D(ts, inter);
                    }
                    else 
                    {
                        code += this.value.generar3D(ts, inter);
                        noval = inter.tmp.getLastTemporal();
                    }
                }
                else
                {
                    String tipo = this.type.tipoAuxiliar;
                    switch (tipo)
                    {
                        case "boolean":
                        case "integer":
                            noval += 0;
                            break;
                        case "string":
                            noval += "\"\"";
                            break;
                        case "real":
                            noval += 0.0;
                            break;
                    }
                }
                string referencia = "";
                string tmpvar="";
                if (ts.alias.ToLower().Equals("global"))
                {
                    if (this.type.tipoAuxiliar.Equals("integer") || this.type.tipoAuxiliar.Equals("real") || this.type.tipoAuxiliar.Equals("boolean"))
                    {
                        code += inter.tmp.generarTemporal() + " = HP; //referencia a "+tipovar+ " global\n";
                        tmpvar = inter.tmp.getLastTemporal();
                        code += "Heap[(int)" + tmpvar+ "] = " + noval + ";\n";
                        code += "HP = HP + 1; \n";
                        referencia = "Heap[(int)" + tmpvar + "]";
                    }
                    else
                    {
                        code += noval;
                        noval = inter.tmp.getLastTemporal();
                        code += inter.tmp.generarTemporal() + " = HP;   //referencia a " + tipovar + " global\n";
                        code += "Heap[(int)" + inter.tmp.getLastTemporal() + "] = " + noval + ";\n";
                        code += "HP = HP + 1; \n";
                    }
                }
                else 
                {
                    if (this.type.tipoAuxiliar.Equals("integer") || this.type.tipoAuxiliar.Equals("real"))
                    {
                        code += inter.tmp.generarTemporal() + " = SP + " +inter.size +"; //referencia a " + tipovar + " local\n";
                        code += "Stack[(int)" + inter.tmp.getLastTemporal() + "] = " + noval + ";\n";
                    }
                    else
                    {
                        code += noval;
                        noval = inter.tmp.getLastTemporal();
                        code += inter.tmp.generarTemporal() + " = SP + " +inter.size+ ";   //referencia a " + tipovar + " local\n";
                        code += "Stack[(int)" + inter.tmp.getLastTemporal() + "] = " + noval + ";\n";
                    }
                    inter.size++;
                }
                ts.setVariablePos(variable, inter.tmp.getLastTemporal());
                ts.setVariableRef(variable, referencia);
            }

            return code + "\n";
        }
    }
}
