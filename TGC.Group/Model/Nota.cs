using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    class Nota : IInteractuable
    {
        public void Interactuar(Personaje personaje)
        {
            personaje.cantidadNotas++;
        }

        public void Usar(Personaje personaje) { }

    }
}
