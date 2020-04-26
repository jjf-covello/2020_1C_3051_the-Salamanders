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
        String MediaDir = "C:\\Users\\tinch\\Documents\\.GITHUB\\2020_1C_3051_the-Salamanders\\TGC.Group\\Media\\";
        private TgcScene tgcScene { get; set; }
        private TgcSimpleTerrain heightmap = new TgcSimpleTerrain();
        string currentHeightmap;
        string currentTexture;
        float currentScaleXZ;
        float currentScaleY;
        public void InstanciarEstructuras() //va en el init()
        {
            TgcSceneLoader loader = new TgcSceneLoader();
            tgcScene = loader.loadSceneFromFile(MediaDir + "EscenarioPortal\\EscenarioPortal-TgcScene.xml");
        }

        public void InstanciarHeightmap() //va en el init()
        {
            currentScaleXZ = 600f;
            currentScaleY = 301f;

            currentHeightmap = MediaDir + "Heighmaps\\heightmapUsarEsteDeBase.jpg";
            currentTexture = MediaDir + "Heighmaps\\Grass.jpg";

            heightmap.loadHeightmap(currentHeightmap, currentScaleXZ, currentScaleY, new TGCVector3(0, -1, 0));
            heightmap.loadTexture(currentTexture);
        }

        public void RenderEscenario()
        {
            tgcScene.Meshes.ForEach(mesh => mesh.UpdateMeshTransform());
            tgcScene.Meshes.ForEach(mesh => mesh.Render());

            heightmap.Render();
        }

        public void DisposeEscenario()
        {
            heightmap.Dispose();
            tgcScene.Meshes.ForEach(mesh => mesh.Dispose());
        }
    }
}

