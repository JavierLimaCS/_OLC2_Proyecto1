using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2_Proyecto1.Analizador
{
    class Analizador
    {
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
                Console.WriteLine(arbol.ParserMessages[0].Message);
                return;
            }
            Console.WriteLine("Exito ALV");
        }
        
    }
}
