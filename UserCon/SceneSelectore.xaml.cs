using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace EPQui.UserCon
{
    /// <summary>
    /// Interaction logic for SceneSelectore.xaml
    /// </summary>
    public partial class SceneSelectore : Window
    {
        public SceneSelectore()
        {
            InitializeComponent();
            Closed += SceneSelectore_Closed;
        }

        private void SceneSelectore_Closed(object? sender, EventArgs e)
        {
            
        }
    }
}
