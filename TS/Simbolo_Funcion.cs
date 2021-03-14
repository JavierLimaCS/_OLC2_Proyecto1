using Proyecto1.Interprete.Instruccion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{
    class Simbolo_Funcion : Simbolo
    {
        private LinkedList<Instruccion> listaInstruccions;
        private LinkedList<Instruccion> listaSentencias;
        private Dictionary<string,Parametro> param;
        public Simbolo_Funcion(string n, Tipo ty, int l, int c) : base(n, ty, l, c)
        {
            this.Id = n;
            this.Tipo = ty;
            this.Linea = l;
            this.Columna = c;
            this.esConstante = false;
            this.listaInstruccions = new LinkedList<Instruccion>();
            this.listaSentencias = new LinkedList<Instruccion>();
            this.param = new Dictionary<string, Parametro>();
        }

        public Dictionary<string,Parametro> Params { get => param; set => param = value; }
        public LinkedList<Instruccion> listaInst { get => listaInstruccions; set => listaInstruccions = value; }
        public LinkedList<Instruccion> listaSent { get => listaSentencias; set => listaSentencias = value; }

    }
}
