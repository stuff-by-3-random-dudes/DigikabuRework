using DigikabuRework.Klassen;
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
            getStundenPlanUndTermine();
            SetupTimer();
            InitializeComponent();
            
        }

        private void SetupTimer()
        {
            if (dispatcherTimer == null)
            {
                
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                Console.WriteLine("Setup Timer");
            }
            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            changeStunde();
        }

        public async Task getStundenPlanUndTermine()
        {
           await mvm.getStundenUndTermine();
            
        }
        public void changeStunde()
        {
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
                }
                else if(row != null)
                {
                    row.Background = this.Background;
                }
            }
        }

        private void SP_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StundenInfo si = new StundenInfo();
            si.ShowDialog();
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
            //SetupTimer();
            dispatcherTimer.Start();
        }
    }
}
