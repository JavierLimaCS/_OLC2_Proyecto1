using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.AST
{
    class AST
    {
        private static int contador;
        private static string grafo;

        public static string getDot(ParseTreeNode raiz)
        {
            grafo = "digraph G{ \n node [shape=box, fontcolor=black fontname = \"Arial\"]\n";
            grafo += "nodo0[label=\"" + escapar(raiz.ToString()) + "\"];\n";
            contador = 1;
            recorrerAst("nodo0", raiz);
            grafo += "}";
            return grafo;
        }

        private static void recorrerAst(string padre, ParseTreeNode raiz)
        {
            foreach (ParseTreeNode hijo in raiz.ChildNodes)
            {
                String nameHijo = "nodo" + contador.ToString();
                if (hijo.Token == null)
                {
                    grafo += nameHijo + "[label=\"" + escapar(hijo.ToString()) + "\" style=filled];\n";
                }
                else
                {
                    grafo += nameHijo + "[label=\"" + escapar(hijo.ToString()) + "\"];\n";
                }
                grafo += padre + "->" + nameHijo + ";\n";
                contador++;
                recorrerAst(nameHijo, hijo);
            }
        }

        private static string escapar(string cadena)
        {
            cadena = cadena.Replace("\\", "\\\\");
            cadena = cadena.Replace("\"", "\\\"");
            return cadena;
        }
    }
}
