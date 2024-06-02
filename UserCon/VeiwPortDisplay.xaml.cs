using System;
using OpenTK.Graphics.OpenGL4;
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
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

namespace EPQui.UserCon
{
    /// <summary>
    /// Interaction logic for VeiwPortDisplay.xaml
    /// </summary>
    public partial class VeiwPortDisplay : UserControl
    {
        public VeiwPortDisplay()
        {

            InitializeComponent();
            var settings = new GLWpfControlSettings
            {
                MajorVersion = 4,
                MinorVersion = 6
            };
            window.Start(settings);

            
            window.Loaded += Window_Loaded1;
            window.Unloaded += Window_Unloaded;



        }

        public Gyzmo gyzmo;
        public int SCR_WIDTH;
        public int SCR_HEIGHT;
        public Hierarchy scene;
        public Camera camera;
        public float speed;
        public int hoverObj;
        public int editObjHover;
        public int editObj;
        public int selectedObjj = 0;

        public delegate void SampleEventHandler();
        public event SampleEventHandler SampleEvent;
        private void Window_Loaded1(object sender, RoutedEventArgs e)
        {
            
            window.Render += Window_Render;
            window.SizeChanged += Window_SizeChanged;
            window.MouseMove += Window_MouseMove;
            window.KeyDown += Grid_KeyDown;
            window.KeyUp += Grid_KeyUp;
            window.MouseDown += window_MouseDown;
            window.MouseUp += window_MouseUp;
            window.MouseLeave += Window_MouseLeave;
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            GL.FrontFace(FrontFaceDirection.Cw);

            SCR_WIDTH = (int)window.ActualWidth;
            SCR_HEIGHT = (int)window.ActualHeight;
            camera = new Camera(SCR_WIDTH, SCR_HEIGHT, new Vector3(4.0f, 3.0f, 0.0f), window.Framebuffer);
           // scene = new Hierarchy();
            this.Focusable = true;

            gyzmo = new Gyzmo();

        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseR = false;
        }

        Vector2 mouseD, mouseA, mouseP, mouseS = new Vector2(0.005f);

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mouseD = (mouseP - new Vector2((float)e.GetPosition(window).X, (float)e.GetPosition(window).Y)) * new Vector2(-1, 1);
            mouseP = new Vector2((float)e.GetPosition(window).X, (float)e.GetPosition(window).Y);



            if (mouseR) mouseA += mouseD * mouseS;
            if (mouseA.Y <= -1.5f) mouseA.Y = -1.5f;
            if (mouseA.Y >= 1.5f) mouseA.Y = 1.5f;

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SCR_WIDTH = (int)window.ActualWidth;
            SCR_HEIGHT = (int)window.ActualHeight;
            camera.updateScreenSize(SCR_WIDTH, SCR_HEIGHT);

        }
        int frameCount;
        float elapsTime;
        private float fps;
        private float delta;
        private void Window_Render(TimeSpan obj)
        {
            frameCount++;
            delta = (float)obj.TotalSeconds;
            elapsTime += delta;
            if (elapsTime > 0.2f)
            {
            fps = frameCount / elapsTime;
            frameCount = 0;
            elapsTime = 0;
            FPStxt.Text ="FPS: " + ((int)fps).ToString();
            }
            update();
            camera.updateMatrix(45.0f, 0.1f, 100.0f, SCR_WIDTH, SCR_HEIGHT);
            List<LightContainer> lights = new List<LightContainer>();


            camera.frameC.Clear();
            scene.UpdateClick(camera, 0, 0);
            hoverObj = camera.frameC.update((int)mouseP.X, SCR_HEIGHT - (int)mouseP.Y);

            camera.frameE.Clear();
            if (selectedObjj >= 0 && scene.children.Count > 0)
            {
                GL.Clear(ClearBufferMask.DepthBufferBit);
                GL.Disable(EnableCap.CullFace);
                gyzmo.UpdateClick(camera, scene.children[selectedObjj]);
                GL.Enable(EnableCap.CullFace);
            }
            editObjHover = camera.frameE.update((int)mouseP.X, SCR_HEIGHT - (int)mouseP.Y);
            camera.frame.Clear();
            scene.Update(lights, camera);
            if (selectedObjj >= 0 && scene.children.Count > 0)
            {
                GL.Clear(ClearBufferMask.DepthBufferBit);

                GL.Disable(EnableCap.CullFace);
                gyzmo.Update(camera, scene.children[selectedObjj]);
                GL.Enable(EnableCap.CullFace);

            }


            camera.frame.update();




        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            camera.destroy();
            scene.destroy();
        }

