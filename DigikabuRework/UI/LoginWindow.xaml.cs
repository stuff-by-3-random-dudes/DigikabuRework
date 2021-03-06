﻿using DigikabuRework.Interfaces;
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

namespace DigikabuRework
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ErrorThrower
    {
        MainViewModel mvm;
        public MainWindow()
        {
            InitializeComponent();
            mvm = (MainViewModel)FindResource("mvm");
        }

        private void Window_Move(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        
        private void Login_Click(object sender, RoutedEventArgs e)
        {
           Login(sender);
        }

        public void ThrowErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Es ist ein Fehler aufgetretten", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private async Task Login(object sender)
        {
            try
            {
                (sender as Button).IsEnabled = false;
                await mvm.LoginAppStart(PW.Password, this);
                this.Close();
            }
            catch (Exception)
            {
                (sender as Button).IsEnabled = true;
            }
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
