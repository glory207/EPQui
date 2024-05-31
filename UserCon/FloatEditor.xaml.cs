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
    public partial class FloatEditor : UserControl, INotifyPropertyChanged
    {
        bool mini;
        private string DisplayName = "Na";
        public float speed;
        public float Speed
        {
            get { return speed; }
            set {
                speed = value;
                slider1.speed = speed; }
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
        public FloatEditor()
        {
            DataContext = this;
            InitializeComponent();
            slider1.BoundName = " ";
            slider1.PropertyChangedUp += Slider1_PropertyChanged;

            this.MouseMove += slider1.Slider_MouseMove;
            this.MouseMove += Slider_MouseMove;
            SizeChanged += TransformEditor_SizeChanged;
            Speed = 0.01f;
        }

        private void TransformEditor_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            slider1.txtBlock.Width = Math.Max((ActualWidth - 30),0.1f);
        }

        private void Slider1_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            up = true;
            BoundVector = slider1.BoundVal;
        }
        bool up = false;
        public event PropertyChangedEventHandler? PropertyChanged;

        private float boundVector;
        public float BoundVector
        {
            get { return boundVector; }
            set
            {
                boundVector = value;
                if (up)
                {
                    slider1.silence = true;
                    slider1.BoundVal = BoundVector;
                    up = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("boundVector"));
                    
                }
                else
                {
                    slider1.silence = true;
                    slider1.BoundVal = BoundVector;
                }

                
                
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mini = !mini;
            if(mini) border.Height = 30;
            else border.Height = double.NaN;
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
                BoundVector -= delta * Speed;
            }
        }

    }

}
