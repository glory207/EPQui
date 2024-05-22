using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for ObjectSelector.xaml
    /// </summary>
    public partial class ObjectSelector : UserControl, INotifyPropertyChanged
    {
        public ObjectSelector()
        {
            InitializeComponent();
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
