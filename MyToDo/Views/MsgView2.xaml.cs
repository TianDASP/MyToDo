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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyToDo.Views
{
    /// <summary>
    /// MsgView2.xaml 的交互逻辑
    /// </summary>
    public partial class MsgView2 : UserControl
    {
        public MsgView2()
        {
            InitializeComponent();
            this.Loaded += MsgView2_Loaded;
        }

        private void MsgView2_Loaded(object sender, RoutedEventArgs e)
        {
            Window win = ((UserControl)sender).Parent as Window;
            if (win != null)
            {
                win.WindowStyle = WindowStyle.None; 
                win.ResizeMode = ResizeMode.NoResize; 
            }
        }
    }
}
