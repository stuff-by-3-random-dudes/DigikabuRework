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
        private System.Windows.Threading.DispatcherTimer dispatcherTimer = null;

        public TimerWindow()
        {
            mvm = (MainViewModel)FindResource("mvm");
            mvm.TimerIsOpen = true;
            InitializeComponent();
            SetupTimer();
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

        private void SetupTimer()
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            Console.WriteLine("Setup Timer");
            dispatcherTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Wenn vor Unterrichtsbeginn
                if (mvm.Stundenplan[0].Std.Anfang.Subtract(DateTime.Now) > new TimeSpan(0))//8:30 - 8:15 = 15
                {
                    mvm.AktuelleStunde = null;
                    mvm.NaechsteStunde = mvm.Stundenplan[0];

                    mvm.ZeitBisNaechsteStunde = Klassen.StundenDauer.StundenDauerListe[0].Anfang.Subtract(DateTime.Now);
                }
                // Wenn im Unterricht
                else
                {
                    foreach (Klassen.Stunde stunde in mvm.Stundenplan)
                    {
                        if (stunde.Std.AktuelleStunde())
                        {
                            mvm.AktuelleStunde = stunde;
                            mvm.ZeitBisNaechsteStunde = stunde.Std.Ende.Subtract(DateTime.Now);

                            int index = mvm.Stundenplan.IndexOf(mvm.AktuelleStunde) + 1;
                            if (index < mvm.Stundenplan.Count && index > 0)
                            {
                                mvm.NaechsteStunde = mvm.Stundenplan[index];
                            }
                            else if (index == mvm.Stundenplan.Count)
                            {
                                mvm.NaechsteStunde = null;
                            }
                            break;
                        }
                    }
                }
                // Wenn nach Unterricht
                if (DateTime.Now.Subtract(Klassen.StundenDauer.StundenDauerListe[Klassen.StundenDauer.StundenDauerListe.Count - 1].Ende) > new TimeSpan(0))//17- 16.5
                {
                    mvm.ZeitBisNaechsteStundeAsString = "Unterricht heute vorbei";
                    mvm.AktuelleStunde = null;
                    mvm.NaechsteStunde = null;
                }
            }
            catch (Exception)
            {
                // Wenn die Property Stundenliste nicht gelesen werden kann, weil sie durch drücken auf stundenplan neu geladen wird
                mvm.AktuelleStunde = null;
                mvm.NaechsteStunde = null;
            }
        }
    }
}
