using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
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
    /// Interaction logic for TransformEditor.xaml
    /// </summary>
    public partial class TransformEditor : UserControl, INotifyPropertyChanged
    {
        public TransformEditor()
        {
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private float boundTextX;
        private float boundTextY;
        private float boundTextZ;
        public float BoundTextX
        {
            get { return boundTextX; }
            set
            {
                boundTextX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundTextX"));
            }
        }
        public float BoundTextY
        {
            get { return boundTextY; }
            set
            {
                boundTextY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundTextY"));
            }
        }
        public float BoundTextZ
        {
            get { return boundTextZ; }
            set
            {
                boundTextZ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundTextZ"));
            }
        }

        
    }
    
}
