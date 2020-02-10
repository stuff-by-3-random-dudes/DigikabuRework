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
    /// Interaktionslogik für WochenStundenPlan.xaml
    /// </summary>
    public partial class WochenStundenPlan : Page, IDisposable
    {
        MainViewModel mvm;
        public WochenStundenPlan()
        {
            mvm = (MainViewModel)FindResource("mvm");
            InitializeComponent();
            mvm.GetWochenSP(false);
        }

        public void Dispose()
        {
           
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(mvm.SelStunde != null)
            {
                StundenInfo si = new StundenInfo();
                si.Show();
            }
        }

        private void Left_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SwitchWSP(false);
        }

        private void Right_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SwitchWSP(true);
        }
        private void SwitchWSP(bool next)
        {
            if (next)
            {
                Right.IsEnabled = false;
                Left.IsEnabled = true;
            }
            else
            {
                Left.IsEnabled = false;
                Right.IsEnabled = true;
            }
            mvm.GetWochenSP(next);
        }
    }
}
