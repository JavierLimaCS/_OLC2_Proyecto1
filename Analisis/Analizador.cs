using Irony.Parsing;
using Proyecto1.Analisis;
using Proyecto1.Interprete.Expresion;
using Proyecto1.Interprete.Instruccion;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Proyecto1.Analisis
{
    class Analizador
    {
        public string consola = "";
        public LinkedList<Error> lista_errores = new LinkedList<Error>();
        TabladeSimbolos global = new TabladeSimbolos();
        public void Analizar(String cadena)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            foreach (var item  in lenguaje.Errors)
            {
                System.Diagnostics.Debug.WriteLine(item);
            }

            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;
            if (raiz == null)
            {
                foreach (var er in arbol.ParserMessages) 
                {
                    if (er.Message.Contains("Invalid character"))
                    {
                        lista_errores.AddLast(new Error("Léxico", er.Message, er.Location.Line + 1, er.Location.Column  + 1));
                    }
                    else 
                    {
                        lista_errores.AddLast(new Error("Sintáctico", er.Message, er.Location.Line + 1, er.Location.Column + 1));
                    }
                }
                consola = "Hay " + lista_errores.Count +" errores en el archivo de entrada, revise reporte de errores \n";
                crearReporteErrores();
                return;
            }
            crearReporteErrores();
            if (BuscarAnidadas(raiz))
            {
                consola = "No puede ejecutarse el archivo de entrada porque existen funciones anidadas\n" +
                          "Por favor, traduzca antes de ejecutar el archivo.";
                return;
            }
            LinkedList<Instruccion> listaInstrucciones = instrucciones(raiz.ChildNodes[1]);
            LinkedList<Instruccion> listaSentencias = instrucciones(raiz.ChildNodes[3]);
            this.generarGrafo(raiz);
            
        }

        public Boolean BuscarAnidadas(ParseTreeNode raiz)
        {
            String no_terminal;
            foreach (var nodo in raiz.ChildNodes[1].ChildNodes)
            {
                no_terminal = nodo.Term.Name;
                switch (no_terminal)
                {
                    case "Funcion":
                        foreach (var funcionAnidada in nodo.ChildNodes[3].ChildNodes) 
                        {
                            if(funcionAnidada.Term.Name == "Funcion") return true;
                        }
                        return false;
                }
            }
            return false;
        }

        public void ejecutar(LinkedList<Instruccion> instrucciones, TabladeSimbolos ts)
        {
            foreach (var instruccion in instrucciones)
            {
                instruccion.Ejecutar(ts, "global");
            }
        }
        public LinkedList<Instruccion> instrucciones(ParseTreeNode actual)
        {
            LinkedList<Instruccion> listaInstrucciones = new LinkedList<Instruccion>();
            String instruccion = "";
            //int indice = 0;
            foreach (ParseTreeNode nodo in actual.ChildNodes)
            {
                //instruccion = nodo.ChildNodes[indice].Term.Name;
                instruccion = nodo.Term.Name;
                switch (instruccion) 
                {
                    case "Declaraciones":
                        //listaInstrucciones.AddLast(new Declaracion(nodo.ChildNodes))
                        System.Diagnostics.Debug.WriteLine("SII");
                        break;
                }
                System.Diagnostics.Debug.WriteLine(instruccion);
            }
            return listaInstrucciones;
        }

        public void generarGrafo(ParseTreeNode raiz)
        {
            string grafoDot = AST.AST.getDot(raiz);
            string path = "C:/compiladores2/ast.txt";
            try
            {
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(grafoDot);
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            this.generarReporteAST();
        }

        public void generarReporteAST() 
        {
            try 
            {
                string comando = "dot -Tpng C:/compiladores2/ast.txt -o C:/compiladores2/ast_report.png";
                var procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + comando);
                var proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public void crearReporteErrores() 
        {
            String errores = "<html>\n <body> <h2>Reporte de Errores</h2> <table style=\"width:100%\" border=\"1\"> <tr><th>Tipo</th><th>Descripcion del error</th><th>Linea</th> <th>Columna</th></tr> \n";
            for(int i = 0; i < this.lista_errores.Count; i++) 
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
            using (StreamWriter outputFile = new StreamWriter("C:/compiladores2/reporteErrores.html"))
            {
                outputFile.WriteLine(errores);
            }
        }

    }
}
