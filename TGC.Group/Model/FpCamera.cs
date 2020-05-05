using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.DirectInput;
using System;
using System.Drawing;
using System.Windows.Forms;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.BoundingVolumes;
using Device = Microsoft.DirectX.Direct3D.Device;

namespace TGC.Group.Model
{
    public class FpCamera : TgcCamera
    {
        public bool Enable { get; set; }

        /// <summary>
        /// Hacia donde es X, desde la perspectiva de la camara.
        /// </summary>
        TGCVector3 xAxis;

        /// <summary>
        /// Hacia donde es Y, desde la perspectiva de la camara.
        /// </summary>
        TGCVector3 yAxis;

        /// <summary>
        /// Hacia donde es Z, desde la perspectiva de la camara.
        /// </summary>
        TGCVector3 zAxis;

        /// <summary>
        /// Hacia adonde es "adelante" (Z+ desde la perspectiva del que camina).
        /// </summary>
        TGCVector3 forward;

        /// <summary>
        /// Hacia donde mira la camara, desde los ejes del mundo.
        /// </summary>
        TGCVector3 target;

        /// <summary>
        /// Donde esta la camara, desde los ejes del mundo.
        /// </summary>
        TGCVector3 eye;

        /// <summary>
        /// Hacia donde es arriba, desde los ejes del mundo.
        /// </summary>
        readonly TGCVector3 up = new TGCVector3(0.0f, 1.0f, 0.0f);

        /// <summary>
        /// Matriz de vista
        /// </summary>
        TGCMatrix vM;

        /// <summary>
        /// true si la posicion cambio desde el ultimo render.
        /// </summary>
        bool positionChanged;

        /// <summary>
        /// true si la direccion de vista cambio desde el ultimo render.
        /// </summary>
        bool rotationChanged;

        /// <summary>
        /// La rotacion total sobre el eje x.
        /// </summary>
        float absoluteRotationX;

        public float MovementSpeed { get; set; }

        /// <summary>
        /// Cuánto mas rapido puede ir cuando camina hacia adelante.
        /// </summary>
        public float ForwardFactor { get; set; }

        /// <summary>
        /// La velocidad actual de la camara.
        /// </summary>
        public float CurrentSpeed { get; set; }

        float rotationSpeed;

        /// <summary>
        /// Velocidad de rotacion, en grados / ( segundo * pix ).
        /// </summary>
        public float RotationSpeed
        {
            set { rotationSpeed = FastMath.ToRad(value); }
            get { return FastMath.ToDeg(rotationSpeed); }
        }

        float maxTopAngle;
        float maxBottomAngle;

        /// <summary>
        /// Establece el maximo angulo de rotacion X (pitch) hacia arriba, en grados.
        /// </summary>
        public float MaxTopAngle
        {
            set { maxTopAngle = FastMath.ToRad(value); }
            get { return FastMath.ToDeg(maxTopAngle); }
        }

        /// <summary>
        /// Establece el maximo angulo de rotacion X (pitch) hacia abajo, en grados.
        /// </summary>
        public float MaxBottomAngle
        {
            set { maxBottomAngle = FastMath.ToRad(value); }
            get { return FastMath.ToDeg(maxBottomAngle); }
        }

        /// <summary>
        /// true si el mouse esta actualmente capturado por la camara.
        /// </summary>
        private bool lockMouse;

        /// <summary>
        /// Centro de la ventana actual, en coordenadas de la pantalla.
        /// </summary>
        private Point windowCenter;

        /// <summary>
        /// El sonido usado para caminar.
        /// </summary>
        //public TgcStaticSound MovementSound { get; set; }

        private TgcBoundingAxisAlignBox boundingBox;

        /// <summary>
        /// Retorna la AABB de la posicion actual.
        /// </summary>
        public TgcBoundingAxisAlignBox BoundingBox
        {
            get { return boundingBox; }
        }

        /// <summary>
        /// Controla la captura del mouse.
        /// </summary>
        public bool LockMouse
        {
            set
            {
                lockMouse = value;

                if (lockMouse)
                    Cursor.Hide();

                else
                    Cursor.Show();

            }

            get { return lockMouse; }
        }

        public FpCamera()
        {
            positionChanged = true;
            rotationChanged = true;

            target = new TGCVector3(100f, 15f, 150f);
            eye = new TGCVector3(100f, 15f, 100f);

            // \todo: configurable
            float half_box = 4.0f;
            boundingBox = new TgcBoundingAxisAlignBox(
                new TGCVector3(eye.X - half_box, 0.0f, eye.Z - half_box),
                new TGCVector3(eye.X + half_box, eye.Y, eye.Z + half_box));

            vM = TGCMatrix.Identity;

            xAxis = new TGCVector3();
            yAxis = new TGCVector3();
            zAxis = new TGCVector3();
            forward = new TGCVector3();

            absoluteRotationX = 0.0f;

            MovementSpeed = 25.0f;
            ForwardFactor = 1.5f;
            RotationSpeed = 3.0f;
            MaxTopAngle = 88.0f;
            MaxBottomAngle = -80.0f;
            CurrentSpeed = MovementSpeed;

            Control window = D3DDevice.Instance.Device.CreationParameters.FocusWindow;

            windowCenter = window.PointToScreen(
                new Point(window.Width / 2, window.Height / 2));

            lockMouse = false;

            Enable = true;

            setCamera(eye, target);
        }

