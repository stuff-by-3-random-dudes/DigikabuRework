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

namespace DigikabuRework.UI
{
    /// <summary>
    /// Interaktionslogik für TimerWindow.xaml
    /// </summary>
    public partial class TimerWindow : Window
    {
        MainViewModel mvm = null;
        public TimerWindow()
        {
            mvm = (MainViewModel)FindResource("mvm");
            mvm.TimerIsOpen = true;
            InitializeComponent();
        }
        private void Window_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    private void MoveWindow(object sender, MouseButtonEventArgs e)
    {
        try
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        catch (Exception)
        {

        }
    }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mvm.TimerIsOpen = false;
        }
    }
}
