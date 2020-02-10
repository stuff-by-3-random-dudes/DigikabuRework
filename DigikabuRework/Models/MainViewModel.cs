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


        public WochenStundenPlan WSP { get; set; } = new WochenStundenPlan();

        private DigiCon Connection;

        public SchuelerInfo Sinfo { get; set; }


        public MainViewModel()
        {
            StundenDauer.SetupStundenDauerListe();
            UserName = Settings.Default.UserName;
            KeepData = Settings.Default.keepData;
            Connection = new DigiCon(this);
            GetSpeiseplan();
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
                await Connection.LoginAsync();
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
           await Connection.Logout();
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
            await Connection.GetStundenUndTermine(t);
           
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
            await Connection.GetSpeiseplan();
        }
        public async Task GetWochenSP(bool nextWeek)
        {
            ClearWSP();
            DateTime start = StartingDateOfWeek(DateTime.Now);
            
            if (nextWeek)
            {
                start = start.AddDays(7);
            }
            WSP.Start = start;
            WSP.End = start.AddDays(6);
            WSP.Montag = await GetStundenplanOfDay(start);
            WSP.Dienstag = await GetStundenplanOfDay(start.AddDays(1));
            WSP.Mittwoch = await GetStundenplanOfDay(start.AddDays(2));
            WSP.Donnerstag = await GetStundenplanOfDay(start.AddDays(3));
            WSP.Freitag = await GetStundenplanOfDay(start.AddDays(4));
        }
        private void ClearWSP()
        {
            WSP.Montag.Clear();
            WSP.Dienstag.Clear();
            WSP.Mittwoch.Clear();
            WSP.Donnerstag.Clear();
            WSP.Freitag.Clear();
        }
        public async Task<List<Stunde>> GetStundenplanOfDay(DateTime t)
        {
            return await Connection.GetStunden(t);
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
