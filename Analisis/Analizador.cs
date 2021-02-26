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
        private List<Object> salida = new List<Object>();
        TabladeSimbolos global = new TabladeSimbolos(null, "Global"); //Entorno Global
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
            //this.ejecutar(listaInstrucciones, global);
            //this.ejecutar(listaSentencias, global);
            this.generarGrafo(raiz);
            for (int i = 0; i < salida.Count; i++) 
            {
                if (salida.ElementAt(i) is String) 
                {
                    consola += salida.ElementAt(i).ToString();
                }
            }
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
            Object output;
            foreach (var instruccion in instrucciones)
            {
                if (instruccion != null) 
                {
                    output = instruccion.Ejecutar(ts);
                    if (output is List<Object>) 
                    {
                        this.salida.AddRange((List<Object>)output); 
                    }
                    else
                    {
                        this.salida.Add(output);
                    }
                }
                
            }
        }
        public Instruccion instruccion(ParseTreeNode actual)
        {
            switch (actual.Term.Name.ToLower())
            {
                case "writeln":
                    LinkedList<Expresion> exp_list = new LinkedList<Expresion>();
                    foreach (var exp in actual.ChildNodes[1].ChildNodes) 
                    {
                            exp_list.AddLast(expresion(exp));
                    }
                    return new Writeln(exp_list);
                case "write":
                    LinkedList<Expresion> exp_list2 = new LinkedList<Expresion>();
                    foreach (var exp in actual.ChildNodes[1].ChildNodes)
                    {
                        exp_list2.AddLast(expresion(exp.ChildNodes[0]));
                    }
                    return new Write(exp_list2);
                case "declaracion":
                    LinkedList<Declaracion> lista_decla = new LinkedList<Declaracion>();
                    foreach (var decla in actual.ChildNodes) 
                    {
                        String tipo = decla.ChildNodes[1].ChildNodes[0].Token.Text;
                        String id = "";
                        int l = decla.ChildNodes[0].ChildNodes[0].Token.Location.Line + 1;
                        int c = decla.ChildNodes[0].ChildNodes[0].Token.Location.Column + 1;
                        List<String> vars = new List<string>();
                        Expresion valor = null;
                        if (decla.ChildNodes[2].ChildNodes.Count > 0)
                        {
                            valor = expresion(decla.ChildNodes[2].ChildNodes[0]);
                        }
                        foreach (var variable in decla.ChildNodes[0].ChildNodes)
                        {
                            id = variable.Token.Text;
                            vars.Add(id);
                        }
                        Tipos new_type = Tipos.ERROR;
                        switch (tipo) 
                        {
                            case "integer":
                                new_type = Tipos.INT;
                                break;
                            case "decimal":
                                new_type = Tipos.REAL;
                                break;
                            case "string":
                                new_type = Tipos.STRING;
                                break;
                            case "boolean":
                                new_type = Tipos.BOOLEAN;
                                break;
                        }
                        lista_decla.AddLast(new Declaracion(new Tipo(new_type, tipo), vars, valor, l, c));
                    }
                    return new Declaraciones(lista_decla);
                case "asignacion":
                    return new Asignacion(actual.ChildNodes[0].Token.Text, expresion(actual.ChildNodes[1]));
                case "llamada":
                    if (actual.ChildNodes.Count == 1)
                    {
                        return new Llamada(actual.ChildNodes[0].Token.Text, null);
                    }
                    else 
                    {
                        List<Expresion> list = new List<Expresion>();
                        foreach (var exp in actual.ChildNodes[1].ChildNodes) 
                        {
                            list.Add(expresion(exp));
                        }
                        return new Llamada(actual.ChildNodes[0].Token.Text, list);
                    }
                case "while":
                    return new While(expresion(actual.ChildNodes[1]), instrucciones(actual.ChildNodes[4])); ; 
                case "if":
                    if (actual.ChildNodes.Count == 6)
                    {
                        if (actual.ChildNodes[4].ChildNodes.Count > 0)
                        {
                            return new If(expresion(actual.ChildNodes[1]), instrucciones(actual.ChildNodes[4]), instruccion(actual.ChildNodes[4]));
                        }
                        else
                        {
                            return new If(expresion(actual.ChildNodes[1]), instrucciones(actual.ChildNodes[4]), null);
                        }
                    }
                    else 
                    {
                        if (actual.ChildNodes[4].ChildNodes.Count > 0)
                        {
                            return new If(expresion(actual.ChildNodes[1]), instrucciones(actual.ChildNodes[3]), instruccion(actual.ChildNodes[4]));
                        }
                        else
                        {
                            return new If(expresion(actual.ChildNodes[1]), instrucciones(actual.ChildNodes[3]), null);
                        }
                    }
                case "else":
                    if (actual.ChildNodes.Count == 1)
                    {
                        return new Else(instruccion(actual.ChildNodes[1]));
                    }
                    else
                    {
                        return new Else(instrucciones(actual.ChildNodes[2]));
                    }
            }
            return null;
        }

        public Expresion expresion(ParseTreeNode actual)
        {
            if (actual.ChildNodes.Count == 3)
            {
                string operador = actual.ChildNodes[1].Token.Text;
                switch (operador)
                {
                    case "+":
                        return new Aritmetica(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), '+');
                    case "-":
                        return new Aritmetica(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), '-');
                    case "*":
                        return new Aritmetica(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), '*');
                    case "/":
                        return new Aritmetica(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), '/');
                    case "=":
                        return new Relacional(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), '=');
                    case "<>":
                        return new Relacional(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), '!');
                    case ">":
                        return new Relacional(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), '>');
                    case "<":
                        return new Relacional(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), '<');
                    case ">=":
                        return new Relacional(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), 'm');
                    case "<=":
                        return new Relacional(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), 'i');
                    default:
                        return new Aritmetica(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), '%');
                }
            }
            else
            {
                String tipo = actual.ChildNodes[0].Term.Name;
                tipo = tipo.ToLower();
                switch(tipo) 
                {
                    case "entero":
                        return new Primitivo('N', actual.ChildNodes[0].Token.Text);
                    case "cadena":
                        return new Primitivo('S', actual.ChildNodes[0].Token.Text);
                    case "decimal":
                        return new Primitivo('R', actual.ChildNodes[0].Token.Text);
                    case "id":
                        return new Primitivo('I', actual.ChildNodes[0].Token.Text);
                    case "true":
                    case "false":
                        return new Primitivo('B', actual.ChildNodes[0].Token.Text);
                }
                return null;
                
            }
        }
        public LinkedList<Instruccion> instrucciones(ParseTreeNode actual)
        {
            LinkedList<Instruccion> listaInstrucciones = new LinkedList<Instruccion>();
            foreach (ParseTreeNode nodo in actual.ChildNodes)
            {
                listaInstrucciones.AddLast(instruccion(nodo));
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
