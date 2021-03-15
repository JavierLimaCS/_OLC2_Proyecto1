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
using System.Windows.Forms;

namespace Proyecto1.Analisis
{
    class Analizador
    {
        public string consola = "";
        public LinkedList<Error> lista_errores = new LinkedList<Error>();
        private List<Object> salida = new List<Object>();
        TabladeSimbolos global = new TabladeSimbolos(null, "Global"); //Entorno Global
        RichTextBox rt;
        public void Analizar(String cadena, RichTextBox rl)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            rt = rl;
            rt.Text = "";
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
            if (BuscarAnidadas(raiz))
            {
                consola = "No puede ejecutarse el archivo de entrada porque existen funciones anidadas\n" +
                          "Por favor, traduzca antes de ejecutar el archivo.";
                return;
            }
            LinkedList<Instruccion> listaInstrucciones = instrucciones(raiz.ChildNodes[1]);
            LinkedList<Instruccion> listaSentencias = instrucciones(raiz.ChildNodes[3]);
            this.ejecutar(listaInstrucciones, global);
            this.ejecutar(listaSentencias, global);
            this.generarGrafo(raiz);
            this.global.generarTS(this.global.alias);
            crearReporteErrores();
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
                    if (instruccion.Semanticos.Count > 0) 
                    {
                        foreach (var nodo in instruccion.Semanticos) 
                        {
                            this.lista_errores.AddLast(nodo);
                        }
                    }
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
                    return new Writeln(exp_list, rt);
                case "write":
                    LinkedList<Expresion> exp_list2 = new LinkedList<Expresion>();
                    foreach (var exp in actual.ChildNodes[1].ChildNodes)
                    {
                        exp_list2.AddLast(expresion(exp));
                    }
                    return new Write(exp_list2, rt);
                case "constantes":
                    LinkedList<Declaracion> lista_decla_const = new LinkedList<Declaracion>();
                    foreach (var decla in actual.ChildNodes)
                    {
                        String tipov = decla.ChildNodes[2].ChildNodes[0].Term.Name.ToLower();
                        String id = "";
                        Tipo tipo_var = getTipo(tipov);
                        int line = decla.ChildNodes[0].ChildNodes[0].Token.Location.Line + 1;
                        int col = decla.ChildNodes[0].ChildNodes[0].Token.Location.Column + 1;
                        List<String> vars = new List<string>();
                        Expresion valor = null;
                        if (decla.ChildNodes[2].ChildNodes.Count > 0)
                        {
                            valor = expresion(decla.ChildNodes[2]);
                        }
                        foreach (var variable in decla.ChildNodes[0].ChildNodes)
                        {
                            id = variable.Token.Text.ToLower();
                            vars.Add(id);
                        }

                        lista_decla_const.AddLast(new Declaracion(tipo_var, vars, valor, line, col, true));
                    }
                    return new Declaraciones(lista_decla_const);
                case "declaracion":
                    LinkedList<Declaracion> lista_decla = new LinkedList<Declaracion>();
                    foreach (var decla in actual.ChildNodes) 
                    {
                        String tipov = decla.ChildNodes[1].ChildNodes[0].Token.Text;
                        String id = "";
                        Tipo tipo_var = getTipo(tipov);
                        int line = decla.ChildNodes[0].ChildNodes[0].Token.Location.Line + 1;
                        int col = decla.ChildNodes[0].ChildNodes[0].Token.Location.Column + 1;
                        List<String> vars = new List<string>();
                        Expresion valor = null;
                        if (decla.ChildNodes[2].ChildNodes.Count > 0)
                        {
                            valor = expresion(decla.ChildNodes[2].ChildNodes[1]);
                        }
                        foreach (var variable in decla.ChildNodes[0].ChildNodes)
                        {
                            id = variable.Token.Text.ToLower();
                            vars.Add(id);
                        }
                        
                        lista_decla.AddLast(new Declaracion(tipo_var, vars, valor, line, col, false));
                    }
                    return new Declaraciones(lista_decla);
                case "funcion":
                    int contf = 2;
                    if (actual.ChildNodes.Count == 6) contf = 1;
                    String tipo = actual.ChildNodes[contf].ChildNodes[0].Token.Text;
                    Tipo tipo_funct = getTipo(tipo);
                    Simbolo_Funcion nuevafuncion = new Simbolo_Funcion(actual.ChildNodes[0].Token.Text.ToLower(), tipo_funct, actual.ChildNodes[0].Token.Location.Line + 1, actual.ChildNodes[0].Token.Location.Column + 1); ;
                    if (actual.ChildNodes.Count == 7)
                    {
                        foreach (var args in actual.ChildNodes[1].ChildNodes)
                        {
                            int indice = 0;
                            if (args.ChildNodes.Count > 2) indice = 1;
                            string tipoparam = args.ChildNodes[indice + 1].ChildNodes[0].Token.Text;
                            Tipo param_type = getTipo(tipoparam);
                            Parametro parametro;
                            int conta = 0;
                            foreach (var param in args.ChildNodes[indice].ChildNodes)
                            {
                                parametro = new Parametro(param.Token.Text, param_type);
                                nuevafuncion.Params.Add(param.Token.Text, parametro);
                                conta++;
                            }
                        }
                    }
                    nuevafuncion.Tipo = tipo_funct;
                    return new Funcion(nuevafuncion, instrucciones(actual.ChildNodes[contf+1]), instrucciones(actual.ChildNodes[contf+3]));

