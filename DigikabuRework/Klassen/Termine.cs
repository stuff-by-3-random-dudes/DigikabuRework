using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigikabuRework.Klassen
{
    class Termine
    {
        public string Tag { get; set; }
        public string Termin { get; set; }
        public Termine(string t, string te)
        {
            Tag = t;
            Termin = te;
        }
    }
}
