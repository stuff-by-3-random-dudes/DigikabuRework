using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigikabuRework.Klassen
{
    class StundenDauer
    {
        public static List<StundenDauer> StundenDauerListe { get; set; } = new List<StundenDauer>();
        public DateTime Anfang { get; set; }
        public DateTime Ende { get; set; }
        public Schulstunden Schulstunde { get; set; }

        public StundenDauer(DateTime a, DateTime e)
        {
            Anfang = a;
            Ende = e;
        }

        public override string ToString()
        {
            return $"{ToZeit(Anfang)} - {ToZeit(Ende)}";
        }

        public bool AktuelleStunde()
        {
            DateTime jetzt = DateTime.Now;
            TimeSpan nichts = new TimeSpan(0);
            return jetzt.Subtract(Anfang) >= nichts && Ende.Subtract(jetzt) > nichts;
        }

        public static string ToZeit(DateTime dt)
        {
            return dt.ToString("HH:mm");
        }

        public static void SetupStundenDauerListe()
        {
            DateTime start = Convert.ToDateTime("8:30");
            int stundenanz = 10, pausenIndex = 2;
            TimeSpan stundenDauer = new TimeSpan(0, 45, 0);
            TimeSpan pausenDauer = new TimeSpan(0, 15, 0);
            StundenDauerListe.Clear();
            StundenDauer sd;
            DateTime beginn, ende = start;
            for (int i = 0; i < stundenanz + 1; i++)
            {
                beginn = ende;
                if (i == pausenIndex)
                {
                    ende = beginn.Add(pausenDauer);
                    sd = new StundenDauer(beginn, ende);
                    sd.Schulstunde = Schulstunden.Pause;
                }
                else
                {
                    ende = beginn.Add(stundenDauer);
                    var arr = Enum.GetValues(typeof(Schulstunden));
                    sd = new StundenDauer(beginn, ende);
                    if (i > pausenIndex)
                    {
                        
                            sd.Schulstunde = (Schulstunden)arr.GetValue(i-1);
                        
                        
                    }
                    else
                    {
                        sd.Schulstunde = (Schulstunden)arr.GetValue(i);
                    }
                }

                StundenDauerListe.Add(sd);
            }
        }
    }
}
