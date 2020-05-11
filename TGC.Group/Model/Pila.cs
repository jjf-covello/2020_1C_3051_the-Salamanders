using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    class Pila : IInteractuable
    {
        public void Interactuar(Personaje personaje)
        {
            personaje.objetosInteractuables.Add(this);
        }

        public void Usar(Personaje personaje)
        {
            if(personaje.objetosInteractuables.Any(objeto => objeto is Linterna))
            {
                var linterna = (Linterna)personaje.objetosInteractuables.Find(objeto => objeto is Linterna);
                linterna.Recargar();
                
                personaje.objetosInteractuables.Remove(this);
            }
            
        }
    }
}
