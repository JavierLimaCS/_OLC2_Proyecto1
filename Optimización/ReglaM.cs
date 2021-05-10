using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Optimización
{
    class ReglaM
    {
        string tipo;
        string _regla;
        string codigo_anterior;
        string codigo_actual;
        int fila;
       
        public ReglaM(string tipo, string regla, string cod_anter, string cod_nuevo, int fila)
        {
            this.tipo = tipo;
            this._regla = regla;
            this.codigo_anterior = cod_anter;
            this.codigo_actual = cod_nuevo;
            this.fila = fila;
        }

        public string Tipo { get => tipo; set => tipo = value; }
        public string _Regla { get => _regla; set => _regla = value; }
        public string Codigo_anterior { get => codigo_anterior; set => codigo_anterior = value; }
        public string Codigo_Actual { get => codigo_actual; set => codigo_actual = value; }
        public int Fila { get => fila; set => fila = value; }
    }
}
