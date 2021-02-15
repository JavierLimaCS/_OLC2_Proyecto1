using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Analisis
{
    class Traductor
    {
        public String console = "";
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
            traduccion(raiz.ChildNodes[1]);
        }

        public void traduccion(ParseTreeNode instrucciones) 
        {
            foreach (var nodo in instrucciones.ChildNodes) 
            {
                String no_terminal = nodo.Term.Name;
                switch (no_terminal) 
                {
                    case "":
                        break;
                
                }
            }
        }
    }
}
