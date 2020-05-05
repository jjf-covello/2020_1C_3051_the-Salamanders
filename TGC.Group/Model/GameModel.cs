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
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar el modelo que instancia GameForm <see cref="Form.GameForm.InitGraphics()" />
    ///     line 97.
    /// </summary>
    public class GameModel : TgcExample
    {
        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        /// <param name="mediaDir">Ruta donde esta la carpeta con los assets</param>
        /// <param name="shadersDir">Ruta donde esta la carpeta con los shaders</param>
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }
        Escenario escenario = new Escenario();
        Personaje personaje = new Personaje();
      
        //Caja que se muestra en el ejemplo.
        private TGCBox Box { get; set; }

        //TgcRotationalCamera CamaraRotacional { get; set; }
        //Mesh de TgcLogo.
        private TgcMesh Mesh { get; set; }
        private TgcMesh fondo { get; set; }
        private TgcScene tgcScene { get; set; }
        //Boleano para ver si dibujamos el boundingbox
        private bool BoundingBox { get; set; }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, estructuras de optimización, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>
        public override void Init()
        {
            //Device de DirectX para crear primitivas.
            var d3dDevice = D3DDevice.Instance.Device;
            
            escenario.InstanciarEstructuras();
            escenario.InstanciarHeightmap();
            escenario.InstanciarSkyBox();
            personaje.InstanciarPersonaje();

            /*
            var cameraPosition = new TGCVector3(-2500, 0, -15000);
            var lookAt = new TGCVector3(0, 0, 0);
            Camara.SetCamera(cameraPosition, lookAt);
            */

            //TODO: Hacer que la camara siga a la bounding box
            MiCamara camaraInterna = new MiCamara(personaje.PosicionMesh(), 220, 300);
            Camara = camaraInterna;


            //Internamente el framework construye la matriz de view con estos dos vectores.
            //Luego en nuestro juego tendremos que crear una cámara que cambie la matriz de view con variables como movimientos o animaciones de escenas

        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();
            bool caminar = false;
            //Capturar Input teclado
            if (Input.keyPressed(Key.F))
            {
                BoundingBox = !BoundingBox;
            }

            if (Input.keyDown(Key.W))
            {
                //Le digo al wachin que vaya para adelante
                personaje.MoverPersonaje('W', ElapsedTime);
                caminar = true;
            }

            if (Input.keyDown(Key.A))
            {
                //Le digo al wachin que vaya para la izquierda
                personaje.MoverPersonaje('A', ElapsedTime);
                caminar = true;
            }

            if (Input.keyDown(Key.S))
            {
                //Le digo al wachin que vaya a para atras
                personaje.MoverPersonaje('S', ElapsedTime);
                caminar = true;
            }

            if (Input.keyDown(Key.D))
            {
                //Le digo al wachin que vaya para la derecha
                personaje.MoverPersonaje('D', ElapsedTime);
                caminar = true;
            }

            personaje.animarPersonaje(caminar);

         
            //camaraInterna.updateCamera(ElapsedTime, Input);

            //Capturar Input Mouse
            /*
            if (Input.buttonUp(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                //Como ejemplo podemos hacer un movimiento simple de la cámara.
                //En este caso le sumamos un valor en Y
                Camara.SetCamera(Camara.Position + new TGCVector3(200f, 100f, 200f), Camara.LookAt);
                //Ver ejemplos de cámara para otras operaciones posibles.

                //Si superamos cierto Y volvemos a la posición original.
                if (Camara.Position.Y > 3000f)
                {
                    Camara.SetCamera(new TGCVector3(Camara.Position.X, 0f, Camara.Position.Z), Camara.LookAt);
                }
            }
            */
            PostUpdate();
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            PreRender();

            escenario.RenderEscenario();
            personaje.RenderPersonaje(ElapsedTime);

            //Render de BoundingBox, muy útil para debug de colisiones.
            if (BoundingBox)
            {
                Box.BoundingBox.Render();
                tgcScene.Meshes.ForEach(mesh => mesh.BoundingBox.Render());
                //fondo.BoundingBox.Render();
            }

            //Finaliza el render y presenta en pantalla, al igual que el preRender se debe para casos puntuales es mejor utilizar a mano las operaciones de EndScene y PresentScene
            PostRender();
        }

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            escenario.DisposeEscenario();
            personaje.DisposePersonaje();
        }
    }
}