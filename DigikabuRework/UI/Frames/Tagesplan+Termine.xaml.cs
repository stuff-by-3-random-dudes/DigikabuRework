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
    public partial class Tagesplan_Termine : Page
    {
        MainViewModel mvm;
        private static System.Windows.Threading.DispatcherTimer dispatcherTimer = null;
        public Tagesplan_Termine()
        {
            mvm = (MainViewModel)FindResource("mvm");
            getStundenPlanUndTermine(); // vllt auch kein await
            InitializeComponent();
            SetupTimer();
        }

        private void SetupTimer()
        {
            if (dispatcherTimer == null)
            {
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                
                
            }
            dispatcherTimer.Start();
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
                if (item.Std.AktuelleStunde() && row != null)//item.Schulstunde == "1"
                {
                    row.Background = Brushes.DarkGray;
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
    }
}
