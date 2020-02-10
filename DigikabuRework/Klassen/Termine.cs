using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigikabuRework.Klassen
{
    class Termine
    {
        public string WochenTag { get; set; }
        public string Tag { get; set; }
        public string Termin { get; set; }
        public Termine(string d, string t, string te)
        {
            WochenTag = d;
            Tag = t;
            Termin = te;
        }
    }
}
