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
    /// Interaction logic for name_Editor.xaml
    /// </summary>
    public partial class name_Editor : UserControl, INotifyPropertyChanged
    {
        public name_Editor()
        {
            InitializeComponent();
            this.Focusable = true;
        }

        private string boundVal;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string BoundVal
        {
            get { return boundVal; }
            set
            {
                boundVal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundVal"));

            
            }
        }

        private void txtBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
            }
        }
    }
}
