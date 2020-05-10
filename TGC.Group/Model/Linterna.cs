using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    class Linterna : IUsable
    {
        //12000 = 120 segundos
        public float duracionMax = 12000;
        public float duracion = 12000;
        public void Interactuar(Personaje personaje)
        {
            
        }

        public void Recolectar(Personaje personaje)
        {
            if(!personaje.objetosInteractuables.Any( objeto => objeto is Linterna))
            {
                personaje.objetosInteractuables.Add(this);
            }
        }

        public void Usar(Personaje personaje)
        {
            //Me equipa la linterna
            //Una vez equipada en cada frame se va restando 1 simulando la duracion o tiempo pasado
            personaje.tieneLuz = true;
            duracion -= 1;   
        }

        public void Recargar()
        {
            this.duracion = duracionMax;
        }
    }
}
