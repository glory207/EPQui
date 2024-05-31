using OpenTK.Mathematics;
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

namespace EPQui.UserCon
{
    /// <summary>
    /// Interaction logic for LightEditor.xaml
    /// </summary>
    public partial class LightEditor : UserControl, INotifyPropertyChanged
    {
        public LightContainer target;
        public LightEditor()
        {
            InitializeComponent();
            traS.BoundName = "Color";
            traS.PropertyChanged += TraE_PropertyChanged;
            traS.slider1.BoundName = "R";
            traS.slider2.BoundName = "G";
            traS.slider3.BoundName = "B";
            traI.BoundName = "intensity";
            traI.PropertyChanged += TraE_PropertyChanged;
            traA.BoundName = "cone";
            traA.PropertyChanged += TraE_PropertyChanged;
            traA.slider1.BoundName = "ang";
            traA.slider2.BoundName = "sft";
            combo.SelectionChanged += Combo_SelectionChanged;
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            target.Type = (LightType)combo.SelectedIndex;
        }

        public void set(LightContainer tr)
        {
            target = tr;
            traS.BoundVector = clamp(target.lightColor.Xyz,0,1);
            traI.BoundVector = target.intencity;
            traA.BoundVector = clamp(target.angle, 0,3.1f);
            combo.SelectedIndex = (int)target.Type;
        }
        bool mini;
        private void title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mini = !mini;
            if (mini) this.Height = 50;
            else this.Height = double.NaN;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void TraE_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

                traS.BoundVector = clamp(traS.BoundVector, 0, 1);
                target.lightColor.Xyz = traS.BoundVector;
                target.intencity = traI.BoundVector;
           float rX = clamp(traA.BoundVector.X, 0, 1.57f);
           float rY = clamp(traA.BoundVector.Y, 0, 1);
            traA.BoundVector = new Vector2(rX, rY);
                target.angle = new Vector2(rX, rY);

        }

        Vector3 clamp(Vector3 f, float a, float b)
        {
            f = new Vector3(clamp(f.X,a,b), clamp(f.Y, a, b), clamp(f.Z, a, b));
            return f;
        }
        Vector2 clamp(Vector2 f, float a, float b)
        {
            f = new Vector2(clamp(f.X,a,b), clamp(f.Y, a, b));
            return f;
        }
        float clamp(float f,float a, float b)
        {
            if(f < a) f = a;
            if(f > b) f = b;
            return f;
        }

    }
}