                case "procedimiento":
                    Simbolo_Funcion nuevo = new Simbolo_Funcion(actual.ChildNodes[0].Token.Text.ToLower(), null, actual.ChildNodes[0].Token.Location.Line+1, actual.ChildNodes[0].Token.Location.Column+1); ;
                    int proc_index_inst = 1;
                    int proc_index_sent = 3;
                    if (actual.ChildNodes.Count > 5)
                    {
                        proc_index_inst = 2;
                        proc_index_sent = 4;
                        foreach(var args in actual.ChildNodes[1].ChildNodes) 
                        {
                            int indice = 0;
                            if (args.ChildNodes.Count > 2) indice=1; 
                            string tipoparam = args.ChildNodes[indice+1].ChildNodes[0].Token.Text;
                            Tipo param_type = getTipo(tipoparam);
                            Parametro parametro;
                            int cont = 0;
                            foreach (var param in args.ChildNodes[indice].ChildNodes) 
                            {
                                parametro = new Parametro(param.Token.Text, param_type);
                                nuevo.Params.Add(param.Token.Text, parametro);
                                cont++;
                            }
                        }
                    }
                    return new Procedimiento(nuevo, instrucciones(actual.ChildNodes[proc_index_inst]), instrucciones(actual.ChildNodes[proc_index_sent]));
                case "asignacion":
                    string id_asigna = "";
                    List<string> acceso = new List<string>();
                    if (actual.ChildNodes[0].Term.Name.ToLower().Equals("accesoobjeto"))
                    {
                        int contacc = 0;
                        foreach (var accsObj in actual.ChildNodes[0].ChildNodes)
                        {
                            acceso.Add(accsObj.Token.Text.ToLower());
                            contacc++;
                        }
                        return new Asignacion(acceso, expresion(actual.ChildNodes[1]));
                    }
                    else if (actual.ChildNodes[0].Term.Name.ToLower().Equals("accesoarray")) 
                    {
                        id_asigna = actual.ChildNodes[0].ChildNodes[0].Token.Text.ToLower();
                        return new Asignacion(id_asigna, expresion(actual.ChildNodes[0].ChildNodes[2]), expresion(actual.ChildNodes[1]));
                    }
                    id_asigna = actual.ChildNodes[0].Token.Text.ToLower();
                    id_asigna = actual.ChildNodes[0].Token.Text.ToLower();
                    return new Asignacion(id_asigna, expresion(actual.ChildNodes[1]));
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
                    int indice_while = 0;
                    if (actual.ChildNodes[3].ChildNodes.Count == 3) indice_while = 1;
                    return new While(expresion(actual.ChildNodes[1]), instrucciones(actual.ChildNodes[3].ChildNodes[indice_while])); ; 
                case "if":
                    int contador_if = 0;
                    if (actual.ChildNodes.Count == 6)
                    {
                        Else elsito = null;
                        if (actual.ChildNodes[5].ChildNodes.Count > 0)
                        {
                            elsito = new Else(instrucciones(actual.ChildNodes[5]));
                            return new If(expresion(actual.ChildNodes[1]), instrucciones(actual.ChildNodes[3]), elsito);
                        }
                        else
                        {
                            return new If(expresion(actual.ChildNodes[1]), instrucciones(actual.ChildNodes[3]), elsito);
                        }
                    }
                    else 
                    {
                        Else elsito = null;
                        if (actual.ChildNodes[4].ChildNodes.Count < 1)
                        {

                            if (actual.ChildNodes[3].ChildNodes.Count > 1) contador_if = 1;
                            return new If(expresion(actual.ChildNodes[1]), instrucciones(actual.ChildNodes[3].ChildNodes[contador_if]), elsito);
                        }
                        else
                        {
                            if (actual.ChildNodes[3].ChildNodes.Count > 1) contador_if = 1;
                            elsito = (Else)instruccion(actual.ChildNodes[4]);
                            return new If(expresion(actual.ChildNodes[1]), instrucciones(actual.ChildNodes[3].ChildNodes[contador_if]), elsito);
                        }
                    }
                case "else":
                    int contador = 0;
                    if (actual.ChildNodes[1].ChildNodes.Count > 1) contador = 1;
                    return new Else(instrucciones(actual.ChildNodes[1].ChildNodes[contador]));
                case "graficar":
                    return new GraficarTS();
                case "array":
                    string id_array = actual.ChildNodes[0].Token.Text.ToLower();
                    int line_a = actual.ChildNodes[0].Token.Location.Line + 1;
                    int col_a = actual.ChildNodes[0].Token.Location.Column + 1;
                    string tipo_array = actual.ChildNodes[6].ChildNodes[0].Token.Text;
                    Tipo array_type = getTipo(tipo_array);
                    int tipoarr = 0;
                    Arreglo new_array = new Arreglo(id_array, array_type, line_a, col_a, tipoarr);
                    Expresion dimension1 = null;
                    Expresion dimension2 = null;
                    List<int> indices_arr = new List<int>(); 
                    foreach (var dimension in actual.ChildNodes[4].ChildNodes) 
                    {
                        if (dimension.ChildNodes.Count > 1) 
                        { 
                            tipoarr = 1;
                            new_array.TipoArreglo = tipoarr;
                        }
                        dimension1 = expresion(dimension.ChildNodes[0]);
                        dimension2 = expresion(dimension.ChildNodes[1]);
                        indices_arr.Add(int.Parse(dimension1.Evaluar(this.global).Value.ToString()));
                        indices_arr.Add(int.Parse(dimension2.Evaluar(this.global).Value.ToString()));
                    }
                    return new DeclaArreglo(id_array, new_array, indices_arr);
                case "type":
                    String id_objeto = actual.ChildNodes[0].Token.Text.ToLower();
                    int l = actual.ChildNodes[0].Token.Location.Line;
                    int c = actual.ChildNodes[0].Token.Location.Column;
                    Objeto objeto = new Objeto(id_objeto, null, l+1, c+1);
                    List<Atributo> attrs = new List<Atributo>();
                    ParseTreeNode var_list = actual.ChildNodes[4];
                    foreach(var variable in var_list.ChildNodes)
                    {
                        String attr_tipo = variable.ChildNodes[1].ChildNodes[0].Token.Text;
                        Tipo obj_tipo = getTipo(attr_tipo);
                        foreach (var vars in variable.ChildNodes[0].ChildNodes) 
                        {
                            Atributo attr_nuevo = new Atributo(vars.Token.Text.ToLower(), obj_tipo, null, vars.Token.Location.Line+1, vars.Token.Location.Column+1);
                            attrs.Add(attr_nuevo);
                        }
                    }
                    objeto.Attribs = attrs;
                    return new DeclaObjeto(id_objeto, objeto);
                case "accesoobjeto":
                    List<string> ids_acceso2 = new List<string>();
                    foreach (var idacc in actual.ChildNodes) 
                    {
                        ids_acceso2.Add(idacc.Token.Text.ToLower());
                    }
                    return new AccesoObjeto(ids_acceso2);
                case "accesoarray":

