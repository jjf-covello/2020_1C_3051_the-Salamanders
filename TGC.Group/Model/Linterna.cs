using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    class Linterna : IEquipable
    {
        //12000 = 120 segundos
        public float duracionMax = 12000;
        public float duracion = 12000;
        public bool estaEncendida = false;
        
        public void Interactuar(Personaje personaje)
        {
            if (!personaje.objetosInteractuables.Any(objeto => objeto is Linterna))
            {
                personaje.objetosInteractuables.Add(this);
            }
        }

        public void Usar(Personaje personaje)
        {
            if (this.estaEncendida)
            {
                this.ApagarLinterna();
            }
            else
            {
                if (personaje.tieneLuz)
                {
                    personaje.UsarItemEnMano();
                }

                this.EncenderLinterna();
            }                  
        }

        public void Equipar(Personaje personaje)
        {
            personaje.setItemEnMano(this);
        }

        public void FinDuracion(Personaje personaje)
        {
            this.ApagarLinterna();
        }

        public void EncenderLinterna()
        {
           this.estaEncendida = true;
        }

        public void ApagarLinterna()
        {
            this.estaEncendida = false;
        }

        public void Recargar()
        {
            this.duracion = duracionMax;
        }

        public void DisminuirDuracion()
        {
            if (this.estaEncendida)
            {
                this.duracion -= 1;
            }
        }

        public float getDuracion()
        {
            return this.duracion;
        }
    }
}
