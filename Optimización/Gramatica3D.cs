using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Optimización
{
    class Gramatica3D : Grammar
    {
        public Gramatica3D() : base(caseSensitive: false)
        {
            #region Expresiones Regulares
            IdentifierTerminal ID = TerminalFactory.CreateCSharpIdentifier("Id");
            NumberLiteral ENTERO = new NumberLiteral("Entero");
            RegexBasedTerminal DECIMAL = new RegexBasedTerminal("Decimal", "[0-9]+[.][0-9]+");
            StringLiteral CADENA = new StringLiteral("Cadena");
            CADENA.AddStartEnd("\"", StringOptions.NoEscapes);
            #endregion

            #region Comentarios
            CommentTerminal COMENTARIOSIMPLE = new CommentTerminal("ComentarioSimple", "//", "\n", "\r\n");
            CommentTerminal COMENTARIOMULTIPLE = new CommentTerminal("ComentarioMultiple", "/*", "*\\");
            NonGrammarTerminals.Add(COMENTARIOSIMPLE);
            NonGrammarTerminals.Add(COMENTARIOMULTIPLE);
            #endregion

            #region Terminales - PALABRAS
            var VOID = ToTerm("void");
            var MAIN = ToTerm("main");
            var FLOAT = ToTerm("float");
            var INT = ToTerm("int");
            var CHAR = ToTerm("char");
            var GOTO = ToTerm("goto");
            #endregion

            #region Terminales - SIGNOS
            var MAS = ToTerm("+", "Aritmetico");
            var MENOS = ToTerm("-", "Aritmetico");
            var POR = ToTerm("*", "Aritmetico");
            var DIV = ToTerm("/", "Aritmetico");
            var MOD = ToTerm("%", "Aritmetico");
            var MAYOR = ToTerm(">", "Relacional");
            var MENOR = ToTerm("<", "Relacional");
            var MAYIG = ToTerm(">=", "Relacional");
            var MENIG = ToTerm("<=", "Relacional");
            var IGUAL = ToTerm("==", "Relacional");
            var DIFF = ToTerm("<>", "Relacional");
            var PAR1 = ToTerm("(", "Parentesis Apertura");
            var PAR2 = ToTerm(")", "Parentesis Cierre");
            var ASIGN = ToTerm("=");
            var PTCOMA = ToTerm(";", "Punto y coma");
            var LLAVE1 = ToTerm("{");
            var LLAVE2 = ToTerm("}");
            var COR1 = ToTerm("[", "Corchete Apertura");
            var COR2 = ToTerm("]", "Corchete Cierre");
            var COM = ToTerm(",");
            var BIPUNTO = ToTerm(":");
            var PT = ToTerm(".");
            #endregion

        }

    }
}
