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
using System.Collections.Generic;

namespace EPQui
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // GameWindow window;
         TransformEditor traE = new TransformEditor();
        public MainWindow()
        {
            InitializeComponent();
            scene = new Hierarchy();
            window.Loaded += Window_Loaded2;
            window.SampleEvent += Window_SampleEvent;
            traE.PropertyChanged += TraE_PropertyChanged;
            theList.Children.Add(traE);
        }

        private void TraE_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            scene.Hobj[window.selectedObjj].Position = traE.BoundVector;
        }
        private void Window_SampleEvent()
        {
            traE.BoundVector = scene.Hobj[window.selectedObjj].Position;
        }
        Hierarchy scene;
        private void Window_Loaded2(object sender, RoutedEventArgs e)
        {
            ((VeiwPortDisplay)sender).scene = scene;

            scene.Hobj = new List<HierObj>()
             {
                 new MeshContainer(new Vector3(0,0,0), "Res/Cube.txt"),
                 new LightContainer(new Vector3(-1.0f, 2.8f, 0.8f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f)),
                 new LightContainer(new Vector3(1.0f, 2.8f, 0.8f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f)),
                 new LightContainer(new Vector3(0.0f, 2.8f, 0.8f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)),
             };
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            scene.AddMesh(new LightContainer(window.camera.Position,new Vector4(1)));
        }
    }


}
