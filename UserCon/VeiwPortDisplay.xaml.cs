﻿using System;
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

            camera = new Camera(SCR_WIDTH, SCR_HEIGHT, new Vector3(0.0f, 0.0f, 0.0f));
            scene = new Hierarchy();
            window.Loaded += Window_Loaded1; 
            window.Unloaded += Window_Unloaded;
            
        }
       public int SCR_WIDTH;
       public int SCR_HEIGHT;
       public Hierarchy scene;
       public Camera camera;
       public float speed = 0.05f;
       public int hoverObj;
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
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            GL.FrontFace(FrontFaceDirection.Cw);

            SCR_WIDTH = (int)window.ActualWidth;
            SCR_HEIGHT = (int)window.ActualHeight;
            camera.updateScreenSize(SCR_WIDTH, SCR_HEIGHT);
        }


        Vector2 mouseA, mouseP, mouseS = new Vector2(0.005f);
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Vector2 mouseD;
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
        private void Window_Render(TimeSpan obj)
        {
            
            update();
            camera.updateMatrix(45.0f, 0.1f, 100.0f, SCR_WIDTH, SCR_HEIGHT);

            
            camera.frameC.Clear();
            scene.UpdateClick(camera);
            hoverObj = camera.update((int)mouseP.X, SCR_HEIGHT - (int)mouseP.Y, camera.frameC);


            camera.frame.Clear();
            scene.Update(camera);
            camera.update(camera.frame);



        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            camera.destroy();
            scene.destroy();
        }

        bool up, down, left, right, shift, space, mouseR;

        private void window_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case System.Windows.Input.MouseButton.Left:
                    
                    break;
                case System.Windows.Input.MouseButton.Right:
                    mouseR = false;
                    break;

            }
        }

        private void window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
            switch (e.ChangedButton)
            {
                case System.Windows.Input.MouseButton.Left:
                    if (hoverObj >= 0)
                    {
                        selectedObjj = hoverObj;
                        SampleEvent.Invoke();
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

            // if (mouseR)
            {
                if (up) { 
                    
                    camera.Position += camera.Orientation * speed;
                }
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