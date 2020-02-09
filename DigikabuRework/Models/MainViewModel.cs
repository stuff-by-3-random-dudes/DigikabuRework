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
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool KeepData { get; set; }

        public Stunde SelStunde { get; set; }

        private List<Stunde> stundenplan = new List<Stunde>();


        public List<Stunde> Stundenplan
        {
            get { return stundenplan; }
            set { stundenplan = value;
                onPropertyChanged();
            }
        }
        private List<Termine> terminplan = new List<Termine>();


        public List<Termine> Terminplan
        {
            get { return terminplan; }
            set
            {
                terminplan = value;
                onPropertyChanged();
            }
        }


       

        private DigiCon Connection;

        public SchuelerInfo Sinfo { get; set; }


        public MainViewModel()
        {
            StundenDauer.SetupStundenDauerListe();
            UserName = Settings.Default.UserName;
            KeepData = Settings.Default.keepData;
            Connection = new DigiCon(this);


        }
        
        public async Task LoginAppStart(string pwd, object sender)
        {
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
        private void OpenMenu()
        {
            UI.MenuWindow mw = new UI.MenuWindow();
            mw.Show();
        }
        public async Task getStundenUndTermine()
        {
            await Connection.getStundenUndTermine();
        }
    }
}
