using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Proyecto1.Analisis
{
    class Error
    {
        private String tipo;
        private String descripcion;
        private int linea;
        private int columna;

        public Error(String tipo, String descripcion, int linea, int columna)
        {
            this.tipo = tipo;
            this.descripcion = descripcion;
            this.linea = linea;
            this.columna = columna;
        }
        public String Descripcion { get => descripcion; set => descripcion = value; }
        public String Tipo { get => tipo; set => tipo = value; }
        public int Linea { get => linea; set => linea = value; }
        public int Columna { get => columna; set => columna = value; }

    }
}
