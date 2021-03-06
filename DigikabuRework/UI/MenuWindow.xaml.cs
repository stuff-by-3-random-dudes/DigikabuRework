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
using System.Windows.Shapes;

namespace DigikabuRework.UI
{
    /// <summary>
    /// Interaktionslogik für MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window, ErrorThrower
    {
        MainViewModel mvm;
        int selectedIndex;
        public MenuWindow()
        {
            InitializeComponent();
            load_frame.Content = new Frames.Tagesplan_Termine();
            mvm = (MainViewModel)FindResource("mvm");
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
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
            catch (Exception)
            {

            }
            
        }
        private void DestroyFrame()
        {
            //(load_frame.Content as IDisposable).Dispose();
            load_frame.Content = null;
            load_frame.NavigationService.RemoveBackEntry();
        }
        private void ListView_Click(object sender, MouseButtonEventArgs e)
        {


            if ((sender as ListView).SelectedIndex != selectedIndex)
            {
                if ((sender as ListView).SelectedIndex != 1)
                {
                    DestroyFrame();
                    selectedIndex = (sender as ListView).SelectedIndex;
                }

                switch ((sender as ListView).SelectedIndex)
                {
                    case 1:
                        if (!mvm.TimerIsOpen)
                        {
                            TimerWindow tw = new TimerWindow();
                            tw.Show();
                        }
                        goto case 8;
                    case 0:
                        load_frame.Content = new Frames.Tagesplan_Termine();
                        Fenster.Text = "Digikabu - Termine";
                        break;
                    case 2:
                        load_frame.Content = new Frames.WochenStundenPlan();
                        Fenster.Text = "Digikabu - Wochenstundenplan";
                        break;
                    case 3:
                        load_frame.Content = new Frames.Schulaufgaben();
                        Fenster.Text = "Digikabu - Schulaufgabenplan";
                        break;
                    case 4:
                        load_frame.Content = new Frames.Speiseplan();
                        Fenster.Text = "Digikabu - Speiseplan";
                        break;
                    case 5:

                        Fenster.Text = "Digikabu - Entschuldigung";
                        break;
                    case 6:
                        load_frame.Content = new Frames.Fehlzeiten();
                        Fenster.Text = "Digikabu - Fehlzeiten";
                        break;
                    case 7:

                        Fenster.Text = "Digikabu - Einstellungen";
                        break;
                    case 8:
                        (sender as ListView).SelectedIndex = selectedIndex;
                        Console.WriteLine((sender as ListView).SelectedIndex);
                        break;
                }
            }
        }

        private void Window_Close(object sender, RoutedEventArgs e)
        {
            Task.Run(() => mvm.Logout());
            this.Close();
        }

        public void ThrowErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Es ist ein Fehler aufgetretten", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Window_Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
