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
using OpenTK.Mathematics;

namespace EPQui.UserCon
{
    /// <summary>
    /// Interaction logic for TransformEditor.xaml
    /// </summary>
    public partial class TransformEditor : UserControl, INotifyPropertyChanged
    {
        bool mini;
        private string DisplayName = "Na";


        public string BoundName
        {
            get { return DisplayName; }
            set
            {
                DisplayName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundName"));
            }
        }
        public TransformEditor()
        {
            DataContext = this;
            InitializeComponent();
            slider1.BoundName = "X";
            slider2.BoundName = "Y";
            slider3.BoundName = "Z"; 
            slider1.PropertyChangedUp += Slider1_PropertyChanged;
            slider2.PropertyChangedUp += Slider1_PropertyChanged;
            slider3.PropertyChangedUp += Slider1_PropertyChanged;
            this.MouseMove += slider1.Slider_MouseMove;
            this.MouseMove += slider2.Slider_MouseMove;
            this.MouseMove += slider3.Slider_MouseMove;
            SizeChanged += TransformEditor_SizeChanged;
        }

        private void TransformEditor_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            slider1.txtBlock.Width = Math.Max((ActualWidth - 90) / 3f,0.1f);
            slider2.txtBlock.Width = Math.Max((ActualWidth - 90) / 3f,0.1f);
            slider3.txtBlock.Width = Math.Max((ActualWidth - 90) / 3f, 0.1f);
        }

        private void Slider1_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            up = true;
            BoundVector = new Vector3(slider1.BoundVal, slider2.BoundVal, slider3.BoundVal);
        }
        bool up = false;
        public event PropertyChangedEventHandler? PropertyChanged;

        private Vector3 boundVector;
        public Vector3 BoundVector
        {
            get { return boundVector; }
            set
            {
                boundVector = value;
                if (up)
                {
                    up = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("boundVector"));
                    
                }
                else
                {
                    slider1.silence = true;
                    slider1.BoundVal = BoundVector.X;
                    slider2.silence = true;
                    slider2.BoundVal = BoundVector.Y;
                    slider3.silence = true;
                    slider3.BoundVal = BoundVector.Z;
                }

                
                
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mini = !mini;
            if(mini) border.Height = 30;
            else border.Height = 82;

        }
    }
    
}
