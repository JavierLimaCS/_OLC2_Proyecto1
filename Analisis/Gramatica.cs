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
            IdentifierTerminal ID = TerminalFactory.CreateCSharpIdentifier("Id");
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
            var RFOR = ToTerm("for");
            var RGRAF = ToTerm("graficat_ts");
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
            NonTerminal Sentencias = new NonTerminal("Sentencias");
            NonTerminal Sentencia = new NonTerminal("Sentencia");
            NonTerminal Declaraciones = new NonTerminal("Declaraciones");
            NonTerminal Constantes = new NonTerminal("Constantes");
            NonTerminal Declaracion = new NonTerminal("Declaracion");
            NonTerminal Funcion = new NonTerminal("Funcion");
            NonTerminal Procedimiento = new NonTerminal("Procedimiento");
            NonTerminal Llamada = new NonTerminal("Llamada");
            NonTerminal Asignacion = new NonTerminal("Asignacion");
            NonTerminal Argumento = new NonTerminal("Argumento");
            NonTerminal Expresion = new NonTerminal("Expresion");
            NonTerminal var_list = new NonTerminal("var_list");
            NonTerminal arguments_list = new NonTerminal("arguments_list");
            NonTerminal Tipo = new NonTerminal("Tipo");
            NonTerminal Value = new NonTerminal("Value");
            NonTerminal S_If = new NonTerminal("S_If");
            NonTerminal S_For = new NonTerminal("S_For");
            NonTerminal S_While = new NonTerminal("S_While");
            NonTerminal S_Write = new NonTerminal("S_Write");
            NonTerminal S_WriteLn = new NonTerminal("S_WriteLn");
            NonTerminal S_Graficar = new NonTerminal("S_Graficar");
            NonTerminal S_Exit = new NonTerminal("S_Exit");
            #endregion

            #region Gramatica
            this.Root = Raiz;
            Raiz.Rule
                = PROG + ID + PTCOMA + Instrucciones + BEGIN + Sentencias + END + PT;

            Instrucciones.Rule
                = MakePlusRule(Instrucciones, Instruccion);

            Instruccion.Rule
                = VAR + Declaraciones
                | Constantes
                | Funcion
                | Procedimiento
                | Empty;


            Declaraciones.Rule
                = MakePlusRule(Declaraciones, Declaracion);

            Declaracion.Rule
                = var_list + BIPUNTO + Tipo + Value + PTCOMA;

            Declaracion.ErrorRule
                = SyntaxError + PTCOMA;

            Value.Rule
                = IGUAL + Expresion
                | Empty;

            Constantes.Rule
                = CONST + ID + IGUAL + Expresion + PTCOMA;

            Constantes.ErrorRule
                = SyntaxError + PTCOMA;

            var_list.Rule
                = MakeListRule(var_list, COM, ID);

            Tipo.Rule
                = TINT
                | TSTRING
                | TREAL
                | TBOOL;

            Funcion.Rule
                = FUNCT + ID + PAR1 + arguments_list + PAR2 + Instrucciones + BEGIN + Sentencias + END + PTCOMA;

            Procedimiento.Rule
                = PROC + ID + PAR1 + arguments_list + PAR2 + Instrucciones + BEGIN + Sentencias + END + PTCOMA;

            arguments_list.Rule
                = MakeListRule(arguments_list, COM, Argumento);

            Argumento.Rule
                = ID + BIPUNTO + Tipo
                | ID;

            Sentencias.Rule
                = MakePlusRule(Sentencias, Sentencia);

            Sentencia.Rule
                = Llamada
                | Asignacion
                | S_If
                | S_For
                | S_While
                | S_WriteLn
                | S_Write
                | S_Exit
                | S_Graficar
                | Empty;


            Llamada.Rule
                = ID + PAR1 + PAR2 + PTCOMA;

            Asignacion.Rule
                = ID + ASIGN + Expresion + PTCOMA;

            S_If.Rule
                = RIF + PAR1 + Expresion + PAR2 + RTHEN + Sentencias;

            S_For.Rule
                = RFOR + PAR1 + Expresion + PAR2;

            S_While.Rule
                = RWHILE + PAR1 + Expresion + PAR2;

            S_WriteLn.Rule
                = WRTLN + PAR1 + Expresion + PAR2 + PTCOMA;

            S_Write.Rule
                = WRT + PAR1 + Expresion + PAR2 + PTCOMA;

            S_Exit.Rule
                = EXIT + PAR1 + PAR2 + PTCOMA;

            S_Graficar.Rule
                = RGRAF + PTCOMA;

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
            this.MarkPunctuation(PTCOMA, BIPUNTO, PT,PAR1, PAR2,  IGUAL, ASIGN);
            this.MarkPunctuation(PROG, VAR, CONST, FUNCT, PROC);
            this.MarkTransient(Instruccion, Sentencia);
            
            #endregion

        }
    }
}
