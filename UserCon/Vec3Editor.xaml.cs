using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public partial class Vec3Editor : UserControl, INotifyPropertyChanged
    {
        bool mini = true;
        private string DisplayName = "Na";
        public float speed;
        public float Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                slider1.speed = speed;
                slider2.speed = speed;
                slider3.speed = speed;
            }
        }

        public string BoundName
        {
            get { return DisplayName; }
            set
            {
                DisplayName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundName"));
            }
        }
        public Vec3Editor()
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
            this.MouseMove += Slider_MouseMove;
            this.LostFocus += Vec3Editor_LostFocus;
            this.MouseLeave += Vec3Editor_LostFocus;

            SizeChanged += TransformEditor_SizeChanged;
            Speed = 0.01f;
        }

        private void Vec3Editor_LostFocus(object sender, RoutedEventArgs e)
        {
            clicked = false;
            slider1.clicked = false;
            slider2.clicked = false;
            slider3.clicked = false;
        }

        private void TransformEditor_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            slider1.txtBlock.Width = Math.Max((ActualWidth  / 3f) - slider1.tle.ActualWidth,0.1f);
            slider2.txtBlock.Width = Math.Max((ActualWidth  / 3f) - slider1.tle.ActualWidth,0.1f);
            slider3.txtBlock.Width = Math.Max((ActualWidth  / 3f) - slider1.tle.ActualWidth, 0.1f);
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
                    slider1.silence = true;
                    slider1.BoundVal = BoundVector.X;
                    slider2.silence = true;
                    slider2.BoundVal = BoundVector.Y;
                    slider3.silence = true;
                    slider3.BoundVal = BoundVector.Z;
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



        float delta, pos;
        bool clicked;
        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            clicked = false;
        }

        private void RowDefinition_MouseDown(object sender, MouseButtonEventArgs e)
        {
            clicked = true;
        }

        public void Slider_MouseMove(object sender, MouseEventArgs e)
        {

            delta = pos - (float)Mouse.GetPosition(InputHitTest(new Point(0, 0))).X;
            pos = (float)Mouse.GetPosition(InputHitTest(new Point(0, 0))).X;
            if (clicked)
            {
                up = true;
                BoundVector -= delta * Speed * Vector3.One;
            }
        }

    }

}
