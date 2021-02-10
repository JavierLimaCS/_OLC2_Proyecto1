using Irony.Parsing;
using Proyecto1.Analisis;
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
        public void Analizar(String cadena)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            foreach (var item  in lenguaje.Errors)
            {
                Console.WriteLine(item);
            }

            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;
            if (raiz == null)
            {
                foreach (var er in arbol.ParserMessages) 
                {
                    lista_errores.AddFirst(new Error("Semantico", er.Message, er.Location.Line + 1, er.Location.Column));
                }
                consola = "Hay " + lista_errores.Count +" errores en el archivo de entrada, revise reporte de errores \n";
                foreach (var elemento in this.lista_errores) 
                {
                    consola = "Desc: "  + elemento.Descripcion + " Linea: "  + elemento.Linea + "\n";
                }
                return;
            }
            consola = "Exito ALV";
            this.generarGrafo(raiz);
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
                string comando = "dot -Tpng C:/compiladores2/ast.txt -o C:/compiladores2/reporte_ast.png";
                var procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + comando);
                var proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


    }
}
