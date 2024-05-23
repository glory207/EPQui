using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace EPQui.UserCon
{
    /// <summary>
    /// Interaction logic for TransformEditor.xaml
    /// </summary>
    public partial class MaterialEditor : UserControl, INotifyPropertyChanged
    {
       public material target;
        public MaterialEditor()
        {
            InitializeComponent();
            traOff.BoundName = "Texture Offset";
            traSca.BoundName = "Texture Scale";
            traOff.PropertyChanged += TraE_PropertyChanged;
            traSca.PropertyChanged += TraE_PropertyChanged;
        }
        public void set(material tr)
        {
            target = tr;
            traOff.BoundVector = target.texOff;
            traSca.BoundVector = target.texScale;
        }
        bool mini;
        private void title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mini = !mini;
            if (mini) this.Height = 50;
            else this.Height = 240;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void TraE_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            target.texOff = traOff.BoundVector;
            target.texScale = traSca.BoundVector; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            //dialog.DefaultExt = ".obj"; // Default file extension
            //dialog.Filter = "Text documents (.obj)|*.obj"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                if (filename.EndsWith(".png")|| filename.EndsWith(".jpg"))
                {
                    target.textures = new List<Texture> { new Texture(filename, "diffuse", 0, PixelFormat.Rgba) };
                }
            }
        }
    }
}
