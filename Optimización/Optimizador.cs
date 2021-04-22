using Irony.Parsing;
using Proyecto1.Analisis;
using Proyecto2.Optimización.Reglas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Proyecto2.Optimización
{
    class Optimizador
    {
        public string codigo;
        public LinkedList<Error> lista_errores;
        public List<Regla> optimizaciones;
        public Optimizador(string code)
        {
            this.codigo = code;
            this.lista_errores = new LinkedList<Error>();
            this.optimizaciones = new List<Regla>();
        }

        public void Optimizacion()
        {
            Gramatica3D grammar = new Gramatica3D();
            LanguageData lenguaje = new LanguageData(grammar);
            foreach (var item in lenguaje.Errors)
            {
                System.Diagnostics.Debug.WriteLine(item);
            }
            string[] code = this.codigo.Split("Intermedio");
            this.codigo = code[1];
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(this.codigo);
            ParseTreeNode raiz = arbol.Root;
            if (raiz == null)
            {
                foreach (var er in arbol.ParserMessages)
                {
                    if (er.Message.Contains("Invalid character"))
                    {
                        lista_errores.AddLast(new Error("Léxico", er.Message, er.Location.Line + 1, er.Location.Column + 1));
                    }
                    else
                    {
                        lista_errores.AddLast(new Error("Sintáctico", er.Message, er.Location.Line + 1, er.Location.Column + 1));
                    }
                }
            }
            LinkedList<Instruccion3D> instrucciones_3d = instrucciones(raiz.ChildNodes[2]);
            this.optimizar(instrucciones_3d);
            this.generar_Reporte_Opt();
        }

        public void optimizar(LinkedList<Instruccion3D> instrucciones) 
        {
            string newcode = "";
            foreach (var instruccion in instrucciones)
            {
                if (instruccion != null)
                {
                    newcode += instruccion.optimizar3d();
                    this.optimizaciones.AddRange(instruccion.Optimizaciones);
                }
            }

        }

        public LinkedList<Instruccion3D> instrucciones(ParseTreeNode actual)
        {
            LinkedList<Instruccion3D> listaInstrucciones = new LinkedList<Instruccion3D>();
            foreach (ParseTreeNode nodo in actual.ChildNodes)
            {
                listaInstrucciones.AddLast(instruccion(nodo));
            }
            return listaInstrucciones;
        }

        public Instruccion3D instruccion(ParseTreeNode actual)
        {
            switch (actual.Term.Name.ToLower())
            {
                case "asignacion":
                    string id = "";
                    char ty = 'a';
                    id = actual.ChildNodes[0].Token.Text;
                    int l = actual.ChildNodes[0].Token.Location.Line + 1;
                    if (actual.ChildNodes[0].Term.Name == "tmp") ty = 't';
                    if (actual.ChildNodes[0].Term.Name == "exp") {
                        if (actual.ChildNodes[0].Term.Name == "tmp") ty = 't';
                    }
                    return new Asignacion3D(id, ty, (Expresion3D)instruccion(actual.ChildNodes[1]),l);
                case "expresion":
                    string izq = actual.ChildNodes[0].ChildNodes[0].Token.Text;
                    string der = actual.ChildNodes[2].ChildNodes[0].Token.Text;
                    string oper = actual.ChildNodes[1].Token.Text;
                    return new Expresion3D(izq, der, oper);
            }
            return null;
        }

        public void generar_Reporte_Opt()
        {
            String simbolo = "<html>\n <head>" +
                "<meta charset=\"utf - 8\"/>" +
                "<title> Reporte de Optimizacion</title>" +
                "<meta name=\"viewport\" content=\"initial-scale=1.0; maximum-scale=1.0; width=device-width;\">" +
                "<link rel=\"stylesheet\" href=\"style.css\">\n" +
                "</head>\n" +
                "<body>" + "\n" +
                "<div class=\"table-title\">" +
                "<h2 style=\"text-align:center;\">Reporte de Optimizacion</h2>\n" +
                "</div> \n" +
                "<table class=\"table-fill\"> " + "\n" +
                "<thead>\n" +
                "<tr><th class=\"text-left\">TIPO OPT.</th>" + "\n" +
                "    <th class=\"text-left\">NO. REGLA</th>" + "\n" +
                "    <th class=\"text-left\">CODIGO ELIMINADO</th> " + "\n" +
                "    <th class=\"text-left\">CODIGO AGREGADO</th>" + "\n" +
                "    <th class=\"text-left\">FILA</th>" + "\n" +
                "</tr> \n</thead>\n <tbody class=\"table-hover\"> \n";
            foreach(var regla in this.optimizaciones)
            {
                simbolo += "<tr>" + "\n" +
                            "<td class=\"text-left\">" + regla.Tipo +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + regla._Regla +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + regla.Codigo_anterior +
                            "</td>" + "\n" +
                            "<td class=\"text-left\">" + regla.Codigo_Actual +
                            "</td>" + "\n" +
                             "<td class=\"text-left\">" + regla.Fila +
                            "</td>" + "\n";
            }
            simbolo += "</tbody> \n</table>\n </body>\n</html>";
            using (StreamWriter outputFile = new StreamWriter("C:/compiladores2/rep_optimizacion.html"))
            {
                outputFile.WriteLine(simbolo);
            }
        }
    }
}
