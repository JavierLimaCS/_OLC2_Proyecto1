using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.TS
{
    class TabladeSimbolos
    {
        TabladeSimbolos padre;
        public String alias;
        public Dictionary<string, Simbolo> variables;
        public Dictionary<string, Simbolo_Funcion> funciones;
        public Dictionary<string, Objeto> types;
        public Dictionary<string, Arreglo> arreglos;
        public TabladeSimbolos(TabladeSimbolos padre, String alias)
        {
            this.padre = padre;
            this.alias = alias;
            this.variables = new Dictionary<string, Simbolo>();
            this.funciones = new Dictionary<string, Simbolo_Funcion>();
            this.types = new Dictionary<string, Objeto>();
            this.arreglos = new Dictionary<string, Arreglo>();
        }

        public Simbolo getVariableValor(String id)
        {
            TabladeSimbolos actual = this;
            while (actual != null)
            {
                if(actual.variables.ContainsKey(id))
                    return actual.variables[id];
                actual = actual.padre;
            };
            return null;
        }

        public string getVariablePos(string id) 
        {
            TabladeSimbolos actual = this;
            string ambito = "g";
            while (actual != null)
            {
                if (actual.variables.ContainsKey(id))
                    return actual.variables[id].Pos + ":" + ambito;
                actual = actual.padre;
                ambito = actual.alias;
            };
            return null;
        }

        public Simbolo_Funcion getFuncion(String id)
        {
            TabladeSimbolos actual = this;
            while (actual != null)
            {
                if (actual.funciones.ContainsKey(id))
                    return actual.funciones[id];
                actual = actual.padre;
            };
            return null;
        }


        public Objeto getObjeto(String id)
        {
            TabladeSimbolos actual = this;
            while (actual != null)
            {
                if (actual.types.ContainsKey(id))
                    return actual.types[id];
                actual = actual.padre;
            };
            return null;
        }

        public Arreglo getArray(String id)
        {
            TabladeSimbolos actual = this;
            while (actual != null)
            {
                if (actual.types.ContainsKey(id))
                    return actual.arreglos[id];
                actual = actual.padre;
            };
            return null;
        }

        public bool setVariableValor(String id, object valor) 
        {
            TabladeSimbolos actual = this;
            while (actual != null)
            {
                if (actual.variables.ContainsKey(id))
                {
                    actual.variables[id].Value = valor;
                    return true;
                }
                actual = actual.padre;
            };
            return false;
        }

        public bool setVariablePos(string id, string tmp) 
        {
            TabladeSimbolos actual = this;
            while (actual != null)
            {
                if (actual.variables.ContainsKey(id))
                {
                    actual.variables[id].Pos = tmp;
                    return true;
                }
                actual = actual.padre;
            };
            return false;
        }

        public bool setValorAccesos(List<string> accesos, object valor)
        {
            TabladeSimbolos actual = this;
            int cont = 0; 
            while (actual != null)
            {
                if (actual.variables.ContainsKey(accesos.ElementAt(cont)))
                {
                    Objeto objt = (Objeto)actual.variables[accesos.ElementAt(cont)].Value;
                    cont++;
                    foreach (var ats in objt.Attribs)
                    {
                        if (ats.Id == accesos.ElementAt(cont))
                        {
                            if (ats.Value is Objeto || ats.Tipo.tipo == Tipos.OBJ)
                            {
                                Objeto tmp = (Objeto)ats.Value;
                                cont++;
                                foreach (var at in tmp.Attribs) 
                                {
                                    if (at.Id == accesos.ElementAt(cont)) 
                                    {
                                        at.Value = valor;
                                    }
                                }
                            }
                            else 
                            {
                                ats.Value = valor;
                            }
                            return true;
                        }
                    }
                }
                actual = actual.padre;
            };
            return false;
        }

        public void setAttrValor(Objeto id, String atr, object valor)
        {
            TabladeSimbolos actual = this;
            while (actual != null)
            {
                Objeto objt = id;
                foreach (var ats in objt.Attribs)
                {
                    if (ats.Id == atr)
                    {
                         ats.Value = valor;
                    }
                }
                
            };
        }

        public bool setFuncionValor(String id, object valor)
        {
            TabladeSimbolos actual = this;
            while (actual != null)
            {
                if (actual.funciones.ContainsKey(id))
                {
                    actual.funciones[id].Value = valor;
                    return true;
                }
                actual = actual.padre;
            };
            return false;
        }

        public bool setValorIndiceArreglo(int indice, object valor, string id) 
        {
            TabladeSimbolos actual = this;
            while (actual != null)
            {
                if (actual.arreglos.ContainsKey(id))
                {
                    actual.arreglos[id].Elementos[indice] = valor;
                    return true;
                }
                actual = actual.padre;
            };
            return false;

        }

        public void declararVariable(string id, Simbolo variable)
        {
            if (!this.variables.ContainsKey(id))
            {
                this.variables.Add(id, variable);
            }
            else
            {
                throw new Exception("La variable " + id + " ya existe en este ambito");
            }
        }

        public void declararFuncion(string id, Simbolo_Funcion funcion)
        {
            if (!this.funciones.ContainsKey(id))
            {
                this.funciones.Add(id, funcion);
            }
            else
            {
                throw new Exception("La funcion " + id + " ya existe en este ambito");
            }
        }

        public void declararObjeto(string id, Objeto objeto)
        {
            if (!this.types.ContainsKey(id))
            {
                this.types.Add(id, objeto);
            }
            else
            {
                throw new Exception("El objeto " + id + " ya existe en este ambito.");
            }
        }

        public void declararArreglo(string id, Arreglo arreglo) 
        {
            if (!this.arreglos.ContainsKey(id))
            {
                this.arreglos.Add(id, arreglo);
            }
            else
            {
                throw new Exception("El objeto " + id + " ya existe en este ambito.");
            }
        }


        public void generarTS(String entorno)
        {
            String simbolo = "<html>\n <head>" +
                "<meta charset=\"utf - 8\"/>" +
                "<title> Reporte de Tabla de Simbolos</title>" +
                "<meta name=\"viewport\" content=\"initial-scale=1.0; maximum-scale=1.0; width=device-width;\">" +
                "<link rel=\"stylesheet\" href=\"style.css\">\n" +
                "</head>\n" +
                "<body>" + "\n" +
                "<div class=\"table-title\">" +
                "<h2 style=\"text-align:center;\">Tabla de Simbolos</h2>\n"+
                "</div> \n"+
                "<table class=\"table-fill\"> " + "\n" +
                "<thead>\n" +
                "<tr><th class=\"text-left\">IDENTIFICADOR</th>" + "\n" +
                "    <th class=\"text-left\">TIPO</th>" + "\n" +
                "    <th class=\"text-left\">LINEA</th> " + "\n" +
                "    <th class=\"text-left\">COLUMNA</th>" + "\n" +
                "    <th class=\"text-left\">AMBITO</th>" + "\n" +
                "</tr> \n</thead>\n <tbody class=\"table-hover\"> \n";
            if (this.padre != null)
            {
                simbolo += this.getSimbolos();
                simbolo += this.padre.getSimbolos();
            }
            else {
                simbolo += this.getSimbolos();
            }
            simbolo += "</tbody> \n</table>\n </body>\n</html>";
            using (StreamWriter outputFile = new StreamWriter("C:/compiladores2/TS_"+entorno+".html"))
            {
                outputFile.WriteLine(simbolo);
            }
        }

        public String getSimbolos() 
        {
            String simbolo = "";
            if (this.variables.Count > 0)
            {
                for (int i = 0; i < this.variables.Count; i++)
                {
                    simbolo += "<tr>" + "\n" +
                            "<td class=\"text-left\">" + this.variables.ElementAt(i).Value.Id +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + this.variables.ElementAt(i).Value.Tipo.tipoAuxiliar +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + this.variables.ElementAt(i).Value.Linea +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + this.variables.ElementAt(i).Value.Columna +
                            "</td>" + "\n" +
                             "<td class=\"text-left\">" + this.alias +
                            "</td>" + "\n";
                }
            }
            if (this.funciones.Count > 0)
            {
                for (int i = 0; i < this.funciones.Count; i++)
                {
                    simbolo += "<tr>" +
                            "<td class=\"text-left\">" + this.funciones.ElementAt(i).Value.Id +
                            "</td>" + "\n";
                            if (this.funciones.ElementAt(i).Value.Tipo == null)
                            {
                                simbolo += "<td class=\"text-left\">" + "PROC::null" +
                                            "</td>" + "\n" ;
                            }
                            else 
                            {
                                simbolo += "<td class=\"text-left\">" + "FUNCT::"+this.funciones.ElementAt(i).Value.Tipo.tipoAuxiliar +
                                "</td>" + "\n";
                            }
                            simbolo +="" +
                            "<td class=\"text-left\">" + this.funciones.ElementAt(i).Value.Linea +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + this.funciones.ElementAt(i).Value.Columna +
                            "</td>" + "\n";
                    if (this.padre == null)
                    {
                        simbolo += "<td class=\"text-left\">" + this.alias +
                           "</td>" + "\n" +
                           "</tr> \n";
                    }
                    else
                    {
                        simbolo += "<td class=\"text-left\">" + this.padre.alias +
                               "</td>" + "\n" +
                               "</tr> \n";
                    }
                }
            }
            if (this.types.Count > 0)
            {
                for (int i = 0; i < this.types.Count; i++)
                {
                    simbolo += "<tr>" +
                            "<td class=\"text-left\">" + this.types.ElementAt(i).Value.Id +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + "object" +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + this.types.ElementAt(i).Value.Linea +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + this.types.ElementAt(i).Value.Columna +
                            "</td>" + "\n";
                    if (this.padre == null)
                    {
                        simbolo += "<td class=\"text-left\">" + this.alias +
                           "</td>" + "\n" +
                           "</tr> \n";
                    }
                    else
                    {
                        simbolo += "<td class=\"text-left\">" + this.padre.alias +
                               "</td>" + "\n" +
                               "</tr> \n";
                    }
                }
            }
            if (this.arreglos.Count > 0) 
            {
                for (int i = 0; i < this.arreglos.Count; i++)
                {
                    simbolo += "<tr>" +
                            "<td class=\"text-left\">" + this.arreglos.ElementAt(i).Value.Id +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + "array" +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + this.arreglos.ElementAt(i).Value.Linea +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + this.arreglos.ElementAt(i).Value.Columna +
                            "</td>" + "\n";
                    if (this.padre == null)
                    {
                        simbolo += "<td class=\"text-left\">" + this.alias +
                           "</td>" + "\n" +
                           "</tr> \n";
                    }
                    else
                    {
                        simbolo += "<td class=\"text-left\">" + this.padre.alias +
                               "</td>" + "\n" +
                               "</tr> \n";
                    }
                }

            }
            return simbolo;
        }

    }
}
