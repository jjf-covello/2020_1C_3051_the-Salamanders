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

namespace TGC.Group.Model
{
    class Personaje
    {
        private TgcSkeletalBoneAttach attachment;
        private TgcSkeletalMesh mesh;
        private string selectedAnim;
        private const float velocidad_movimiento = 100f;
        String MediaDir = "..\\..\\..\\Media\\";

        public void InstanciarPersonaje()
        {
            
            //Paths para archivo XML de la malla
            var pathMesh = MediaDir + "SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml";

            //Path para carpeta de texturas de la malla
            var mediaPath = MediaDir + "SkeletalAnimations\\BasicHuman\\";
            
            //Lista de animaciones disponibles
            string[] animationList =
            {
                "CrouchWalk",
                "FlyingKick",
                "HighKick",
                "Jump",
                "LowKick",
                "Push",
                "Run",
                "StandBy",
                "Talk",
                "Walk"
            };

            //Crear rutas con cada animacion
            var animationsPath = new string[animationList.Length];
            for (var i = 0; i < animationList.Length; i++)
            {
                animationsPath[i] = MediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + animationList[i] + "-TgcSkeletalAnim.xml";
            }

            //Cargar mesh y animaciones
            TgcSkeletalLoader loader = new TgcSkeletalLoader();
            mesh = loader.loadMeshAndAnimationsFromFile(pathMesh, mediaPath, animationsPath);

            mesh.Transform = TGCMatrix.Scaling(2f,2f,2f);
            mesh.Position = new TGCVector3(100, 15, 100);
            //mesh.rotateY(Geometry.DegreeToRadian(180f));

            //Crear esqueleto a modo Debug
            //meshito.buildSkletonMesh();

            //Elegir animacion Caminando
            //mesh.playAnimation(selectedAnim, true);



            //Crear caja como modelo de Attachment del hueso "Bip01 L Hand"
            //Queremos que se haga un attachment de la linterna y la vela eventualmente
            /*
            attachment = new TgcSkeletalBoneAttach();
            var attachmentBox = TGCBox.fromSize(new TGCVector3(5, 100, 5), Color.Blue);
            attachment.Mesh = attachmentBox.ToMesh("attachment");
            attachment.Bone = mesh.getBoneByName("Bip01 L Hand");
            attachment.Offset = TGCMatrix.Translation(10, -40, 0);
            attachment.updateValues();
            */
        }

        public void RenderPersonaje(float elapsedTime)
        {
            //Se puede renderizar todo mucho mas simple (sin esqueleto) de la siguiente forma:
            mesh.animateAndRender(elapsedTime);
        }

        public void DisposePersonaje()
        {
            mesh.Dispose();
        }

        public TGCVector3 PosicionMesh()
        {
            return mesh.Position;
        }

        public void animarPersonaje(bool caminar)
        {
            if(caminar)
            {
                mesh.stopAnimation();
                mesh.playAnimation("Walk", true);
            }
            else
            {
                mesh.stopAnimation();
                mesh.playAnimation("StandBy", true);
            }
        }

        public void MoverPersonaje(char key, float elapsedTime)
        {
            var movimiento = TGCVector3.Empty;
            var posicionOriginal = mesh.Position;

            switch (key)
            {
                case 'W':
                    movimiento.Z = -1;
                    break;

                case 'A':
                    movimiento.X = 1;
                    break;

                case 'S':
                    movimiento.Z = 1;
                    break;

                case 'D':
                    movimiento.X = -1;
                    break;
            }

            movimiento *= velocidad_movimiento * elapsedTime;
            mesh.Position = mesh.Position + movimiento;
            mesh.Transform = TGCMatrix.Translation(mesh.Position);
        }
    }
}
