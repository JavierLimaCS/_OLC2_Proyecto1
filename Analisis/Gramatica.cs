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
            var ARRAY = ToTerm("array");
            var OBJECT = ToTerm("object");
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
            var ROF = ToTerm("of");
            var RWHILE = ToTerm("while");
            var RDO = ToTerm("do");
            var RREPEAT = ToTerm("repeat");
            var RUNTIL = ToTerm("until");
            var RFOR = ToTerm("for");
            var RTO = ToTerm("to");
            var RGRAF = ToTerm("graficar_ts");
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
            var MAYIG = ToTerm(">=", "Relacional");
            var MENIG = ToTerm("<=", "Relacional");
            var IGUAL = ToTerm("=", "Relacional");
            var DIFF = ToTerm("<>", "Relacional");
            var AND = ToTerm("and", "Logico");
            var OR = ToTerm("or", "Logico");
            var NOT = ToTerm("not", "Logico");
            var PAR1 = ToTerm("(", "Parentesis Apertura");
            var PAR2 = ToTerm(")", "Parentesis Cierre");
            var ASIGN = ToTerm(":=");
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
            NonTerminal Sentencias = new NonTerminal("Sentencias");
            NonTerminal Sentencia = new NonTerminal("Sentencia");
            NonTerminal Declaraciones = new NonTerminal("Declaracion");
            NonTerminal Constantes = new NonTerminal("Const");
            NonTerminal Declaracion = new NonTerminal("Var");
            NonTerminal Funcion = new NonTerminal("Funcion");
            NonTerminal Procedimiento = new NonTerminal("Procedimiento");
            NonTerminal Llamada = new NonTerminal("Llamada");
            NonTerminal Asignacion = new NonTerminal("Asignacion");
            NonTerminal Argumento = new NonTerminal("Argumento");
            NonTerminal Expresion = new NonTerminal("Exp");
            NonTerminal var_list = new NonTerminal("var_list");
            NonTerminal arguments_list = new NonTerminal("arguments_list");
            NonTerminal Tipo = new NonTerminal("Tipo");
            NonTerminal Value = new NonTerminal("Value");
            NonTerminal S_If = new NonTerminal("If");
            NonTerminal S_Else = new NonTerminal("Else");
            NonTerminal S_Else2 = new NonTerminal("Else");
            NonTerminal S_For = new NonTerminal("For");
            NonTerminal Cuerpo_Sentencias = new NonTerminal("body_sent");
            NonTerminal S_While = new NonTerminal("While");
            NonTerminal S_Write = new NonTerminal("Write");
            NonTerminal S_WriteLn = new NonTerminal("WriteLn");
            NonTerminal S_Graficar = new NonTerminal("Graficar");
            NonTerminal S_Exit = new NonTerminal("Exit");
            NonTerminal S_Case = new NonTerminal("Case");
            NonTerminal case_list = new NonTerminal("case_list");
            NonTerminal Caso = new NonTerminal("Caso");
            NonTerminal exp_list = new NonTerminal("exp_list");
            NonTerminal Sentencia2 = new NonTerminal("Sentencia");
            NonTerminal Arrays = new NonTerminal("Array");
            NonTerminal Types = new NonTerminal("Type");
            NonTerminal accessObj = new NonTerminal("AccesoObjeto");
            NonTerminal accessArr = new NonTerminal("AccesoArray");
            #endregion

            #region Gramatica
            this.Root = Raiz;
            Raiz.Rule
                = PROG + ID + PTCOMA + Instrucciones + BEGIN + Sentencias + END + PT;

            Instrucciones.Rule
                = MakeStarRule(Instrucciones, Instruccion);

            Instruccion.Rule
                = VAR + Declaraciones
                | Constantes
                | Funcion
                | Procedimiento
                | TYPE + Types
                | TYPE + Arrays;

            Declaraciones.Rule
                = MakePlusRule(Declaraciones, Declaracion);

            Declaracion.Rule
                = var_list + BIPUNTO + Tipo + Value + PTCOMA;

            Declaracion.ErrorRule
                = SyntaxError + PTCOMA;

            Value.Rule
                = MakeStarRule(Value, IGUAL + Expresion);
      
            Constantes.Rule
                = CONST + ID + IGUAL + Expresion + PTCOMA;

            Constantes.ErrorRule
                = SyntaxError + PTCOMA;

            Types.Rule
                = ID + IGUAL + OBJECT + VAR + Declaraciones + END + PTCOMA;

            Arrays.Rule
                = ID + IGUAL + ARRAY + COR1 + Expresion + PT + PT + Expresion + COR2 + ROF + Tipo + PTCOMA; 

            var_list.Rule
                = MakeListRule(var_list, COM, ID);

            Tipo.Rule
                = TINT
                | TSTRING
                | TREAL
                | TBOOL
                | ID;

            Funcion.Rule
                = FUNCT + ID + PAR1 + arguments_list + PAR2 + BIPUNTO + Tipo + PTCOMA + Instrucciones + BEGIN + Sentencias + END + PTCOMA
                | FUNCT + ID + PAR1 + PAR2 + BIPUNTO + Tipo + PTCOMA + Instrucciones + BEGIN + Sentencias + END + PTCOMA
                | FUNCT + ID + BIPUNTO + Tipo + PTCOMA + Instrucciones + BEGIN + Sentencias + END + PTCOMA;

            Procedimiento.Rule
                = PROC + ID + PAR1 + arguments_list + PAR2 + PTCOMA + Instrucciones + BEGIN + Sentencias + END + PTCOMA
                | PROC + ID + PTCOMA + Instrucciones + BEGIN + Sentencias + END + PTCOMA;
 
            arguments_list.Rule
                = MakeListRule(arguments_list, PTCOMA, Argumento);

            Argumento.Rule
                = var_list + BIPUNTO + Tipo
                | VAR + var_list + BIPUNTO + Tipo;

            Sentencias.Rule
                = MakeStarRule(Sentencias, Sentencia);

            Sentencia.Rule
                = Llamada + PTCOMA
                | Asignacion + PTCOMA
                | S_If + PTCOMA
                | S_For + PTCOMA
                | S_While + PTCOMA
                | S_WriteLn + PTCOMA
                | S_Write + PTCOMA
                | S_Exit + PTCOMA
                | S_Graficar + PTCOMA
                | S_Case + PTCOMA;

            Sentencia2.Rule
                = Llamada
                | Asignacion
                | S_If
                | S_For
                | S_While
                | S_WriteLn
                | S_Write
                | S_Exit
                | S_Graficar
                | S_Case
                ;

            accessArr.Rule
                =  ID + COR1 + Expresion + COR2;

            accessObj.Rule
                = ID + PT + ID;

            Llamada.Rule
                = ID + PAR1 + exp_list + PAR2 
                | ID + PAR1 + PAR2;

            Asignacion.Rule
                = ID + ASIGN + Expresion
                | ID + PT + ID + ASIGN + Expresion
                | ID + COR1 + Expresion + COR1 + ASIGN + Expresion;

            S_If.Rule
                = RIF + PAR1 + Expresion + PAR2 + RTHEN + Sentencia2 + RELSE + Sentencia2
                | RIF + PAR1 + Expresion + PAR2 + RTHEN + Cuerpo_Sentencias + S_Else;

            S_Else.Rule
                = MakeStarRule(S_Else, RELSE + Cuerpo_Sentencias);

            S_For.Rule
                = RFOR + ID + ASIGN + Expresion + RTO + Expresion + RDO + Cuerpo_Sentencias;

            Cuerpo_Sentencias.Rule
                = BEGIN + Sentencias + END
                | Sentencia2;

            S_Case.Rule 
                = RCASE + PAR1 + Expresion + PAR2 + ROF + case_list + PTCOMA + S_Else2 + END;

            case_list.Rule
                = MakeListRule(case_list, PTCOMA ,Caso);

            Caso.Rule
                = exp_list + BIPUNTO + Sentencia2
                | exp_list + BIPUNTO + BEGIN + Sentencias + END ;

            S_Else2.Rule
                = RELSE + Sentencia
                | Empty;

            S_While.Rule
                = RWHILE + PAR1 + Expresion + PAR2 + RDO + Cuerpo_Sentencias;

            S_WriteLn.Rule
                = WRTLN + PAR1 + exp_list + PAR2 ;

            S_Write.Rule
                = WRT + PAR1 + exp_list + PAR2;

            S_Exit.Rule
                = EXIT + PAR1 + PAR2 ;

            S_Graficar.Rule
                = RGRAF;

            Expresion.Rule
                = Expresion + MAS + Expresion
                | Expresion + MENOS + Expresion
                | Expresion + DIV + Expresion
                | Expresion + POR + Expresion
                | Expresion + MOD + Expresion
                | Expresion + MAYOR + Expresion
                | Expresion + MENOR + Expresion
                | Expresion + MAYIG + Expresion
                | Expresion + MENIG + Expresion
                | Expresion + DIFF + Expresion
                | Expresion + IGUAL + Expresion
                | Expresion + AND + Expresion
                | Expresion + OR + Expresion
                | NOT + Expresion
                | PAR1 + Expresion + PAR2
                | TTRUE
                | TFALSE
                | ENTERO
                | CADENA
                | DECIMAL
                | ID
                | Llamada
                | accessObj
                | accessArr;

            exp_list.Rule
                = MakeListRule(exp_list, COM, Expresion);

            #endregion

            #region Preferencias
            this.RegisterOperators(1, Associativity.Left, IGUAL, MAYOR, MENOR, MENIG, MAYIG, DIFF);
            this.RegisterOperators(2, Associativity.Left, MAS, MENOS, OR);
            this.RegisterOperators(3, Associativity.Left, POR, DIV, MOD, AND);
            this.RegisterOperators(4, Associativity.Neutral, PAR1, PAR2);
            #endregion

            #region Eliminacion
            this.MarkPunctuation(PTCOMA, BIPUNTO, PT, PAR1, PAR2, ASIGN);
            this.MarkPunctuation(PROG, CONST, FUNCT, PROC, ROF, RFOR, RTO);
            this.MarkTransient(Instruccion, Sentencia);
            
            #endregion

        }
    }
}
