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
    /// Interaction logic for HyrachyMesh.xaml
    /// </summary>
    public partial class HyrachyMesh : UserControl, INotifyPropertyChanged
    {
        public HyrachyMesh()
        {
            DataContext = this;
            InitializeComponent();
        }
        private string boundText;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string BoundText
        {
            get { return boundText; }
            set 
            { 
                boundText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundText"));
            }
        }

    }
}