        bool up, down, left, right, shift, Ctrl, space, mouseR, mouseL;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (int.Parse(((Button)sender).Tag.ToString()))
            {

                case 1:
                    gyzmo.type = GyzmoType.translation;
                    break;
                case 2:
                    gyzmo.type = GyzmoType.rotation;
                    break;
                case 3:
                    gyzmo.type = GyzmoType.scale;
                    break;
                case 4:
                    ((Button)sender).Content = "Perspective";
                    ((Button)sender).Tag = 5;
                    camera.perspective = false;
                    break;
                case 5:
                    ((Button)sender).Content = "Orthographic";
                    ((Button)sender).Tag = 4;
                    camera.perspective = true;

                    break;
                case 6:
                    scene.save();
                    break;
                case 7:
                    var dialog = new Microsoft.Win32.OpenFileDialog();
                    dialog.FileName = "Scene"; // Default file name
                    dialog.DefaultExt = ".sce"; // Default file extension
                    dialog.Filter = "Text documents (.sce)|*.sce"; // Filter files by extension
                    dialog.CheckFileExists = false;
                    
                    bool? result = dialog.ShowDialog();

                    InitializeComponent();
                    if (result == true)
                    {
                        string filename = dialog.FileName;
                        bool valid = false;
                        do
                        {
                            if (File.Exists(filename)) filename = filename.Substring(0, filename.Length - 4) + "(dupe).sce";
                            else valid = true;
                        } while (valid == false);
                        
                        scene.path = filename;
                        scene.save();
                    }
                    break;
            }
        }

        private void window_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
           switch (e.ChangedButton)
           {
               case System.Windows.Input.MouseButton.Left:
                   mouseL = false;
                   scene.children[selectedObjj].Position += scene.children[selectedObjj].PositionAdded;
                   scene.children[selectedObjj].PositionAdded = Vector3.Zero;
                   scene.children[selectedObjj].objectScale += scene.children[selectedObjj].objectScaleAdded;
                   scene.children[selectedObjj].objectScaleAdded = Vector3.Zero;
                   scene.children[selectedObjj].objectRotation = scene.children[selectedObjj].objectRotationAdded * scene.children[selectedObjj].objectRotation;
                   scene.children[selectedObjj].objectRotationAdded = Quaternion.Identity;
                   editObj = 0;
                   SampleEvent.Invoke();
                   break;
               case System.Windows.Input.MouseButton.Right:
                   mouseR = false;
                   break;
          
           }
        }
        private void window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Focus();
        
           switch (e.ChangedButton)
        {
            case System.Windows.Input.MouseButton.Left:
                mouseL = true;
                if (editObjHover == 0)
                {
                    selectedObjj = hoverObj;
                    SampleEvent.Invoke();
                }
               else
               {
                   editObj = editObjHover;
                   Matrix4 ProjectionInv = camera.projection.Inverted();
            
                   float mouse_x = (float)mouseP.X;
                   float mouse_y = (float)mouseP.Y;
            
                   float ndc_x = (2.0f * mouse_x) / SCR_WIDTH - 1.0f;
                   float ndc_y = 1.0f - (2.0f * mouse_y) / SCR_HEIGHT;
            
                   Vector4 ray_ndc_4d = new Vector4(ndc_x, ndc_y, -1.0f, 1.0f);
                   Vector4 ray_view_4d = ProjectionInv * ray_ndc_4d;
            
            
                   Vector4 view_space_intersect = new Vector4(ray_view_4d.X, ray_view_4d.Y, -1, 1);
            
                   Vector3 point_world = (camera.view * view_space_intersect).Xyz.Normalized();
                   gyzmo.set(editObj, camera, scene.children[selectedObjj], point_world);
            
               }
                break;
            case System.Windows.Input.MouseButton.Right:
                mouseR = true;
                break;
        
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
                case Key.LeftCtrl:
                    Ctrl = false;
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
                case Key.LeftCtrl:
                    Ctrl = true;
                    break;
                case Key.D1:
                    gyzmo.type = GyzmoType.translation;
                    break;
                case Key.D2:
                    gyzmo.type = GyzmoType.rotation;
                    break;
                case Key.D3:
                    gyzmo.type = GyzmoType.scale;
                    break;
                case Key.Y:
                    Texture tex = new Texture("Res/textures/planks.png", "diffuse", 0, PixelFormat.Rgba);
                    break;
            }
        }

        void update()
        {

            if (this.IsFocused)
            {
                if (shift) speed = 10 * delta; else speed = 5 * delta;
                if (up) camera.Position += camera.Orientation * speed;
                if (down) camera.Position += -camera.Orientation * speed;
                if (left) camera.Position += -camera.OrientationR * speed;
                if (right) camera.Position += camera.OrientationR * speed;
                if (Ctrl) camera.Position += -camera.OrientationU * speed;
                if (space) camera.Position += camera.OrientationU * speed;


                camera.Orientation = Matrix3.CreateFromAxisAngle(new Vector3(0, 1, 0), mouseA.X) * new Vector3(1, 0, 0);
                camera.OrientationU = Matrix3.CreateFromAxisAngle(new Vector3(0, 1, 0), mouseA.X) * new Vector3(0, -1, 0);
                camera.OrientationR = Matrix3.CreateFromAxisAngle(new Vector3(0, 1, 0), mouseA.X) * new Vector3(0, 0, 1);
                camera.Orientation = Matrix3.CreateFromAxisAngle(camera.OrientationR, mouseA.Y) * -camera.Orientation;
                camera.OrientationU = Matrix3.CreateFromAxisAngle(camera.OrientationR, mouseA.Y) * -camera.OrientationU;
                camera.OrientationR = Matrix3.CreateFromAxisAngle(camera.OrientationR, mouseA.Y) * -camera.OrientationR;


               if (mouseL && editObj != 0)
               {
                   Matrix4 ProjectionInv = camera.projection.Inverted();
              
                   float mouse_x = (float)mouseP.X;
                   float mouse_y = (float)mouseP.Y;
              
                   float ndc_x = (2.0f * mouse_x) / SCR_WIDTH - 1.0f;
                   float ndc_y = 1.0f - (2.0f * mouse_y) / SCR_HEIGHT;
              
                   Vector4 ray_ndc_4d = new Vector4(ndc_x, ndc_y, -1.0f, 1.0f);
                   Vector4 ray_view_4d = ProjectionInv * ray_ndc_4d;
              
              
                   Vector4 view_space_intersect = new Vector4(ray_view_4d.X, ray_view_4d.Y, -1, 1);
              
                   Vector3 point_world = (camera.view * view_space_intersect).Xyz.Normalized();
                   gyzmo.edit(editObj, camera, scene.children[selectedObjj], point_world);
               }
            }
        }
    }
}
