using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class Caso
    {
        List<Expresion.Expresion> condiciones;
        LinkedList<Instruccion> sentencias;
        public Caso(List<Expresion.Expresion> conds, LinkedList<Instruccion> sents) 
        {
            this.Condiciones = conds;
            this.Sentencias = sents;
        }
        public List<Expresion.Expresion> Condiciones { get => condiciones; set => condiciones = value; }
        public LinkedList<Instruccion> Sentencias { get => sentencias; set => sentencias = value; }
    }
}
