using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigikabuRework.Klassen
{
    class SchuelerInfo
    {
        public string Schueler { get; set; } = string.Empty;
        public string Klasse { get; set; } = string.Empty;

        public SchuelerInfo()
        {
            
        }
        public SchuelerInfo(string schueler, string klasse)
        {
            Schueler = schueler;
            Klasse = klasse;
        }
    }
}
