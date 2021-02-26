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
        String alias;
        Dictionary<string, Simbolo> variables;
        Dictionary<string, Simbolo_Funcion> funciones;
        Dictionary<string, object> types;
        public TabladeSimbolos(TabladeSimbolos padre, String alias)
        {
            this.padre = padre;
            this.alias = alias;
            this.variables = new Dictionary<string, Simbolo>();
            this.funciones = new Dictionary<string, Simbolo_Funcion>();
            this.types = new Dictionary<string, object>();
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


        public void generarTS()
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
                simbolo += getSimbolos();
                simbolo += this.padre.getSimbolos();
            }
            else {
                simbolo += getSimbolos();
            }
            simbolo += "</tbody> \n</table>\n </body>\n</html>";
            using (StreamWriter outputFile = new StreamWriter("C:/compiladores2/TS.html"))
            {
                outputFile.WriteLine(simbolo);
            }
        }

        public String getSimbolos() 
        {
            String simbolo = "";
            if (this.variables.Count > 0)
            {
                //simbolo += "<tr> <th colspan = \"5\" class=\"text-left\"><p style=\"text-align:center;\">VARIABLES</p></th></tr>";
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
                            "</td>" + "\n";
                    if (this.padre == null)
                    {
                        simbolo += "<td class=\"text-left\">" + "Global" +
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
            if (this.funciones.Count > 0)
            {
                //simbolo += "<tr> <th colspan = \"5\"> <p style=\"text-align:center;\">FUNCIONES</p></th></tr>";
                for (int i = 0; i < this.variables.Count; i++)
                {
                    simbolo += "<tr>" +
                            "<td class=\"text-left\">" + this.funciones.ElementAt(i).Value.Id +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + this.funciones.ElementAt(i).Value.Tipo.tipoAuxiliar +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + this.funciones.ElementAt(i).Value.Linea +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + this.funciones.ElementAt(i).Value.Columna +
                            "</td>" + "\n";
                    if (this.padre == null)
                    {
                        simbolo += "<td class=\"text-left\">" + "Global" +
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
