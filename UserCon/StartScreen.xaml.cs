using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using static System.Formats.Asn1.AsnWriter;

namespace EPQui.UserCon
{
    /// <summary>
    /// Interaction logic for StartScreen.xaml
    /// </summary>
    public partial class StartScreen : UserControl
    {
        public MainWindow par;
        
        public StartScreen(MainWindow par)
        {

            this.par = par;
            InitializeComponent();
            set();
        }
        public void set()
        {
            wrpPan.Children.Clear();
            string[] meshDir = Directory.GetFiles("Res/scenes/", "*.sce");

            foreach (string str in meshDir)
            {
                Button button = new Button();
                button.Padding = new Thickness(20, 20, 20, 20);
                button.Tag = str;
                button.Content = str.Substring(11, str.Substring(11).Length - 4);
                button.Click += Button_Click;

                wrpPan.Children.Add(button);
            }
            string strr = "new scene";
            Button buttonn = new Button();
            buttonn.Padding = new Thickness(20, 20, 20, 20);
            buttonn.Tag = "non";
            buttonn.Content = strr;
            buttonn.Click += Button_Click;

            wrpPan.Children.Add(buttonn);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            par.grid.Children.Clear();
            TheFullThing fullThing = new TheFullThing((string)((Button)sender).Tag);
            par.grid.Children.Add(fullThing);
            
            
        }
    }
}
