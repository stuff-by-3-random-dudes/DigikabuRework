using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigikabuRework.Klassen
{
    class Fehlzeit : BaseModel
    {
        private int ganztags;
        private int stundenweiße;
        public int Ganztags
        {
            get
            {
                return ganztags;
            }
            set
            {
                ganztags = value;
                
            }
        }
        public int Stundenweiße
        {
            get
            {
                return stundenweiße;
            }
            set
            {
                stundenweiße = value;
               
            }
        }
        private DateTime datum;

        public DateTime Datum
        {
            get { return datum; }
            set { datum = value; }
        }

        private string von;

        public string Von
        {
            get { return von; }
            set { von = value; }
        }
        private string bis;

        public string Bis
        {
            get { return bis; }
            set { bis = value; }
        }

        private string art;

        public string Art
        {
            get { return art; }
            set { art = value; }
        }

        private string beschreibung;

        public string Beschreibung
        {
            get { return beschreibung; }
            set { beschreibung = value; }
        }

        private bool entschuldigt;

        public bool Entschuldigt
        {
            get { return entschuldigt; }
            set { entschuldigt = value; }
        }

        public Fehlzeit(int ganztag, int stundenweise)
        {
            Ganztags = ganztag;
            Stundenweiße = stundenweise;

        }
        public Fehlzeit(DateTime datum, string von, string bis, string art, string beschreibung, bool entschuldigt)
        {
            Datum = datum;
            Von = von;
            Bis = bis;
            Art = art;
            Beschreibung = beschreibung;
            Entschuldigt = entschuldigt;
        }

    }
}
