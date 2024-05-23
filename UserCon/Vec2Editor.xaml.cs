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
    public partial class Vec2Editor : UserControl, INotifyPropertyChanged
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
        public Vec2Editor()
        {
            DataContext = this;
            InitializeComponent();
            slider1.BoundName = "X";
            slider2.BoundName = "Y";
            slider1.PropertyChangedUp += Slider1_PropertyChanged;
            slider2.PropertyChangedUp += Slider1_PropertyChanged;
            this.MouseMove += slider1.Slider_MouseMove;
            this.MouseMove += slider2.Slider_MouseMove;
            this.MouseMove += Slider_MouseMove;
            SizeChanged += TransformEditor_SizeChanged;
        }

        private void TransformEditor_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            slider1.txtBlock.Width = Math.Max((ActualWidth - 60) / 2f,0.1f);
            slider2.txtBlock.Width = Math.Max((ActualWidth - 60) / 2f,0.1f);
        }

        private void Slider1_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            up = true;
            BoundVector = new Vector2(slider1.BoundVal, slider2.BoundVal);
        }
        bool up = false;
        public event PropertyChangedEventHandler? PropertyChanged;

        private Vector2 boundVector;
        public Vector2 BoundVector
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
                    up = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("boundVector"));
                    
                }
                else
                {
                    slider1.silence = true;
                    slider1.BoundVal = BoundVector.X;
                    slider2.silence = true;
                    slider2.BoundVal = BoundVector.Y;
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
                BoundVector -= delta * 0.05f * Vector2.One;
            }
        }

    }

}
