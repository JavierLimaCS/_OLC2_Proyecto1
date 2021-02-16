using Irony.Parsing;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Analisis
{
    class Traductor
    {
        public String console = "";
        public TabladeSimbolos ts = new TabladeSimbolos();
        public void Traducir(String cadena, String filename) 
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;
            if (raiz == null) 
            {
                console = "El archivo " + filename  + " no puede traducirse porque contiene errores.";
                return;
            }
            traduccion(raiz.ChildNodes[1], "global");
        }

        public void traduccion(ParseTreeNode instrucciones, String scope) 
        {
            Simbolo_Funcion funcionPadre;
            Simbolo_Funcion funcionhija;
            foreach (var nodo in instrucciones.ChildNodes) 
            {
                String no_terminal = nodo.Term.Name;
                switch (no_terminal) 
                {
                    case "Funcion":
                        funcionPadre = new Simbolo_Funcion(nodo.ChildNodes[0].Token.Text, Simbolo.Tipo.FUNC, scope, nodo.ChildNodes[0].Token.Location.Line + 1, nodo.ChildNodes[0].Token.Location.Column + 1);
                        this.ts.AddLast(funcionPadre);
                        foreach (var funcionAnidada in nodo.ChildNodes[3].ChildNodes)
                        {
                            if (funcionAnidada.Term.Name == "Funcion") 
                            { 
                            
                            };
                        }
                        break;
                
                }
            }
        }
    }
}
