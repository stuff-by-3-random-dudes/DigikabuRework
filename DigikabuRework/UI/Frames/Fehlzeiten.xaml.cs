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
    /// Interaktionslogik für Fehlzeiten.xaml
    /// </summary>
    public partial class Fehlzeiten : Page, IDisposable
    {
        MainViewModel mvm;

        public Fehlzeiten()
        {
            mvm = (MainViewModel)FindResource("mvm");
            
            InitializeComponent();
        }

        public void Dispose()
        {
           
        }
    }
}
