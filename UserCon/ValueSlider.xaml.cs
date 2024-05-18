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
    /// Interaction logic for ValueSlider.xaml
    /// </summary>
    public partial class ValueSlider : UserControl, INotifyPropertyChanged
    {
        public ValueSlider()
        {
            DataContext = this;
            InitializeComponent();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangedEventHandler? PropertyChangedUp;
        private float boundVal;
        public bool silence;
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
                    PropertyChangedUp?.Invoke(this, new PropertyChangedEventArgs("Up"));
                }
            }
        }
        private string boundname = "";
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
