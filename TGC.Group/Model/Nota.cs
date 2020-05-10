using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    class Nota : IRecolectable
    {
        public void Interactuar(Personaje personaje)
        {
            throw new NotImplementedException();
        }

        public void Recolectar(Personaje personaje)
        {
            if(personaje.cantidadNotas < personaje.notasRequeridas)
            {
                personaje.cantidadNotas++;
            }
            else
            {
                //Ganaste
            }
            
        }
    }
}
