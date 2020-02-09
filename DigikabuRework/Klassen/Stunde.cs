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
        public string VonBis { get; set; }
        public string Fach { get; set; }
        public string Lehrer { get; set; }
        public string Klassenzimmer { get; set; }
        public Stunde()
        {

        }
        public Stunde(string stunde, string vonbis, string fach)
        {
            Schulstunde = stunde;
            VonBis = vonbis;
            Fach = fach;
        }
    }
}
