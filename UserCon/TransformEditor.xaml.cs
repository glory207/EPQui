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

namespace EPQui.UserCon
{
    /// <summary>
    /// Interaction logic for TransformEditor.xaml
    /// </summary>
    public partial class TransformEditor : UserControl, INotifyPropertyChanged
    {
       public HierObj target;
        public TransformEditor()
        {
            InitializeComponent();
            traEP.BoundName = "Position";
            traER.BoundName = "objectRotation";
            traES.BoundName = "objectScale";
            traEP.PropertyChanged += TraE_PropertyChanged;
            traER.PropertyChanged += TraE_PropertyChanged;
            traES.PropertyChanged += TraE_PropertyChanged;
        }
        public void set(HierObj tr)
        {
            target = tr;
            traEP.BoundVector = target.Position;
            traER.BoundVector = target.objectRotation;
            traES.BoundVector = target.objectScale;
        }
        bool mini;
        private void title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mini = !mini;
            if (mini) this.Height = 50;
            else this.Height = 300;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void TraE_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            {
                target.Position = traEP.BoundVector;
                target.objectRotation = traER.BoundVector;
                target.objectScale = traES.BoundVector;
            }
        }
    }
}
