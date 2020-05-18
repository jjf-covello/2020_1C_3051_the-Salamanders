using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
     public class Escalera
    {

        public List<TgcMesh> escalones;
        public TgcMesh escalonActual;

        public void pasarPorEscalon(Personaje personaje)
        {
            if (estasFrenteAlEscalon(personaje))
            {

                //podes Seguir subiendo

                //mover el personaje arriba del escalon actual
                //TODO
                var posicionY = personaje.Position.Y + escalonActual.BoundingBox.PMax.Y;
                escalonActual = escalonSiguiente();
                TGCVector3 nuevaPosicion = new TGCVector3(personaje.Position.X, posicionY, personaje.Position.Z);
                //solo lo levanto para que no quede pegado a la escalera el x y el z se van a modificar solos con el update de la camara
                personaje.TeletrasportarmeA(nuevaPosicion);

            }

        }

        private bool estasFrenteAlEscalon(Personaje personaje)
        {
            var escalonMasCercano = escalones.OrderBy(mesh => personaje.DistanciaHacia(mesh)).First();
            return escalonMasCercano.Equals(escalonActual);
        }
        private TgcMesh escalonSiguiente()
        {
            escalones.OrderBy(mesh => mesh.Name);
            int index = escalones.FindIndex(mesh => mesh.Equals(escalonActual));
            if (index < escalones.Count())
            {
                index++;
            }
            return escalones.ElementAt(index);
        }
    }
}