                    break;
                case "case":
                    List<Caso> casos = new List<Caso>();
                    foreach (var casito in actual.ChildNodes[2].ChildNodes) {
                        Caso caso_nuevo;
                        List<Expresion> exps = new List<Expresion>();
                        int index_casos = 1;
                        foreach (var conds in casito.ChildNodes[0].ChildNodes) 
                        {
                            exps.Add(expresion(conds));
                        }
                        if (casito.ChildNodes.Count > 2) index_casos = 2;
                        caso_nuevo = new Caso(exps, instrucciones(casito.ChildNodes[index_casos]));
                        casos.Add(caso_nuevo);
                    }
                    return new Case(expresion(actual.ChildNodes[1]), casos, instruccion(actual.ChildNodes[3]));
                case "for":
                    int indice_for = 0;
                    bool reverse = false;
                    if (actual.ChildNodes[2].Token.Text.Equals("downto")) reverse = true;
                    if (actual.ChildNodes[5].ChildNodes.Count == 3) indice_for = 1;
                    String id_for = actual.ChildNodes[0].Token.Text.ToLower();
                    return new For(id_for, expresion(actual.ChildNodes[1]), expresion(actual.ChildNodes[3]), instrucciones(actual.ChildNodes[5].ChildNodes[indice_for]), reverse);
                case "repeat":
                    int cont_repeat = 0;
                    if (actual.ChildNodes[1].ChildNodes.Count >1) cont_repeat=1;
                    return new Repeat(instrucciones(actual.ChildNodes[1].ChildNodes[cont_repeat]), expresion(actual.ChildNodes[3]));
                case "exit":
                    if(actual.ChildNodes.Count > 1)
                        return new Exit(expresion(actual.ChildNodes[1]));
                    return new Exit(null);
                case "break":
                    return new Break();
                case "continue":
                    return new Continue();
            }
            return null;
        }

        public Expresion expresion(ParseTreeNode actual)
        {
            if (actual.ChildNodes.Count == 3)
            {
                string operador = actual.ChildNodes[1].Token.Text.ToLower();
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
                    case "%":
                        return new Aritmetica(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), '%');
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
                    case "and":
                        return new Logica(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), 'a');
                    case "or":
                        return new Logica(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), 'o');
                    case "mod":
                        return new Aritmetica(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), 'm');
                    case "div":
                        return new Aritmetica(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), 'd');
                    default:
                        return new Relacional(expresion(actual.ChildNodes[0]), expresion(actual.ChildNodes[2]), '=');
                }
            }
            else if (actual.ChildNodes.Count == 2) 
            {
                string tipo = actual.ChildNodes[0].Token.Text.ToLower();
                switch (tipo) 
                {
                    case "not":
                        return new Logica(expresion(actual.ChildNodes[1]), null, 'n');
                    default:
                        return new Aritmetica(expresion(actual.ChildNodes[1]), null, '-');
                }
            }
            else
            {
                string tipo = actual.ChildNodes[0].Term.Name;
                tipo = tipo.ToLower();
                switch (tipo)
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
                    case "llamada":
                        return new Primitivo('L', instruccion(actual.ChildNodes[0]));
                    case "accesoobjeto":
                        if (actual.ChildNodes[0].ChildNodes.Count == 1)
                            return new Primitivo('I', actual.ChildNodes[0].ChildNodes[0].Token.Text.ToLower());
                        return new Primitivo('O', instruccion(actual.ChildNodes[0]));
                    case "accesoarray":
                        return new Primitivo('A', instruccion(actual.ChildNodes[0]));
                    default:
                        return expresion(actual.ChildNodes[0]);
                }

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
        public Tipo getTipo(String op)
        {
            switch (op)
            {
                case "integer":
                case "entero":
                    return new Tipo(Tipos.INT, "integer");
                case "real":
                case "decimal":
                    return new Tipo(Tipos.REAL, "real");
                case "string":
                case "cadena":
                    return new Tipo(Tipos.STRING, "string");
                case "boolean":
                case "true":
                case "false":
                    return new Tipo(Tipos.BOOLEAN, "boolean");
                default:
                    return new Tipo(Tipos.OBJ, op);
            }
        }
    }
}
