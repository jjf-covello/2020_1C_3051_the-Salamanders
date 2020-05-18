using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace TGC.Group.Model
{
    class Puerta : IInteractuable
    {
        TGCVector3 posicionEntrada = new TGCVector3(-1222,15, -6857);
        TGCVector3 posicionSalida = new TGCVector3(-1200,15, -7500);
        public void Interactuar(Personaje personaje)
        {
            this.Usar(personaje);
        }

        public void Usar(Personaje personaje)
        {
            if (personaje.estoyAdentro)
            {
                personaje.TeletrasportarmeA(posicionSalida);
            }
            else
            {
                personaje.TeletrasportarmeA(posicionEntrada);
            }

            personaje.estoyAdentro = !personaje.estoyAdentro;
           
        }
    }
}
