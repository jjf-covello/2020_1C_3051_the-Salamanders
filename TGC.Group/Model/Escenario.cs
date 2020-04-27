using System;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.Camara;
using TGC.Core.Terrain;


namespace TGC.Group.Model
{

    public class Escenario
    {
        String MediaDir = "..\\..\\..\\Media\\";
        private TgcScene tgcScene { get; set; }
        private TgcSimpleTerrain heightmap = new TgcSimpleTerrain();
        private TgcSkyBox skyBox = new TgcSkyBox();
        string currentHeightmap;
        string currentTexture;
        float currentScaleXZ;
        float currentScaleY;

        public void InstanciarEstructuras() //va en el init()
        {
            TgcSceneLoader loader = new TgcSceneLoader();
            tgcScene = loader.loadSceneFromFile(MediaDir + "NuestrosModelos\\esteclaveee-TgcScene.xml");
        }

        public void InstanciarSkyBox()
        {
            string texturesPath = MediaDir + "SkyBoxPiolasa.jpg";

            //Configurar tamaño SkyBox
            skyBox.Center = new TGCVector3(0, 0, 0);
            skyBox.Size = new TGCVector3(10000, 10000, 10000);

            //Configurar las texturas para cada una de las 6 caras
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath);
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath);
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath);
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath);

            //Hay veces es necesario invertir las texturas Front y Back si se pasa de un sistema RightHanded a uno LeftHanded
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath);
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath);

            //Inicializa las configuraciones del skybox
            skyBox.Init();
        }

        public void InstanciarHeightmap() //va en el init()
        {
            currentScaleXZ = 600f;
            currentScaleY = 301f;

            currentHeightmap = MediaDir + "Heighmaps\\heightmapUsarEsteDeBase.jpg";
            currentTexture = MediaDir + "Heighmaps\\Grass.jpg";

            heightmap.loadHeightmap(currentHeightmap, currentScaleXZ, currentScaleY, new TGCVector3(0, -10, 0));
            heightmap.loadTexture(currentTexture);
        }

        public void RenderEscenario()
        {
            tgcScene.Meshes.ForEach(mesh => mesh.UpdateMeshTransform());
            tgcScene.Meshes.ForEach(mesh => mesh.Render());

           // heightmap.Render();

           // skyBox.Render();
        }

        public void DisposeEscenario()
        {
            skyBox.Dispose();
            heightmap.Dispose();
            tgcScene.Meshes.ForEach(mesh => mesh.Dispose());
        }
    }
}

