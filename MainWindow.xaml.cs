using System; using OpenTK.Graphics.OpenGL4;
using System.Windows;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Timers;
using Timer = System.Timers.Timer;
using OpenTK.Wpf;
using System.Reflection;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK;
using System.Diagnostics;
using Key = System.Windows.Input.Key;
using System.Windows.Input;
using OpenTK.Windowing.Common.Input;
using Quaternion = OpenTK.Mathematics.Quaternion;
using EPQui.UserCon;

namespace EPQui
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        int SCR_WIDTH;
        int SCR_HEIGHT;
        bool upCam = false;
        Hierarchy scene;
        Camera camera;
        Shader sd;
        int meshPointer = 0;
        Shader gridshaderProgram, gridshaderProgram2;
        Mesh gridMesh;
        Timer Timer = new Timer() { Interval = 10 };
        float speed = 0.05f;
        int hoverObj;
        int selectedObj = 0;
        // GameWindow window;
        TransformEditor traE = new TransformEditor();
        public MainWindow()
        {
            InitializeComponent();
            var settings = new GLWpfControlSettings
            {
                MajorVersion = 4,
                MinorVersion = 6
            };
            window.Start(settings);
            window.Loaded += Window_Loaded1; ;
           // Window_Loaded();
            window.Unloaded += Window_Unloaded;
            
        }

        private void Window_Loaded1(object sender, RoutedEventArgs e)
        {
            window.Render += Window_Render;
            window.SizeChanged += Window_SizeChanged;
            window.MouseMove += Window_MouseMove;
            window.KeyDown += Grid_KeyDown;
            window.KeyUp += Grid_KeyUp;
            SCR_WIDTH = (int)window.ActualWidth;
            SCR_HEIGHT = (int)window.ActualHeight;
            camera = new Camera(SCR_WIDTH, SCR_HEIGHT, new Vector3(0.0f, 0.0f, 0.0f));
            gridMesh = new Mesh();
            scene = new Hierarchy();
            gridshaderProgram = new Shader("Res/grid.vert", "Res/grid.frag", "Res/grid.geomertry");
            gridshaderProgram2 = new Shader("Res/grid.vert", "Res/Clicks.frag", "Res/grid.geomertry");
            
            theList.Children.Clear();
            for (int i = 0;i < scene.Hobj.Count; i++)
            {
                HyrachyMesh hyrachyMesh = new HyrachyMesh();
                hyrachyMesh.BoundText = scene.Hobj[i].mesh.name;
                theList.Children.Add(hyrachyMesh);
            }
            theList.Children.Add(traE);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            GL.FrontFace(FrontFaceDirection.Cw);

            Debug.WriteLine(SCR_WIDTH.ToString());
        }
        Vector2 mouseA, mouseP, mouseS = new Vector2(0.005f);
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Vector2 mouseD;
            mouseD = (mouseP - new Vector2((float)e.GetPosition(window).X, (float)e.GetPosition(window).Y)) * new Vector2(-1,1);
            mouseP = new Vector2((float)e.GetPosition(window).X, (float)e.GetPosition(window).Y);

            

            if (mouseR)  mouseA += mouseD * mouseS;
            if (mouseA.Y <= -1.5f) mouseA.Y = -1.5f;
            if (mouseA.Y >= 1.5f) mouseA.Y = 1.5f;
        }

        private void Window_SizeChanged(object senderr, SizeChangedEventArgs e)
        {
            SCR_WIDTH = (int)window.ActualWidth;
            SCR_HEIGHT = (int)window.ActualHeight;
            upCam = true;
            camera.updateScreenSize(SCR_WIDTH, SCR_HEIGHT);
            
        }
        private void Window_Render(TimeSpan obj)
        {
            update();

            camera.updateMatrix(45.0f, 0.1f, 100.0f, SCR_WIDTH, SCR_HEIGHT);
            scene.Update(camera);
            gridshaderProgram.Activate();
            Matrix4 mat = Matrix4.Identity;
            GL.UniformMatrix4(GL.GetUniformLocation(gridshaderProgram.ID, "model"), false, ref mat);
            GL.Uniform4(GL.GetUniformLocation(gridshaderProgram.ID, "lightColor"), 1, 1, 1, 1);
            GL.Uniform3(GL.GetUniformLocation(gridshaderProgram.ID, "camUp"), camera.Orientation);
            gridMesh.Draw(gridshaderProgram, camera);
            camera.update();
            scene.UpdateClick(camera);
            hoverObj = camera.updateClicks((int)mouseP.X, SCR_HEIGHT - (int)mouseP.Y) -1;

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

            gridshaderProgram.Delete();
            camera.destroy();

            scene.destroy();
        }

        bool up,down,left, right,shift,space,mouseR;

        private void window_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mouseR = false;
        }

        private void window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mouseR = true;
            if (hoverObj >= 0) { selectedObj = hoverObj;
               traE.BoundTextX = scene.Hobj[selectedObj].Position.X;
               traE.BoundTextY = scene.Hobj[selectedObj].Position.Y;
                traE.BoundTextZ = scene.Hobj[selectedObj].Position.Z;
            }
        }

        private void Grid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    up = false;
                    break;
                case Key.A:
                    left = false;
                    break;
                case Key.S:
                    down = false;
                    break;
                case Key.D:
                    right = false;
                    break;
                case Key.Space:
                    space = false;
                    break;
                case Key.LeftShift:
                    shift = false;
                    break;
            }
        }

        private void Grid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    up = true;
                    break;
                case Key.A:
                    left = true;
                    break;
                case Key.S:
                    down = true;
                    break;
                case Key.D:
                    right = true;
                    break;
                case Key.Space:
                    space = true;
                    break;
                case Key.LeftShift:
                    shift = true;
                    break;
            }
        }


        void update()
        {
            

            scene.Hobj[selectedObj].Position.X = traE.BoundTextX;
            scene.Hobj[selectedObj].Position.Y = traE.BoundTextY;
            scene.Hobj[selectedObj].Position.Z = traE.BoundTextZ;

           // if (mouseR)
            {
                if (up) camera.Position += camera.Orientation * speed;
                if (down) camera.Position += -camera.Orientation * speed;
                if (left) camera.Position += -camera.OrientationR * speed;
                if (right) camera.Position += camera.OrientationR * speed;
                if (shift) camera.Position += -camera.OrientationU * speed;
                if (space) camera.Position += camera.OrientationU * speed;


                camera.Orientation = Matrix3.CreateFromAxisAngle(new Vector3(0, 1, 0), mouseA.X) * new Vector3(1, 0, 0);
                camera.OrientationU = Matrix3.CreateFromAxisAngle(new Vector3(0, 1, 0), mouseA.X) * new Vector3(0, -1, 0);
                camera.OrientationR = Matrix3.CreateFromAxisAngle(new Vector3(0, 1, 0), mouseA.X) * new Vector3(0, 0, 1);
                camera.Orientation = Matrix3.CreateFromAxisAngle(camera.OrientationR, mouseA.Y) * -camera.Orientation;
                camera.OrientationU = Matrix3.CreateFromAxisAngle(camera.OrientationR, mouseA.Y) * -camera.OrientationU;
                camera.OrientationR = Matrix3.CreateFromAxisAngle(camera.OrientationR, mouseA.Y) * -camera.OrientationR;
            }
        }
    }


}
