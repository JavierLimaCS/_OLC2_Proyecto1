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
        public TabladeSimbolos ts = new TabladeSimbolos(null, "Global");
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
            Simbolo_Funcion funcionPadre;
            Simbolo_Funcion funcionhija;
            foreach (var nodo in instrucciones.ChildNodes) 
            {
                String no_terminal = nodo.Term.Name;
                switch (no_terminal) 
                {
                    case "Funcion":
                        funcionPadre = new Simbolo_Funcion(nodo.ChildNodes[0].Token.Text, new Tipo(Tipos.FUNCTION, "funcion"), nodo.ChildNodes[0].Token.Location.Line + 1, nodo.ChildNodes[0].Token.Location.Column + 1);
                        this.ts.declararFuncion(nodo.ChildNodes[0].Token.Text, funcionPadre);
                        if (existeFuncion(nodo)) 
                        {
                            TabladeSimbolos nuevo_entorno = new TabladeSimbolos(ts, nodo.ChildNodes[0].Token.Text);
                            foreach (var hijo in nodo.ChildNodes[3].ChildNodes) 
                            { 
                                if(hijo.Term.Name == "Funcion")
                                {
                                    String nuevo_id = nodo.ChildNodes[0].Token.Text + "_" + hijo.ChildNodes[0].Token.Text;
                                    String nueva_scope = nodo.ChildNodes[0].Token.Text;
                                    funcionhija = new Simbolo_Funcion(nuevo_id, new Tipo(Tipos.FUNCTION, "funcion"), hijo.ChildNodes[0].Token.Location.Line + 1, hijo.ChildNodes[0].Token.Location.Column + 1);
                                    nuevo_entorno.declararFuncion(nuevo_id, funcionhija);
                                }
                                if (hijo.Term.Name == "Declaracion")
                                {
                                    String tipito_var = hijo.ChildNodes[0].ChildNodes[1].ChildNodes[0].Token.Text;
                                    String nueva_id = nodo.ChildNodes[0].Token.Text + "_" + hijo.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Text;
                                    Simbolo var_funcion = new Simbolo(nueva_id, new Tipo(Tipos.INT, tipito_var), hijo.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Location.Line + 1, hijo.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Location.Column+1);
                                    nuevo_entorno.declararVariable(nueva_id, var_funcion);
                                }
                            }
                            nuevo_entorno.generarTS();
                        };
                        break;
                    case "Declaracion":
                        String tipo_var = nodo.ChildNodes[0].ChildNodes[1].ChildNodes[0].Token.Text;
                        String id_var = nodo.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Text;
                        Simbolo nueva_var = new Simbolo(id_var, new Tipo(Tipos.INT, tipo_var), nodo.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Location.Line + 1, nodo.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Location.Column + 1);
                        this.ts.declararVariable(id_var, nueva_var);
                        break;
                
                }
            }
        }

        public Boolean existeFuncion(ParseTreeNode Sentencias) 
        {
            foreach (var funcionAnidada in Sentencias.ChildNodes[3].ChildNodes)
            {
                if (funcionAnidada.Term.Name == "Funcion")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
