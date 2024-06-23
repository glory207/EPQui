using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using OpenTK.Mathematics;

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
            traER.Speed = 0.01f;
            traES.BoundName = "objectScale";
            traEP.PropertyChanged += TraE_PropertyChanged;
            traER.PropertyChanged += TraE_PropertyChanged;
            traES.PropertyChanged += TraE_PropertyChanged;
        }
        public void set(HierObj tr)
        {
            target = tr;
            traEP.BoundVector = target.Position;
            traER.BoundVector = target.objectRotation.ToEulerAngles();
            traES.BoundVector = target.objectScale;
            Danm.BoundVal = target.name;
        }
        bool mini = false;
        private void title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mini = !mini;
            if (mini) this.Height = 50;
            else this.Height = double.NaN;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public delegate void deletedEventHandler();
        public event deletedEventHandler? deleted;
        public event deletedEventHandler? duped;


        private void TraE_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            {
                target.Position = traEP.BoundVector;
                target.objectRotation = Quaternion.FromEulerAngles(traER.BoundVector);
                target.objectScale = traES.BoundVector;
                target.name = Danm.BoundVal;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        
            deleted.Invoke();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            duped.Invoke();
        }
    }
}
