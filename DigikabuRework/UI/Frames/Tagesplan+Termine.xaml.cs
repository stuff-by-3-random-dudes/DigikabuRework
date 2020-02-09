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
        public Tagesplan_Termine()
        {
            mvm = (MainViewModel)FindResource("mvm");
            getStundenPlanUndTermine();
            InitializeComponent();
            
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

                if (item.Schulstunde == "1")

                {

                    row.Background = Brushes.DarkGray;

                }
                else
                {

                    row.Background = this.Background;

                }

            }
        }


    }
}
