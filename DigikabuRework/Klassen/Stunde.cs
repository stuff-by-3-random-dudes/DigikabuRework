using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigikabuRework.Klassen
{
    class Stunde
    {
        public string Schulstunde { get; set; }
        public StundenDauer Std { get; set; }
        public Schulstunden Stn { get; set; }
        public string Fach { get; set; }
        public string Lehrer { get; set; }
        public string Klassenzimmer { get; set; }
        public Stunde()
        {

        }
       
        public Stunde(Schulstunden s, string fach, string lehrer, string kzimmer) : this(s, fach)
        {
            Lehrer = lehrer;
            Klassenzimmer = kzimmer;

        }
        public Stunde(Schulstunden s, string fach)
        {
            Stn = s;
            if(s != Schulstunden.Pause)
            {
                Schulstunde = ( ((int)s) + 1 ).ToString();
            }
            else
            {
                Schulstunde = s.ToString();
            }
            if(fach == "")
            {
                Fach = "Entfällt";
            }
            else
            {
                Fach = fach;
            }
           
            setStundenDauer();
        }

        private void setStundenDauer()
        {
            //Std = StundenDauer.StundenDauerListe[(int)stn];
            foreach (var item in StundenDauer.StundenDauerListe)
            {
                if (item.Schulstunde == Stn)
                {
                    Std = item;
                    break;
                }
            }
        }

    }
}
