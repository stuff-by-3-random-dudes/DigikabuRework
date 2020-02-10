using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigikabuRework.Klassen
{
    class Speise
    {
        public string Wochentag { get; set; }
        public string Gericht { get; set; }
        public Speise(string w, string g)
        {
            Wochentag = w;
            Gericht = g;
        }
    }
}
