using OpenTK.Mathematics;
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

namespace EPQui.UserCon
{
    /// <summary>
    /// Interaction logic for ValueSlider.xaml
    /// </summary>
    public partial class ValueSlider : UserControl, INotifyPropertyChanged
    {
        public float speed = 0;
        public ValueSlider()
        {
            DataContext = this;
            InitializeComponent();
            this.MouseDown += ValueSlider_MouseDown;
            this.Focusable = true;
        }

        private void ValueSlider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            clicked = true;
           
        }

        float delta,pos;
        public void Slider_MouseMove(object sender, MouseEventArgs e)
        {
            
            delta = pos - (float)Mouse.GetPosition(InputHitTest(new Point(0, 0))).X;
            pos = (float)Mouse.GetPosition(InputHitTest(new Point(0, 0))).X;
            if (clicked&&e.LeftButton == MouseButtonState.Pressed) BoundVal -= delta * speed;
            else clicked = false;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangedEventHandler? PropertyChangedUp;
        private float boundVal;
        public bool silence;
        public bool clicked;
        public float BoundVal
        {
            get { return boundVal; }
            set
            {
                boundVal = value;
                if (silence) {
                    silence = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundVal"));
                    
                }
                else
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundVal"));
                    PropertyChangedUp?.Invoke(this, new PropertyChangedEventArgs("Up"));
                }
            }
        }
        private string boundname = "";

        private void txtBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                this.Focus();
            }
        }

        public string BoundName
        {
            get { return boundname; }
            set
            {
                boundname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundName"));
            }
        }
    }
}
