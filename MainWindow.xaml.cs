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
        StartScreen startScreen;
        public MainWindow()
        {
            InitializeComponent();
            startScreen = new StartScreen(this);
            grid.Children.Add(startScreen);
        }

    }


}
