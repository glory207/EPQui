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
        public MainWindow()
        {
            InitializeComponent();


            scene = new Hierarchy();
            window.Loaded += Window_Loaded2;
            window.SampleEvent += Window_SampleEvent;
            
            string[] meshDir = Directory.GetFiles("Res/meshes/", "*.obj");
            ObjectSelector buttonL = new ObjectSelector();
            buttonL.textBlk.Text = "light";
            buttonL.MouseDown += Button_Click3;

            wrpPan.Children.Add(buttonL);
            foreach (string str in meshDir)
            {
                ObjectSelector button = new ObjectSelector();
                button.Tag = str;
                button.textBlk.Text = str.Substring(11, str.Substring(11).Length - 4);
                button.MouseDown += Button_Click2;

                wrpPan.Children.Add(button);
            }
            
        }
        TransformEditor traE;
        MaterialEditor matE;
        LightEditor ligE;
        private void Window_SampleEvent()
        {
            theList2.Children.Clear();
            if (window.selectedObjj >= 0 && scene.children.Count > 0)
            {
                
                traE = new TransformEditor();
                traE.deleted += TraE_deleted;
                theList2.Children.Add(traE);
                traE.set(scene.children[window.selectedObjj]);
                if (scene.children[window.selectedObjj].GetType() == typeof(MeshContainer))
                {

                    matE = new MaterialEditor();
                    theList2.Children.Add(matE);
                    matE.set(((MeshContainer)scene.children[window.selectedObjj]).mate);
                }
                else if (scene.children[window.selectedObjj].GetType() == typeof(LightContainer))
                {
                    ligE = new LightEditor();
                    theList2.Children.Add(ligE);
                    ligE.set(((LightContainer)scene.children[window.selectedObjj]));
                }
            }
        }

        private void TraE_deleted()
        {
            theList2.Children.Clear();
            window.selectedObjj = -1;
        }

        Hierarchy scene;
        private void Window_Loaded2(object sender, RoutedEventArgs e)
        {
            ((VeiwPortDisplay)sender).scene = scene;

            scene.children = new List<HierObj>()
             {
                 new LightContainer(new Vector3(0.0f, 2.8f, 0.8f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f),scene){Type = LightType.point},
                 new MeshContainer(new Vector3(0.0f, 0.0f, 0.0f), "Res/meshes/cube.obj",scene){objectScale = new Vector3(50,0.05f,50), mate = new material(){texScale = new Vector2(50)}},
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

            scene.children.Add(new MeshContainer(Vector3.Zero, (string)((FrameworkElement)sender).Tag, scene));

        }
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            scene.children.Add(new LightContainer(window.camera.Position, new Vector4(1, 1, 1, 1), scene));

        }
        
    }


}
