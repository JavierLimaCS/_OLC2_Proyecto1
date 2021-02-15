using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.TS
{
    class TabladeSimbolos : LinkedList<Simbolo>
    {
        public TabladeSimbolos() : base() 
        { 
            
        }

        public Object getValor(String id)
        {
            foreach (Simbolo s in this)
            {
                if (s.Id.Equals(id))
                {
                    return s.Value;
                }
            }
            Console.WriteLine("La variable " + id + " no existe en este ámbito.");
            return null;
        }
        
        public void setValor(String id, Object valor)
        {
            foreach (Simbolo s in this)
            {
                if (s.Id.Equals(id))
                {
                    s.Value = valor;
                    return;
                }
            }
            Console.WriteLine("La variable " + id + " no existe en este ámbito, por lo "
                    + "que no puede asignársele un valor.");
        }

        public void generarTabladeSimbolos() {
            foreach (var simbolo in this)
            {
                System.Diagnostics.Debug.Write("Simbolo: "  + simbolo.Id);
            }
        }

    }
}
