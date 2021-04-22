using Irony.Parsing;
using Proyecto1.Codigo3D;
using Proyecto1.Interprete.Instruccion;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Proyecto1.Analisis
{
    class Generador
    {
        public string consola = "";
        public LinkedList<Error> lista_errores = new LinkedList<Error>();
        private List<Object> salida = new List<Object>();
        TabladeSimbolos global = new TabladeSimbolos(null, "Global"); //Entorno Global
        Intermedio codeigointer = new Intermedio();
        RichTextBox rt;
        public void generar(String cadena, RichTextBox rl)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            rt = rl;
            rt.Text = "";
            foreach (var item in lenguaje.Errors)
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
                        lista_errores.AddLast(new Error("Léxico", er.Message, er.Location.Line + 1, er.Location.Column + 1));
                    }
                    else
                    {
                        lista_errores.AddLast(new Error("Sintáctico", er.Message, er.Location.Line + 1, er.Location.Column + 1));
                    }
                }
                consola = "Hay " + lista_errores.Count + " errores en el archivo de entrada, revise reporte de errores \n";
                //crearReporteErrores();
                rt.Text = consola;
                return;
            }
            if (BuscarAnidadas(raiz))
            {
                consola = "No puede ejecutarse el archivo de entrada porque existen funciones anidadas\n" +
                          "Por favor, traduzca antes de ejecutar el archivo.";
                return;
            }
            Analizador an = new Analizador(rt);
            LinkedList<Instruccion> listaInstrucciones = an.instrucciones(raiz.ChildNodes[1]);
            LinkedList<Instruccion> listaSentencias = an.instrucciones(raiz.ChildNodes[3]);
            this.primeraPasada(listaInstrucciones, this.global);
            this.generarCodigoIntermedio(listaInstrucciones, listaSentencias, this.global, this.codeigointer);
            this.global.generarTS(this.global.alias);
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
                            if (funcionAnidada.Term.Name == "Funcion") return true;
                        }
                        return false;
                }
            }
            return false;
        }

        public void generarCodigoIntermedio(LinkedList<Instruccion> instrucciones, LinkedList<Instruccion> sentencias, TabladeSimbolos ts, Intermedio inter)
        {
            string code3d = "";
            string cuerpo = "";
            string encabezado = "#include <stdio.h> \n" +
                            "float Heap[100000]; //estructura heap\n" +
                            "float Stack[100000]; //estructura stack\n" +
                            "float SP; //puntero Stack pointer\n" +
                            "float HP; //puntero Heap pointer\n"; 
            encabezado += "float ";
            cuerpo += "//-----Codigo Intermedio\nvoid main() { \n \n";
            foreach (var instruccion in instrucciones)
            {
                if (instruccion != null)
                {
                    if (!(instruccion is Funcion)) 
                    {
                        if (!(instruccion is Procedimiento)) cuerpo += instruccion.generar3D(ts, inter);
                    }
                }

            }
            foreach (var sentencia in sentencias)
            {
                if (sentencia != null)
                {
                    cuerpo += sentencia.generar3D(ts, inter);
                }

            }
            cuerpo += "    return; \n" +
                "} \n \n";


            foreach (var instruccion in instrucciones)
            {
                if (instruccion != null)
                {
                    if (instruccion is Funcion || instruccion is Procedimiento)
                    {
                        cuerpo += instruccion.generar3D(ts, inter);
                    }
                }

            }
            string tmps = "";
            foreach (var tmp in inter.tmp.temporales)
            {
                tmps += tmp + ",";
            }
            tmps = tmps.TrimEnd(',');
            encabezado += tmps + "; \n";
            string funciones = "";
            foreach (var funcion in ts.funciones) {
                funciones += "void " + funcion.Key + "(); \n";
            }
            encabezado += "\n//Encabezado de Funciones \n" + funciones + "\n";
            code3d = encabezado + cuerpo;
            rt.Text = code3d;
        }

        public void primeraPasada(LinkedList<Instruccion> instruccions, TabladeSimbolos ts)
        {
            Object output;
            foreach (var instruccion in instruccions)
            {
                if (instruccion != null)
                {
                    if (instruccion is Funcion) output = instruccion.Ejecutar(ts);
                    else if (instruccion is Procedimiento) output = instruccion.Ejecutar(ts);
                    else if (instruccion is Declaraciones) output = instruccion.Ejecutar(ts);
                    else if (instruccion is DeclaArreglo) output = instruccion.Ejecutar(ts);
                }

            }
        }
        public void crearReporteErrores()
        {
            String errores = "<html>\n <body> <h2>Reporte de Errores</h2> <table style=\"width:100%\" border=\"1\"> <tr><th>Tipo</th><th>Descripcion del error</th><th>Linea</th> <th>Columna</th></tr> \n";
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
            using (StreamWriter outputFile = new StreamWriter("C:/compiladores2/reporteErrores.html"))
            {
                outputFile.WriteLine(errores);
            }
        }
    }
}
