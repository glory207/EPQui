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
using System.Windows.Controls.Primitives;
using System.IO;

namespace EPQui
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // GameWindow window;
         TransformEditor traEP = new TransformEditor();
         TransformEditor traER = new TransformEditor();
         TransformEditor traES = new TransformEditor();

        public MainWindow()
        {
            InitializeComponent();
            scene = new Hierarchy();
            window.Loaded += Window_Loaded2;
            window.SampleEvent += Window_SampleEvent;
            traEP.PropertyChanged += TraE_PropertyChanged;
            traER.PropertyChanged += TraE_PropertyChanged;
            traES.PropertyChanged += TraE_PropertyChanged;
            theList.Children.Add(traEP);
            theList.Children.Add(traER);
            theList.Children.Add(traES);

            String[] meshDir = Directory.GetFiles("Res/meshes/","*.obj");
            ObjectSelector buttonL = new ObjectSelector();
            buttonL.textBlk.Text = "light";
            buttonL.MouseDown += Button_Click3;

            wrpPan.Children.Add(buttonL);
            foreach (string str in meshDir)
            {
                ObjectSelector button = new ObjectSelector();
                button.Tag = str;
                button.textBlk.Text = str.Substring(11, str.Substring(11).Length-4);
                button.MouseDown += Button_Click2;

                wrpPan.Children.Add(button);
            }
        }

        private void TraE_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (scene.Hobj[window.selectedObjj].GetType() == typeof(LightContainer))
            {
                scene.Hobj[window.selectedObjj].Position = traEP.BoundVector;
                scene.Hobj[window.selectedObjj].lightColor.Xyz = traER.BoundVector;
                scene.Hobj[window.selectedObjj].objectScale = traES.BoundVector;
            }
            else if (scene.Hobj[window.selectedObjj].GetType() == typeof(MeshContainer))
            {
                scene.Hobj[window.selectedObjj].Position = traEP.BoundVector;
                scene.Hobj[window.selectedObjj].objectRotation = traER.BoundVector;
                scene.Hobj[window.selectedObjj].objectScale = traES.BoundVector;
            }
        }
        private void Window_SampleEvent()
        {
            if (window.selectedObjj >= 0 && scene.Hobj.Count > 0)
            {

                if (scene.Hobj[window.selectedObjj].GetType() == typeof(LightContainer))
                {

                    traEP.BoundVector = scene.Hobj[window.selectedObjj].Position;
                    traER.BoundVector = scene.Hobj[window.selectedObjj].lightColor.Xyz;
                    traES.BoundVector = scene.Hobj[window.selectedObjj].objectScale;
                    traEP.BoundName = "Position";
                    traER.BoundName = "lightColor";
                    traES.BoundName = "objectScale";
                }
                else if (scene.Hobj[window.selectedObjj].GetType() == typeof(MeshContainer))
                {


                    traEP.BoundVector = scene.Hobj[window.selectedObjj].Position;
                    traER.BoundVector = scene.Hobj[window.selectedObjj].objectRotation;
                    traES.BoundVector = scene.Hobj[window.selectedObjj].objectScale;
                    traEP.BoundName = "Position";
                    traER.BoundName = "objectRotation";
                    traES.BoundName = "objectScale";
                }
            }
        }
        Hierarchy scene;
        private void Window_Loaded2(object sender, RoutedEventArgs e)
        {
            ((VeiwPortDisplay)sender).scene = scene;

            scene.Hobj = new List<HierObj>()
             {
                 new LightContainer(new Vector3(-1.0f, 2.8f, 0.8f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f)),
                 new LightContainer(new Vector3(1.0f, 2.8f, 0.8f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f)),
                 new LightContainer(new Vector3(0.0f, 2.8f, 0.8f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)),
             };
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".obj"; // Default file extension
            dialog.Filter = "Text documents (.obj)|*.obj"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;

                Button button = new Button();
                button.Tag = filename;
                button.Height = 50;
                button.Width = 50;
                button.Click += Button_Click2;
                
                wrpPan.Children.Add(button);
            }

        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            scene.AddMesh(new MeshContainer(Vector3.Zero, (string)((FrameworkElement)sender).Tag));

        }
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            scene.AddMesh(new LightContainer(window.camera.Position,new Vector4(1,1,1,1)));

        }
        private void deleteObj(object sender, RoutedEventArgs e)
        {
            scene.DeleteObj(window.selectedObjj);
            window.selectedObjj = -1;
        }
    }


}
