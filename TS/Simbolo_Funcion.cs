using Proyecto1.Interprete.Instruccion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.TS
{
    class Simbolo_Funcion : Simbolo
    {
        private LinkedList<Simbolo> listaParametros;
        private LinkedList<Instruccion> listaInstruccions;
        private LinkedList<Instruccion> listaSentencias;
        public Simbolo_Funcion(string n, Tipo ty, int l, int c) : base(n, ty, l, c)
        {
            this.Id = n;
            this.Tipo = ty;
            this.Linea = l;
            this.Columna = c;
        }

        public LinkedList<Simbolo> Params { get => listaParametros; set => listaParametros = value; }
        public LinkedList<Instruccion> listaInst { get => listaInstruccions; set => listaInstruccions = value; }
        public LinkedList<Instruccion> listaSent { get => listaSentencias; set => listaSentencias = value; }

    }
}
