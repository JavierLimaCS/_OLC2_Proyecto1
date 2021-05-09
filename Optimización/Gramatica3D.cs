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
            RegexBasedTerminal temporal = new RegexBasedTerminal("tmp", "t[0-9]+");
            RegexBasedTerminal label = new RegexBasedTerminal("lbl", "L[0-9]+");
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
            var RET = ToTerm("return");
            var HEAP = ToTerm("Heap");
            var STACK = ToTerm("Stack");
            var PRINTF = ToTerm("printf");
            var IFF = ToTerm("if");
            var SP = ToTerm("SP");
            var HP = ToTerm("HP");
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
            var DIFF = ToTerm("!=", "Relacional");
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

            #region No Terminales
            NonTerminal Raiz = new NonTerminal("Raiz");
            NonTerminal Instrucciones = new NonTerminal("Instrucciones");
            NonTerminal Instruccion = new NonTerminal("Instruccion");
            NonTerminal Metodos = new NonTerminal("Metodos");
            NonTerminal Metodo = new NonTerminal("Metodo");
            NonTerminal Expresion = new NonTerminal("Expresion");
            NonTerminal Salto = new NonTerminal("Salto");
            NonTerminal Etiqueta = new NonTerminal("Etiqueta");
            NonTerminal Asignacion = new NonTerminal("Asignacion");
            NonTerminal Retorno = new NonTerminal("Retorno");
            NonTerminal Tipo = new NonTerminal("Tipo");
            NonTerminal Impresion = new NonTerminal("Impresion");
            NonTerminal Llamada = new NonTerminal("Llamada");
            NonTerminal Condicional = new NonTerminal("Condicional");
            #endregion

            #region GramaticaOptimizacion
            this.Root = Raiz;

            Raiz.Rule = VOID + MAIN + PAR1 + PAR2 + LLAVE1 + Instrucciones + LLAVE2 + Metodos;

            Metodos.Rule
                = MakeStarRule(Metodos, Metodo);

            Metodo.Rule
                = VOID + ID + PAR1 + PAR2 + LLAVE1 + Instrucciones + LLAVE2;

            Instrucciones.Rule
                = MakeStarRule(Instrucciones, Instruccion);

            Instruccion.Rule
                = Etiqueta
                | Salto
                | Asignacion + PTCOMA
                | Impresion
                | Llamada
                | Condicional
                | Retorno;

            Condicional.Rule
                = IFF + PAR1 + Expresion + PAR2 + Salto;

            Llamada.Rule
                = ID + PAR1 + PAR2 + PTCOMA;

            Etiqueta.Rule
                = label + BIPUNTO;

            Impresion.Rule
                = PRINTF + PAR1 + CADENA + COM + PAR1 + Tipo + PAR2 + Expresion + PAR2 + PTCOMA; 

            Salto.Rule
                = GOTO + label + PTCOMA;

            Asignacion.Rule
                = temporal + ASIGN + Expresion
                | ID + ASIGN + Expresion
                | HEAP + COR1 + Expresion + COR2 + ASIGN + Expresion
                | STACK + COR1 + Expresion + COR2 + ASIGN + Expresion;

            Retorno.Rule
                = RET + PTCOMA;

            Expresion.Rule
                = MENOS + Expresion
                | Expresion + MAS + Expresion
                | Expresion + MENOS + Expresion
                | Expresion + DIV + Expresion
                | Expresion + POR + Expresion
                | Expresion + MOD + Expresion
                | Expresion + MAYOR + Expresion
                | Expresion + MENOR + Expresion
                | Expresion + IGUAL + Expresion
                | Expresion + MAYIG + Expresion
                | Expresion + MENIG + Expresion
                | Expresion + DIFF + Expresion
                | ENTERO
                | CADENA
                | DECIMAL
                | HEAP + COR1 + Expresion + COR2
                | STACK + COR1 + Expresion + COR2
                | temporal
                | PAR1 + Tipo + PAR2 + temporal
                | PAR1 + Tipo + PAR2 + SP
                | PAR1 + Tipo + PAR2 + HP
                | ID;

            Tipo.Rule
                = INT | CHAR | FLOAT;

            #endregion

            #region Preferencias
            this.RegisterOperators(1, Associativity.Left, IGUAL, MAYOR, MENOR, MENIG, MAYIG, DIFF);
            this.RegisterOperators(2, Associativity.Left, MAS, MENOS);
            this.RegisterOperators(3, Associativity.Left, POR, DIV, MOD);
            // this.RegisterOperators(4, Associativity.Right, MENOS);
            this.RegisterOperators(7, Associativity.Neutral, PAR1, PAR2);
            #endregion


            #region Eliminacion
            this.MarkPunctuation(PTCOMA, BIPUNTO, PT, PAR1, PAR2, ASIGN, LLAVE1, LLAVE2);
            this.MarkTransient(Instruccion);
            #endregion
        }

    }
}
