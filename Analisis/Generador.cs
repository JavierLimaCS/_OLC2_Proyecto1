using Irony.Parsing;
using Proyecto1.Codigo3D;
using Proyecto1.Interprete.Instruccion;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
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
                return;
            }
            if (BuscarAnidadas(raiz))
            {
                consola = "No puede ejecutarse el archivo de entrada porque existen funciones anidadas\n" +
                          "Por favor, traduzca antes de ejecutar el archivo.";
                return;
            }
            Analizador an = new Analizador();
            LinkedList<Instruccion> listaInstrucciones = an.instrucciones(raiz.ChildNodes[1]);
            LinkedList<Instruccion> listaSentencias = an.instrucciones(raiz.ChildNodes[3]);
            this.generarCodigoIntermedio(listaInstrucciones, listaSentencias, this.global, this.codeigointer);
            //this.generarGrafo(raiz);
            this.global.generarTS(this.global.alias);
            //crearReporteErrores();
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
            String output = "#include <stdio.h> \n" +
                            "int Heap[100000]; \n" + //estructura heap
                            "int Stack[100000]; \n" + //estructura stack
                            "int SP; \n"+ //puntero Stack pointer
                            "int HP; \n \n"; //puntero Heap pointer
            
            foreach (var instruccion in instrucciones)
            {
                if (instruccion != null)
                {
                    output += instruccion.generar3D(ts, inter);
                }

            }
            output += "\nvoid main() { \n";
            foreach (var sentencia in sentencias)
            {
                if (sentencia != null)
                {
                    output += sentencia.generar3D(ts, inter);
                }

            }
            output += "}";
            rt.Text = output;
        }
    }
}
