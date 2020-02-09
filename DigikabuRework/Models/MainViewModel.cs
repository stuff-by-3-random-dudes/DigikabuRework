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
    class MainViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool KeepData { get; set; }

        public Stunde SelStunde { get; set; }

        public List<Stunde> Stundenplan { get; set; } = new List<Stunde>();
        public List<Termine> Terminplan { get; set; } = new List<Termine>();

        private DigiCon Connection;

        public SchuelerInfo Sinfo { get; set; }


        public MainViewModel()
        {
            UserName = Settings.Default.UserName;
            KeepData = Settings.Default.keepData;
            Connection = new DigiCon(this);
        }
        
        public void LoginAppStart(string pwd, object sender)
        {
            Password = pwd;
            Login(sender);
        }
        public void Login(object sender)
        {
            try
            {
                //throw new Exception("test");
                Sinfo = new SchuelerInfo(UserName, "testklasse");
                OpenMenu();
            }
            catch (Exception e)
            {
                (sender as ErrorThrower).ThrowErrorMessage(e.Message);
            }
           

        }
        private void OpenMenu()
        {
            UI.MenuWindow mw = new UI.MenuWindow();
            mw.Show();
        }
        
    }
}
