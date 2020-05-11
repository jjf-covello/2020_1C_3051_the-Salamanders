using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TGC.Group.Model
{
    class Vela : IEquipable
    {
        //6000 = 60 segundos
        public float duracionMax = 6000; 
        public float duracion = 6000;
        public bool estaEncendida = true;
        
        public void Interactuar(Personaje personaje)
        {
            if (personaje.objetosInteractuables.Any(objeto => objeto is Vela))
            {
                var vela = (Vela)personaje.objetosInteractuables.Find(objeto => objeto is Vela);
                vela.AumentarDuracion();
            }
            else
            {
                personaje.objetosInteractuables.Add(this);
            }
        }

        public void Usar(Personaje personaje) { }

        public void Equipar(Personaje personaje)
        {
            personaje.setItemEnMano(this);
        }
        
        public void FinDuracion(Personaje personaje)
        {
            this.DesecharVela(personaje);
        }

        public void AumentarDuracion()
        {
            this.duracion = duracionMax;
        }

        public void DisminuirDuracion()
        {
            this.duracion -= 1;
        }

        public void DesecharVela(Personaje personaje)
        {
            personaje.objetosInteractuables.Remove(this);
        }

        public float getDuracion()
        {
            return this.duracion;
        }
    }
}
