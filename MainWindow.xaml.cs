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
using System.Windows.Media;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

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
            
            string[] meshDir = Directory.GetFiles("Res/scenes/", "*.sce");

            foreach (string str in meshDir)
            {
                Button button = new Button();
                button.Padding = new Thickness(20, 20, 20, 20);
                button.Tag = str;
                button.Content = str.Substring(11, str.Substring(11).Length - 4);
                button.Click += Button_Click; 

                wrpPan.Children.Add(button);
            }
            string strr = "new scene";
            Button buttonn = new Button();
            buttonn.Padding = new Thickness(20, 20, 20, 20);
            buttonn.Tag = "non";
            buttonn.Content = strr;
            buttonn.Click += Button_Click;

            wrpPan.Children.Add(buttonn);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grid.Children.Clear();
            grid.Children.Add(new TheFullThing((string)((FrameworkElement)sender).Tag));
        }
    }


}
