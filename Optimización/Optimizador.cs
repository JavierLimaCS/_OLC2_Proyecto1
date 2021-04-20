using Irony.Parsing;
using Proyecto1.Analisis;
using Proyecto2.Optimización.Reglas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Optimización
{
    class Optimizador
    {
        public string codigo;
        public LinkedList<Error> lista_errores;
        public Optimizador(string code)
        {
            this.codigo = code;
            this.lista_errores = new LinkedList<Error>();
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
        }

        public void optimizar(LinkedList<Instruccion3D> instrucciones) 
        {
            string newcode = "";
            foreach (var instruccion in instrucciones)
            {
                if (instruccion != null)
                {
                    newcode += instruccion.optimizar3d();
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
    }
}
