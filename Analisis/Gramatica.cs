using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Proyecto1.Analisis
{
    class Gramatica : Grammar
    {
        public Gramatica() : base(caseSensitive: false)
        {
            #region Expresiones Regulares
            IdentifierTerminal ID = TerminalFactory.CreateCSharpIdentifier("Iden");
            NumberLiteral ENTERO = new NumberLiteral("Entero");
            RegexBasedTerminal DECIMAL = new RegexBasedTerminal("Decimal", "[0-9]+'.'[0-9]+");
            StringLiteral CADENA = new StringLiteral("Cadena");
            CADENA.AddStartEnd("\'", StringOptions.NoEscapes);
             #endregion

            #region Comentarios
            CommentTerminal COMENTARIOSIMPLE = new CommentTerminal("ComentarioSimple", "//", "\n", "\r\n");
            CommentTerminal COMENTARIOMULTIPLE = new CommentTerminal("ComentarioMultiple", "(*", "*)");
            CommentTerminal COMENTARIOMULTIPLE2 = new CommentTerminal("ComentarioMultiple2", "{", "}");
            NonGrammarTerminals.Add(COMENTARIOSIMPLE);
            NonGrammarTerminals.Add(COMENTARIOMULTIPLE);
            NonGrammarTerminals.Add(COMENTARIOMULTIPLE2);
            #endregion

            #region Terminales - PALABRAS
            var PROG = ToTerm("program");
            var TYPE = ToTerm("type");
            var VAR = ToTerm("var");
            var CONST = ToTerm("const");
            var BEGIN = ToTerm("begin");
            var END = ToTerm("end");
            var PROC = ToTerm("procedure");
            var FUNCT = ToTerm("function");
            var TTRUE = ToTerm("true");
            var TFALSE = ToTerm("false");
            var TSTRING = ToTerm("string");
            var TREAL = ToTerm("real");
            var TINT = ToTerm("integer");
            var TBOOL = ToTerm("boolean");
            var WRTLN = ToTerm("writeln");
            var WRT = ToTerm("write");
            var EXIT = ToTerm("exit");
            var RIF = ToTerm("if");
            var RTHEN = ToTerm("then");
            var RELSE = ToTerm("else");
            var RCASE = ToTerm("case");
            var RWHILE = ToTerm("while");
            var RDO = ToTerm("do");
            var RREPEAT = ToTerm("repeat");
            var RUNTIL = ToTerm("until");
            var RBREAK = ToTerm("break");
            var RCONTINUE = ToTerm("continue");
            #endregion

            #region Terminales - SIGNOS
            var MAS = ToTerm("+", "Aritmetico");
            var MENOS = ToTerm("-", "Aritmetico");
            var POR = ToTerm("*", "Aritmetico");
            var DIV = ToTerm("/", "Aritmetico");
            var MOD = ToTerm("%", "Aritmetico");
            var MAYOR = ToTerm(">", "Relacional");
            var MENOR = ToTerm("<", "Relacional");
            var MANIG = ToTerm(">=", "Relacional");
            var MENIG = ToTerm("<=", "Relacional");
            var IGUAL = ToTerm("=", "Relacional");
            var DIFF = ToTerm("<>", "Relacional");
            var AND = ToTerm("and", "Logico");
            var OR = ToTerm("or", "Logico");
            var NOT = ToTerm("not", "Logico");
            var PAR1 = ToTerm("(");
            var PAR2 = ToTerm(")");
            var ASIGN = ToTerm(":=");
            var PTCOMA = ToTerm(";");
            var LLAVE1 = ToTerm("{");
            var LLAVE2 = ToTerm("}");
            var COR1 = ToTerm("[");
            var COR2 = ToTerm("]");
            var COM = ToTerm(",");
            var BIPUNTO = ToTerm(":");
            var PT = ToTerm(".");
            #endregion

            #region No Terminales
            NonTerminal Raiz = new NonTerminal("Raiz");
            NonTerminal Instrucciones = new NonTerminal("Instrucciones");
            NonTerminal Instruccion = new NonTerminal("Instruccion");
            NonTerminal Instrucciones2 = new NonTerminal("Instrucciones2");
            NonTerminal Instruccion2 = new NonTerminal("Instruccion2");
            NonTerminal Declaracion = new NonTerminal("Declaracion");
            NonTerminal Funcion = new NonTerminal("Funcion");
            NonTerminal Procedimiento = new NonTerminal("Procedimiento");
            NonTerminal Llamada = new NonTerminal("Llamada");
            NonTerminal Expresion = new NonTerminal("Expresion");
            NonTerminal var_list = new NonTerminal("var_list");
            NonTerminal tipos = new NonTerminal("tipos");
            #endregion

            #region Gramatica
            this.Root = Raiz;
            Raiz.Rule
                = PROG + ID + PTCOMA + Instrucciones + BEGIN + Instrucciones2 + END + PT;

            Raiz.ErrorRule
                = SyntaxError + PTCOMA;

            Instrucciones.Rule
                = MakePlusRule(Instrucciones, Instruccion);

            Instruccion.Rule
                = Declaracion
                | Funcion
                | Procedimiento
                | Empty;

            Declaracion.Rule
                = VAR + var_list + BIPUNTO + tipos + PTCOMA
                | CONST + ID + IGUAL + Expresion + PTCOMA;

            var_list.Rule
                = MakeListRule(var_list, COM, ID);

            tipos.Rule
                = TINT
                | TSTRING
                | TREAL
                | TBOOL;
            
            Funcion.Rule
                = FUNCT;

            Procedimiento.Rule
                = PROC;

            Instrucciones2.Rule
                = MakePlusRule(Instrucciones2, Instruccion2);

            Instruccion2.Rule
                = Llamada
                | Empty;

            Llamada.Rule
                = ID + PAR1 + PAR2;

            Expresion.Rule
                = Expresion + MAS + Expresion
                | Expresion + MENOS + Expresion
                | Expresion + DIV + Expresion
                | Expresion + POR + Expresion
                | Expresion + MOD + Expresion
                | TTRUE
                | TFALSE
                | ENTERO
                | CADENA
                | DECIMAL;

            #endregion

            #region Preferencias
            this.RegisterOperators(1, Associativity.Left, MAS, MENOS);
            this.RegisterOperators(2, Associativity.Left, POR, DIV);
            this.RegisterOperators(3, Associativity.Left, MOD);
            #endregion

            #region Eliminacion
            this.MarkPunctuation(PTCOMA, BIPUNTO, PT, IGUAL, ASIGN);
            this.MarkTransient(Instruccion, Instruccion2);
            #endregion

        }
    }
}
