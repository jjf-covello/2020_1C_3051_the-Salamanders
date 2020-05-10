using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TGC.Group.Model
{
    class Vela : IUsable
    {
        //6000 = 60 segundos
        public float duracionMax = 6000; 
        public float duracion = 6000;
        
        public void Interactuar(Personaje personaje)
        {
            throw new NotImplementedException();
        }

        public void Recolectar(Personaje personaje)
        {
            if(personaje.objetosInteractuables.Any(objeto => objeto is Vela))
            {
                var vela = (Vela)personaje.objetosInteractuables.Find(objeto => objeto is Vela);
                vela.aumentarDuracion();
            }
            else
            {
                personaje.objetosInteractuables.Add(this);
            }
            
        }

        public void Usar(Personaje personaje)
        {
            //Me equipa la vela
            //Una vez equipada en cada frame se va restando 1 simulando la duracion o tiempo pasado
            personaje.tieneLuz = true;
            duracion -= 1;
        }

        public void aumentarDuracion()
        {
            this.duracion = duracionMax;
        }
    }
}
