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
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.IO;
using System.Windows.Media;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using static System.Formats.Asn1.AsnWriter;

namespace EPQui.UserCon
{
    /// <summary>
    /// Interaction logic for TheFullThing.xaml
    /// </summary>
    public partial class TheFullThing : UserControl
    {
        public TheFullThing(string hie)
        {
            InitializeComponent();
            if (hie == "non") scene = new Hierarchy();
            else scene = new Hierarchy(hie);
            window.Loaded += Window_Loaded2;
            window.SampleEvent += Window_SampleEvent;

            wrpPan.Children.Clear();
            string[] meshDir = Directory.GetFiles("Res/meshes/", "*.obj");
            Button buttonLL = new Button();
            buttonLL.Padding = new Thickness(20, 20, 20, 20);
            buttonLL.Background = new SolidColorBrush(Color.FromRgb(112, 130, 250));
            buttonLL.Content = "add File";
            buttonLL.Click += ButtonLL_Click;
            wrpPan.Children.Add(buttonLL);

            Button buttonL = new Button();
            buttonL.Padding = new Thickness(20, 20, 20, 20);
            buttonL.Content = "light";
            buttonL.Click += Button_Click3;

            wrpPan.Children.Add(buttonL);
            foreach (string str in meshDir)
            {
                Button button = new Button();
                button.Padding = new Thickness(20, 20, 20, 20);
                button.Tag = str;
                button.Content = str.Substring(11, str.Substring(11).Length - 4);
                button.Click += Button_Click2;

                wrpPan.Children.Add(button);
            }



        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            Texture tex = new Texture((string)((Button)sender).Tag, "diffuse", PixelFormat.Rgba);
            if (scene.children[window.selectedObjj].GetType() == typeof(MeshContainer)) ((MeshContainer)scene.children[window.selectedObjj]).mate.textures = new List<Texture> (){ tex};
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
                traE.duped += TraE_duped;
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

        private void TraE_duped()
        {
            if (scene.children[window.selectedObjj].GetType() == typeof(MeshContainer))
            {
                var ms = (MeshContainer)((MeshContainer)scene.children[window.selectedObjj]).Clone();
                scene.children.Add(ms);
            }
            else if (scene.children[window.selectedObjj].GetType() == typeof(LightContainer))
            {
                var ms = (LightContainer)((LightContainer)scene.children[window.selectedObjj]).Clone();
                scene.children.Add(ms);
            }
            setHir();

        }

        private void TraE_deleted()
        {
            theList2.Children.Clear();
            scene.children.Remove(scene.children[window.selectedObjj]);
            window.selectedObjj = -1;
            setHir();
        }

        Hierarchy scene;
        private void Window_Loaded2(object sender, RoutedEventArgs e)
        {
            ((VeiwPortDisplay)sender).scene = scene;

            setHir();

        }
        void setHir()
        {
            theList.Children.Clear();
            for (int i = 0; i < scene.children.Count; i++)
            {
                TextBlock bt = new TextBlock();
                bt.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                bt.Padding = new Thickness(5, 5, 5, 5);
                bt.Margin = new Thickness(5, 0, 1, 5);
                bt.Width = float.NaN;
                bt.Text = scene.children[i].name;
                bt.Tag = i;
                bt.MouseDown += Bt_MouseDown;
                theList.Children.Add(bt);
            }
        }

        private void Bt_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            window.selectedObjj = (int)((TextBlock)sender).Tag;
            Window_SampleEvent();
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

            scene.children.Add(new MeshContainer(window.camera.Position, (string)((FrameworkElement)sender).Tag, scene));
            setHir();
        }
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            scene.children.Add(new LightContainer(scene) { Position = window.camera.Position, lightColor = new Vector4(1, 1, 1, 1) });
            setHir();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            wrpPan.Children.Clear();
           string[] meshDir = Directory.GetFiles("Res/textures/", "*.png");
            Button buttonLL = new Button();
            buttonLL.Padding = new Thickness(20, 20, 20, 20);
            buttonLL.Content = "add File";
            buttonLL.Click += ButtonLL_Click; 
            wrpPan.Children.Add(buttonLL);
            foreach (string str in meshDir)
            {
                Button button = new Button();
                button.Padding = new Thickness(20, 20, 20, 20);
                button.Background = new SolidColorBrush(Color.FromRgb(112, 130, 250));
                button.Tag = str;
                button.Content = str.Substring(13, str.Substring(13).Length - 4);
                button.Click += Button_Click1;

                wrpPan.Children.Add(button);
            }
            meshDir = Directory.GetFiles("Res/textures/", "*.jpg");
            foreach (string str in meshDir)
            {
                Button button = new Button();
                button.Padding = new Thickness(20, 20, 20, 20);
                button.Background = new SolidColorBrush(Color.FromRgb(112, 130, 250));
                button.Tag = str;
                button.Content = str.Substring(13, str.Substring(13).Length - 4);
                button.Click += Button_Click1;

                wrpPan.Children.Add(button);
            }
        }

        private void ButtonLL_Click(object sender, RoutedEventArgs e)
        {
            
           // var dialog = new Microsoft.Win32.OpenFileDialog();
           // dialog.ShowReadOnly = false;
           // // Show open file dialog box
           // dialog.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            wrpPan.Children.Clear();
            string[] meshDir = Directory.GetFiles("Res/meshes/", "*.obj");
            Button buttonLL = new Button();
            buttonLL.Padding = new Thickness(20, 20, 20, 20);
            buttonLL.Background = new SolidColorBrush(Color.FromRgb(112, 130, 250));
            buttonLL.Content = "add File";
            buttonLL.Click += ButtonLL_Click;
            wrpPan.Children.Add(buttonLL);

            Button buttonL = new Button();
            buttonL.Padding = new Thickness(20, 20, 20, 20);
            buttonL.Content = "light";
            buttonL.Click += Button_Click3;
            wrpPan.Children.Add(buttonL);
            foreach (string str in meshDir)
            {
                Button button = new Button();
                button.Padding = new Thickness(20, 20, 20, 20);
                button.Tag = str;
                button.Content = str.Substring(11, str.Substring(11).Length - 4);
                button.Click += Button_Click2;

                wrpPan.Children.Add(button);
            }
        }
    }


}
