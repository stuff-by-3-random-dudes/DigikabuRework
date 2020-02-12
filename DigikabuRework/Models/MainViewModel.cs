using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigikabuRework.Properties;
using DigikabuRework.Klassen;
using DigikabuRework.Connection;
using System.Windows.Controls;
using DigikabuRework.UI;
using DigikabuRework.Interfaces;

namespace DigikabuRework
{
    class MainViewModel : BaseModel
    {
        private TimeSpan zeitBisNaechsteStunde = new TimeSpan();
        public TimeSpan ZeitBisNaechsteStunde
        {
            get
            {
                return zeitBisNaechsteStunde;
            }
            set
            {
                zeitBisNaechsteStunde = value;
                OnPropertyChanged();
                ZeitBisNaechsteStundeAsString = GetTimerAsString();
            }
        }

        private List<Termine> schulaufgabenUndSonstige = new List<Termine>();

        public List<Termine> SchulaufgabenUndSonstige
        {
            get { return schulaufgabenUndSonstige; }
            set { schulaufgabenUndSonstige = value;
                OnPropertyChanged();
            }
        }



        private List<Speise> speiseplan = new List<Speise>();

        

        public List<Speise> Speiseplan
        {
            get { return speiseplan; }
            set { speiseplan = value;
                OnPropertyChanged();
            }
        }

        private string zeitBisNaechsteStundeAsString = "vor Unterrichtsbeginn";
        public string ZeitBisNaechsteStundeAsString
        {
            get
            {
                return zeitBisNaechsteStundeAsString;
            }
            set
            {
                zeitBisNaechsteStundeAsString = value;
                OnPropertyChanged();
            }
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool KeepData { get; set; }
        
        public Stunde SelStunde { get; set; }

        private List<Stunde> stundenplan = new List<Stunde>();


        public List<Stunde> Stundenplan
        {
            get { return stundenplan; }
            set { stundenplan = value;
                OnPropertyChanged();
            }
        }
        private List<Termine> terminplan = new List<Termine>();


        public List<Termine> Terminplan
        {
            get { return terminplan; }
            set
            {
                terminplan = value;
                OnPropertyChanged();
            }
        }

        private Fehlzeit fehlzeit;

        public Fehlzeit AnzahlFehlzeit
        {
            get { return fehlzeit; }
            set { fehlzeit = value;
                OnPropertyChanged();
            }
        }


        private List<Fehlzeit> fehlzeiten = new List<Fehlzeit>();

        public List<Fehlzeit> Fehlzeiten
        {
            get { return fehlzeiten; }
            set { fehlzeiten = value;
                OnPropertyChanged();
            }
        }


        public WochenStundenPlan WochenStdPlan { get; set; } = new WochenStundenPlan();

        private DigiCon connection;

        public SchuelerInfo Sinfo { get; set; }

        public bool TimerIsOpen { get; set; } = false;

        public MainViewModel()
        {
            StundenDauer.SetupStundenDauerListe();
            UserName = Settings.Default.UserName;
            KeepData = Settings.Default.keepData;
            connection = new DigiCon(this);
            
        }

        public async Task GetSchulaufgaben()
        {
            await connection.GetSchulaufgaben();
        }

        public async Task LoginAppStart(string pwd, object sender)
        {
            if (KeepData)
            {
                Settings.Default.UserName = UserName;

            }
            else
            {
                Settings.Default.UserName = string.Empty;
            }
            Settings.Default.keepData = KeepData;
            Settings.Default.Save();
            Password = pwd;
            await LoginAsync(sender);
        }
        
        public async Task LoginAsync(object sender)
        {
            try
            {
                await connection.LoginAsync();
                OpenMenu();
            }
            catch (Exception e)
            {
                (sender as ErrorThrower).ThrowErrorMessage(e.Message);
                throw new Exception();
            }
           
        }
        public async Task Logout()
        {
           await connection.Logout();
        }
        private void OpenMenu()
        {
            UI.MenuWindow mw = new UI.MenuWindow();
            mw.Show();
        }
        
        public async Task GetStundenUndTermine(DateTime t)
        {
            stundenplan.Clear();
            terminplan.Clear();
            await connection.GetStundenUndTermine(t);
           
        }
        public async Task GetFehlzeiten() {
            await connection.GetFehlzeiten();
            Console.WriteLine("Done");
        }

        public string GetTimerAsString()
        {
            string ret = String.Empty;
            if (ZeitBisNaechsteStunde.Hours != 0)
            {
                ret += $"{ZeitBisNaechsteStunde.Hours} Stunden\n";
            }
            if (ZeitBisNaechsteStunde.Minutes != 0)
            {
                ret += $"{ZeitBisNaechsteStunde.Minutes} Minuten\n";
            }
            ret += $"{ZeitBisNaechsteStunde.Seconds} Sekunden verbleibend";
            return ret;
        }
        public async Task GetSpeiseplan()
        {
            Speiseplan = await connection.GetSpeiseplan();
        }
        public async Task GetWochenStundenplan(bool nextWeek)
        {
            ClearWochenStundenPlan();
            DateTime start = StartingDateOfWeek(DateTime.Now);
            
            if (nextWeek)
            {
                start = start.AddDays(7);
            }
            WochenStdPlan.Start = start;
            WochenStdPlan.End = start.AddDays(6);
            WochenStdPlan.Montag = await GetStundenplanOfDay(start);
            WochenStdPlan.Dienstag = await GetStundenplanOfDay(start.AddDays(1));
            WochenStdPlan.Mittwoch = await GetStundenplanOfDay(start.AddDays(2));
            WochenStdPlan.Donnerstag = await GetStundenplanOfDay(start.AddDays(3));
            WochenStdPlan.Freitag = await GetStundenplanOfDay(start.AddDays(4));
        }
        private void ClearWochenStundenPlan()
        {
            WochenStdPlan.Montag.Clear();
            WochenStdPlan.Dienstag.Clear();
            WochenStdPlan.Mittwoch.Clear();
            WochenStdPlan.Donnerstag.Clear();
            WochenStdPlan.Freitag.Clear();
        }
        public async Task<List<Stunde>> GetStundenplanOfDay(DateTime t)
        {
            return await connection.GetStunden(t);
        }
        private DateTime StartingDateOfWeek(DateTime date)
        {
            DateTime usedDate;
            int dateAdjustment = 0;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dateAdjustment = 1;
                    break;
                case DayOfWeek.Monday:
                    dateAdjustment = 0;
                    break;
                case DayOfWeek.Tuesday:
                    dateAdjustment = -1;
                    break;
                case DayOfWeek.Wednesday:
                    dateAdjustment = -2;
                    break;
                case DayOfWeek.Thursday:
                    dateAdjustment = -3;
                    break;
                case DayOfWeek.Friday:
                    dateAdjustment = -4;
                    break;
                case DayOfWeek.Saturday:
                    dateAdjustment = 2;
                    break;
                default:
                    break;
            }
            usedDate = date.AddDays(Convert.ToDouble(dateAdjustment));
            return usedDate;
        }
    }
}
