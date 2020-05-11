using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SkeletalAnimation;
using TGC.Core.Camara;
using TGC.Core.Input;
using Microsoft.DirectX.Direct3D;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    class Monster
    {
        TgcMesh ghost;
        String MediaDir = "..\\..\\..\\Media\\";
        public void InstanciarMonster()
        {
            var loader = new TgcSceneLoader();
            var scene2 = loader.loadSceneFromFile(MediaDir + "Modelame\\GhostGrande-TgcScene.xml"); //Con demon no funca, aca rompe

            //Solo nos interesa el primer modelo de esta escena (tiene solo uno)
            ghost = scene2.Meshes[0];

            ghost.Position = new TGCVector3(200, -350, 100);
            //ghost.Transform = TGCMatrix.Translation(0, 5, 0);
        }

        public void RenderMonster()
        {
            ghost.Render();
        }

        public void DisposeMonster()
        {
            ghost.Dispose();
        }

        //Cuando el player no usa una fuente luminosa en X tiempo
        public void Aparecer(Personaje personaje)
        {
            if (!personaje.tieneLuz && personaje.tiempoSinLuz > 1000)
            {
                if(personaje.tiempoSinLuz > 3000)
                {
                    //Hacer toda la bola de la rotacion de camara
                    //PERDES!!!
                }
                else
                { 
                    TGCVector3 posPersonaje = personaje.getPosition();
                    this.ModificarPosicion(posPersonaje);
                }
            }
        }

        //Cuando el player usa una fuente luminosa o llega a un refugio
        public void Desaparecer()
        {
            ghost.Position = new TGCVector3(0, -2000, 0); //Lo mando abajo del mapa 
        }

        //Rota siempre en la direccion en la que se mueve el jugador
        public void MirarAlJugador()
        {
            //Again con la rotacion del mesh
        }

        public void ModificarPosicion(TGCVector3 posicion)
        {
            var posicionLejana = new TGCVector3(500, -350, 500);

            ghost.Position = posicion + posicionLejana;
        }
    }
}
