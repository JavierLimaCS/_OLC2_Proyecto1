using Irony.Parsing;
using Proyecto1.Analisis;
using Proyecto2.Optimización.Reglas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Proyecto2.Optimización
{
    class Optimizador
    {
        public string codigo;
        string encabezado;
        int contador = 0;
        public LinkedList<Error> lista_errores;
        public List<ReglaM> optimizaciones;
        public List<string> optimizado;
        public Dictionary<string, int> Etiquetas = new Dictionary<string, int>();
        public Dictionary<string, int> Saltos = new Dictionary<string, int>();
        RichTextBox rt_;
        public Optimizador(string code, RichTextBox rt, int cont)
        {
            this.codigo = code;
            this.rt_ = rt;
            this.contador = cont;
            this.lista_errores = new LinkedList<Error>();
            this.optimizaciones = new List<ReglaM>();
            this.optimizado = new List<string>();
        }

        public void Optimizacion()
        {
            if (!(this.codigo.Equals("")) && this.codigo.Contains("#include <stdio.h>"))
            {
                Gramatica3D grammar = new Gramatica3D();
                LanguageData lenguaje = new LanguageData(grammar);
                foreach (var item in lenguaje.Errors)
                {
                    System.Diagnostics.Debug.WriteLine(item);
                }
                string[] code = this.codigo.Split("Intermedio");
                this.encabezado = code[0];
                this.codigo = code[1];
                foreach (string i in this.codigo.Split("\n")) 
                {
                    optimizado.Add(i);
                }
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
                    MessageBox.Show("Codigo Intermedio contiene errores, revise reporte", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.crearReporteErrores();
                }
                else { 
                    List<Instruccion3D> instrucciones_3d = instrucciones(raiz.ChildNodes[2]);
                    if (raiz.ChildNodes[3].ChildNodes.Count > 0) 
                    {
                        foreach (var funcion in raiz.ChildNodes[3].ChildNodes)
                        {
                            instrucciones_3d.AddRange(instrucciones(funcion.ChildNodes[2]));
                        }
                    }
                    this.optimizar(instrucciones_3d);
                    this.generar_Reporte_Opt();
                    this.crearReporteErrores();
                    this.codigo = "";
                    this.codigo += this.encabezado + "Intermedio";
                    for (int i = 0; i <= this.optimizado.Count; i++) 
                    {
                        foreach (var j in this.optimizaciones) 
                        {
                            if (i == j.Fila) 
                            {
                                string opt = this.optimizado.ElementAt(i);
                                this.optimizado[this.optimizado.FindIndex(ind => ind.Equals(opt))] = j.Codigo_Actual;
                            }
                        }
                    }
                    foreach (string i in this.optimizado)
                    {
                        this.codigo += i + "\n";
                    }
                    string cantidad = "";
                    if(this.contador == 1)
                        cantidad += "// Intermedio optimizado " + contador + " vez";
                    else
                        cantidad += "// Intermedio optimizado  " + contador + " veces";
                    this.rt_.Text = this.codigo;
                    this.rt_.Text += cantidad;
                    MessageBox.Show("Código Intermedio Optimizado Correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Codigo Intermedio incorrecto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.crearReporteErrores();
            }
        }

        public void optimizar(List<Instruccion3D> instrucciones) 
        {
            string newcode = "";
            foreach (var instruccion in instrucciones)
            {
                if (instruccion is Etiqueta || instruccion is Salto3D)
                {
                    newcode += instruccion.optimizar3d(Etiquetas,Saltos);
                    this.optimizaciones.AddRange(instruccion.Optimizaciones);
                }
            }
            foreach (var instruccion in instrucciones)
            {
                if (instruccion != null)
                {
                    newcode += instruccion.optimizar3d(Etiquetas, Saltos);
                    this.optimizaciones.AddRange(instruccion.Optimizaciones);
                }
            }

        }

        public List<Instruccion3D> instrucciones(ParseTreeNode actual)
        {
            List<Instruccion3D> listaInstrucciones = new List<Instruccion3D>();
            foreach (ParseTreeNode nodo in actual.ChildNodes)
            {
                listaInstrucciones.Add(instruccion(nodo));
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
                    int l = actual.ChildNodes[0].Token.Location.Line;
                    if (id.Equals("Heap") || id.Equals("Stack")) 
                    {
                        return new Asignacion3D();
                    }
                    if (actual.ChildNodes[0].Term.Name == "tmp") ty = 't';
                    if (actual.ChildNodes[0].Term.Name == "exp") {
                        if (actual.ChildNodes[0].Term.Name == "tmp") ty = 't';
                    }
                    return new Asignacion3D(id, ty, (Expresion3D)instruccion(actual.ChildNodes[1]),l);
                case "expresion":
                    if (actual.ChildNodes.Count == 3)
                    {
                        string izq = actual.ChildNodes[0].ChildNodes[0].Token.Text;
                        string der = actual.ChildNodes[2].ChildNodes[0].Token.Text;
                        string oper = actual.ChildNodes[1].Token.Text;
                        return new Expresion3D(izq, der, oper);
                    }
                    else if (actual.ChildNodes.Count>3) 
                    {
                        return new Expresion3D();
                    }
                    else
                    {
                        string izq = actual.ChildNodes[0].Token.Text;
                        return new Expresion3D(izq);
                    }
                case "salto":
                    string etiqueta = actual.ChildNodes[1].Token.Text;
                    int f = actual.ChildNodes[1].Token.Location.Line;
                    return new Salto3D(etiqueta,f);
                case "condicional":
                    string g1 = actual.ChildNodes[2].ChildNodes[1].Token.Text;
                    int linea = actual.ChildNodes[0].Token.Location.Line;
                    return new Condicional3D(g1,(Expresion3D)instruccion(actual.ChildNodes[1]), linea);
                case "etiqueta":
                    string label = actual.ChildNodes[0].Token.Text;
                    int line = actual.ChildNodes[0].Token.Location.Line;
                    return new Etiqueta(label, line);
            }
            return null;
        }

        public void crearReporteErrores()
        {
            String errores = "<html>\n <head>" +
                "<meta charset=\"utf - 8\"/>" +
                "<title> Reporte de Errores en codigo 3D</title>" +
                "<meta name=\"viewport\" content=\"initial-scale=1.0; maximum-scale=1.0; width=device-width;\">" +
                "<link rel=\"stylesheet\" href=\"style2.css\">\n" +
                "</head>\n" +
                "<body>" + "\n" +
                "<div class=\"table-title\">" +
                "<h2 style=\"text-align:center;\">Reporte de Errores en Codigo Intermedio</h2>\n" +
                "</div> \n" +
                "<table class=\"table-fill\"> " + "\n" +
                "<thead>\n" +
                "<tr><th class=\"text-left\">TIPO</th>" + "\n" +
                "    <th class=\"text-left\">DESCRIPCION</th>" + "\n" +
                "    <th class=\"text-left\">FILA</th> " + "\n" +
                "    <th class=\"text-left\">COLUMNA</th>" + "\n" +
                "</tr> \n</thead>\n <tbody class=\"table-hover\"> \n";
            for (int i = 0; i < this.lista_errores.Count; i++)
            {
                errores += "<tr>" +
                        "<td>" + this.lista_errores.ElementAt(i).Tipo +
                        "</td>" +
                        "<td>" + this.lista_errores.ElementAt(i).Descripcion +
                        "</td>" +
                        "<td>" + this.lista_errores.ElementAt(i).Linea +
                        "</td>" +
                        "<td>" + this.lista_errores.ElementAt(i).Columna +
                        "</td>" +
                        "</tr>";
            }
            errores += "</table> </body> </html>";
            using (StreamWriter outputFile = new StreamWriter("C:/compiladores2/reporteErrores_OPT.html"))
            {
                outputFile.WriteLine(errores);
            }
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
