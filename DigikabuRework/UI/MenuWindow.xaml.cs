using DigikabuRework.Interfaces;
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
    /// Interaktionslogik für MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window, ErrorThrower
    {
        public MenuWindow()
        {
            InitializeComponent();
            load_frame.Content = new Frames.Tagesplan_Termine();
        }
        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void ListView_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ThrowErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Es ist ein Fehler aufgetretten", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
