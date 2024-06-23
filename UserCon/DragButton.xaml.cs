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
    /// Interaction logic for DragButton.xaml
    /// </summary>
    public partial class DragButton : UserControl, INotifyPropertyChanged
    {
        private string DisplayName = "Na";
        public string Dat;
        public string DatObj;
        public string daName
        {
            get
            {
                return DisplayName;
            }
            set
            {
                DisplayName = value;
               textt.Text = value;
            }
        }
        public DragButton(string dat, string datobj)
        {
            InitializeComponent();
            MouseEnter += DragButton_MouseEnter;
            MouseLeave += DragButton_MouseLeave;
            MouseMove += DragButton_MouseMove;
            Dat = dat;
            DatObj = datobj;
        }

        private void DragButton_MouseMove(object sender, MouseEventArgs e)
        {
           if(e.LeftButton == MouseButtonState.Pressed)
            {
              //  DragDrop.DoDragDrop("asd","asd",DragDropEffects.All);
                DataObject dataObject = new DataObject(DatObj, Dat);
                DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);

            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void DragButton_MouseLeave(object sender, MouseEventArgs e)
        {
            daBox.Background = new SolidColorBrush(Color.FromRgb(20, 130, 200));
           
        }

        private void DragButton_MouseEnter(object sender, MouseEventArgs e)
        {
            daBox.Background = new SolidColorBrush(Color.FromRgb(112, 130, 250));
        }
    }
}
