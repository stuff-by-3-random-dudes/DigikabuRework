﻿using DigikabuRework.Klassen;
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

namespace DigikabuRework.UI.Frames
{
    /// <summary>
    /// Interaktionslogik für Tagesplan_Termine.xaml
    /// </summary>
    public partial class Tagesplan_Termine : Page, IDisposable
    {
        MainViewModel mvm;

        private System.Windows.Threading.DispatcherTimer dispatcherTimer = null;
        
        public Tagesplan_Termine()
        {
            mvm = (MainViewModel)FindResource("mvm");
            GetStundenPlanUndTermine();
            SetupTimer();
            InitializeComponent();
            
        }

        private void SetupTimer()
        {
            if (dispatcherTimer == null)
            {
                
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(Stundentimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                Console.WriteLine("Setup Timer");
            }
            
        }

        private void Stundentimer_Tick(object sender, EventArgs e)
        {
            ChangeStunde();
        }
       
        public async Task GetStundenPlanUndTermine()
        {
           await mvm.GetStundenUndTermine();
        }
        public void ChangeStunde()
        {
            bool zeitGesetzt = false;
            foreach (Stunde item in SP.ItemsSource)
            {
                var row = SP.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (item.Std.AktuelleStunde())//item.Schulstunde == "1"
                {
                    if (row != null)
                    {
                        row.Background = Brushes.DarkGray;
                    }
                    mvm.ZeitBisNaechsteStunde = item.Std.Ende.Subtract(DateTime.Now);
                    zeitGesetzt = true;
                }
                else if(row != null)
                {
                    row.Background = this.Background;
                }
            }
            if (!zeitGesetzt && DateTime.Now.Subtract(StundenDauer.StundenDauerListe[StundenDauer.StundenDauerListe.Count - 2].Ende) > new TimeSpan(0))//17- 16.5
            {
                //mvm.ZeitBisNaechsteStunde = StundenDauer.StundenDauerListe[StundenDauer.StundenDauerListe.Count - 2].Ende.Subtract(DateTime.Now);
                mvm.ZeitBisNaechsteStundeAsString = "Unterricht heute vorbei";
            }
            else if (!zeitGesetzt)
            {
                mvm.ZeitBisNaechsteStunde = StundenDauer.StundenDauerListe[0].Anfang.Subtract(DateTime.Now);
            }
        }

        private void SP_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(SP.SelectedIndex != -1)
            {
                StundenInfo si = new StundenInfo();
                si.ShowDialog();
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            Console.WriteLine("Stopped");
        }

        public void Dispose()
        {
            dispatcherTimer.Stop();
            dispatcherTimer = null;
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            dispatcherTimer.Start();
        }
    }
}