        public TGCVector3 getPosition()
        {
            return eye;
        }

        /// <returns>
        /// Retorna el vector donde mira la camara (a una distancia
        /// de 1.0f del ojo), en relacion al mundo.
        /// </returns>
        public TGCVector3 getLookAt()
        {
            return target;
        }

        /// <summary>
        /// Actualizar el estado interno de la cámara en cada frame
        /// </summary>
        public void updateCamera(float elapsedTime, TgcD3dInput input)
        {
            if (!Enable)
                return;

            if (input.keyPressed(Key.L))
                LockMouse = !LockMouse;

            if (!LockMouse)
                return;

            // posicion
            //
            bool moved = false;
            TGCVector3 movement = new TGCVector3(0.0f, 0.0f, 0.0f);

            if (input.keyDown(Key.W))
            {
                movement += forward * (MovementSpeed * elapsedTime * ForwardFactor);
                moved = true;
            }

            if (input.keyDown(Key.A))
            {
                movement += xAxis * (-MovementSpeed * elapsedTime);
                moved = true;
            }

            if (input.keyDown(Key.S))
            {
                movement += forward * (-MovementSpeed * elapsedTime);
                moved = true;
            }

            if (input.keyDown(Key.D))
            {
                movement += xAxis * (MovementSpeed * elapsedTime);
                moved = true;
            }

           /* if (moved)
            {
                move(movement);

                MovementSound.play();
            }
            */
            // rotacion
            //

            // invertidos: moverse en x cambia el heading (rotacion sobre y) y viceversa.
            float rotY = input.XposRelative * rotationSpeed;
            float rotX = input.YposRelative * rotationSpeed;

            if (rotY != 0.0f || rotX != 0.0f)
                look(rotX, rotY);

            if (lockMouse)
                Cursor.Position = windowCenter;

        }

        /// <summary>
        /// Rota en los deltas indicados.
        /// </summary>
        /// <param name="rotX"></param>
        /// <param name="rotY"></param>
        private void look(float rotX, float rotY)
        {
            // controlar los limites de rotacion sobre X (pitch)
            //

            absoluteRotationX += rotX;

            if (absoluteRotationX > maxTopAngle)
            {
                rotX = maxTopAngle - (absoluteRotationX - rotX);
                absoluteRotationX = maxTopAngle;
            }
            else if (absoluteRotationX < maxBottomAngle)
            {
                rotX = maxBottomAngle - (absoluteRotationX - rotX);
                absoluteRotationX = maxBottomAngle;
            }

            // rotar la camara
            //

            // \todo optimize ?
            TGCMatrix deltaRM =
                TGCMatrix.RotationAxis(xAxis, rotX) *
                TGCMatrix.RotationAxis(up, rotY);

            TGCVector4 result;

            result = TGCVector3.Transform(xAxis, deltaRM);
            xAxis = new TGCVector3(result.X, result.Y, result.Z);

            result = TGCVector3.Transform(yAxis, deltaRM);
            yAxis = new TGCVector3(result.X, result.Y, result.Z);

            result = TGCVector3.Transform(zAxis, deltaRM);
            zAxis = new TGCVector3(result.X, result.Y, result.Z);

            // recalcular las dependencias
            //

            forward = TGCVector3.Cross(xAxis, up);
            forward.Normalize();

            target = eye + zAxis;

            rotationChanged = true;
        }

        public void setCamera(TGCVector3 eye, TGCVector3 target)
        {
            this.eye = eye;
            this.target = target;

            zAxis = target - eye;
            zAxis.Normalize();

            xAxis = TGCVector3.Cross(up, zAxis);
            xAxis.Normalize();

            yAxis = TGCVector3.Cross(zAxis, xAxis);
            yAxis.Normalize();

            forward = TGCVector3.Cross(xAxis, up);
            forward.Normalize();

            rotationChanged = true;
            positionChanged = true;
        }

        /// <summary>
        /// Entrega la matriz de vista a D3D.
        /// </summary>
        public void updateViewMatrix(Microsoft.DirectX.Direct3D.Device d3dDevice)
        {
            if (!Enable)
                return;

            rebuildViewMatrix();

            d3dDevice.Transform.View = vM;
        }

        public void move(TGCVector3 delta)
        {
            eye += delta;
            target += delta;

            boundingBox.move(delta);

            positionChanged = true;
        }

        /// <summary>
        /// Actualiza la matriz de vista, solo lo que sea necesario.
        /// </summary>
        void rebuildViewMatrix()
        {
            if (rotationChanged)
                goto Rotation;
            else if (positionChanged)
                goto Position;
            else
                return;

            Rotation:
            vM.M11 = xAxis.X; vM.M12 = yAxis.X; vM.M13 = zAxis.X; // (1,4) = 0
            vM.M21 = xAxis.Y; vM.M22 = yAxis.Y; vM.M23 = zAxis.Y; // (2,4) = 0
            vM.M31 = xAxis.Z; vM.M32 = yAxis.Z; vM.M33 = zAxis.Z; // (3,4) = 0

            rotationChanged = false;

        Position:
            vM.M41 = -TGCVector3.Dot(xAxis, eye);
            vM.M42 = -TGCVector3.Dot(yAxis, eye);
            vM.M43 = -TGCVector3.Dot(zAxis, eye);
            // (4,4) = 1

            positionChanged = false;
        }

        public void Dispose()
        {
           // MovementSound.dispose();
        }
    }
}


