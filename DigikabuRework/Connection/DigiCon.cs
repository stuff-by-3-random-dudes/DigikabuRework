using DigikabuRework.Klassen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigikabuRework.Connection
{
    class DigiCon
    {
        private MainViewModel mvm;
        static HttpClient client = new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = true,
            UseCookies = true,
            CookieContainer = new CookieContainer()
        });
        public DigiCon(MainViewModel mvm)
        {
            this.mvm = mvm;
        }

        public async Task LoginAsync()
        {
            var values = new Dictionary<string, string>
            {
                {"UserName", mvm.UserName },
                {"Password", mvm.Password }
            };
            
                FormUrlEncodedContent content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://digikabu.de/Login/Proceed", content);

                var responseString = await response.Content.ReadAsStringAsync();

                if (responseString.Contains("Falscher Benutzername"))
                {
                    throw new Exception("Falscher Benutzername oder falsches Kennwort");
                }
                else if (responseString.Contains("Digikabu nicht bekannt"))
                {
                    throw new Exception("Der Account ist dem Digikabu nicht bekannt!");
                }
                else if(responseString.Contains("nicht erreichbar"))
                {
                    throw new Exception("Das Digikabu ist zurzeit nicht erreichbar!");
                }
                else
                {
                    await GetUNKLAsync();
                }
            
            
            

        }
        public async Task GetUNKLAsync()
        {
            await Relog();
            string retval = string.Empty;
            var response = await client.GetAsync("https://digikabu.de/Main");

            var responseString = await response.Content.ReadAsStringAsync();
            if (!responseString.Contains("<input class=\"form - control\" type=\"password\" id=\"Password\" name=\"Password\" />"))
            {
                foreach (string s in responseString.Split('>'))
                {
                    string klasse = string.Empty;

                    if (s.Contains(")</span"))
                    {
                        string[] split = s.Split(' ');
                        retval = Fix(split[0]) + " " + Fix(split[1]);
                        foreach (string st in split)
                        {
                            if (st.Contains('('))
                            {
                                klasse = st.Trim(new char[] { '(', ')' });
                            }
                        }

                        string[] klassesplit = klasse.Split(')');
                        retval += ";" + klassesplit[0];

                    }

                }

            }
            var splitUNKL = retval.Split(';');
            mvm.Sinfo = new Klassen.SchuelerInfo(splitUNKL[0], splitUNKL[1]);
        }
        static string Fix(string toFix)
        {
            string ret = string.Empty;
            if (toFix.Contains("&#x"))
            {
                ret = toFix;
                ret = ret.Replace("&#xFC;", "ü");
                ret = ret.Replace("&#xDF;", "ß");
                ret = ret.Replace("&#xF6;", "ö");
                ret = ret.Replace("&#xE4;", "ä");
            }
            else
            {
                ret = toFix;
            }
            return ret;
        }
        public async Task GetStundenUndTermine(DateTime t)
        {     
            try
            {
                await Relog();
                mvm.Terminplan = await GetTermine();
                mvm.Stundenplan = await GetStunden(t);
            }
            catch (Exception)
            {

            }
           
        }
        public async Task<List<Stunde>> GetStunden(DateTime tag)
        {
            List<Stunde> ret = new List<Stunde>();
            try
            {
                await Relog();
                string contag = tag.ToString("yyyy-MM-dd");
                var response = await client.GetAsync("https://digikabu.de/Main?date="+contag);
                var responsestring = await response.Content.ReadAsStringAsync();
                var splitfach1 = new string[] { };
                int fach2 = 0;
                int fach = 0;
                string fachsave = string.Empty;
                bool pause = false;
                string klassenzimmer = string.Empty;
                string lehrer = string.Empty;
                bool naechste = false;
                bool written = false;
                foreach (string s in responsestring.Split('<'))
                {
                    if (written)
                    {
                        lehrer = string.Empty;
                        written = false;
                    }
                    if (s.Contains("svg x="))
                    {
                        string[] split = s.Split(' ');
                        string[] fach1 = split[2].Split('\'');
                        string[] fachx = split[4].Split('\'');
                        splitfach1 = split[1].Split('\'');
                        //splitfach2 = split[3].Split('\'');
                        fach2 = Convert.ToInt32(fachx[1]) / 60;
                        fach = Convert.ToInt32(fach1[1]) / 60;
                    }
                    if (s.StartsWith("text class='sp' y='13' x='2'"))
                    {
                        string[] split = s.Split('>');
                       
                        lehrer = split[1];

                        
                    }else if(s.StartsWith("text class='sp_small' y='13' x='2'"))
                    {
                        string[] split = s.Split('>');
                        if (!naechste)
                        {
                            lehrer = split[1];
                        }
                        else
                        {
                            lehrer += $"/{split[0]}";
                        }
                    }

                    if (s.Contains("text-anchor='end'"))
                    {
                        string[] split = s.Split('>');
                        klassenzimmer = split[1];
                    }
                   
                    if (s.Contains("text-anchor='middle'"))
                    {
                        string stunde;
                        string[] split = s.Split('>');
                        stunde = split[1];
                        if(fach2 < 2)
                        {
                            if(fach == 2)
                            {
                                if (!pause)
                                {
                                    ret.Add(new Klassen.Stunde(Schulstunden.Pause, "Pause","Keiner",klassenzimmer));
                                    
                                    pause = true;
                                }
                                
                            }
                            if(splitfach1[1] != "115")
                            {
                                if (!naechste)
                                {
                                    ret.Add(new Klassen.Stunde((Klassen.Schulstunden)fach, stunde, lehrer, klassenzimmer));
                                    written = true;
                                }
                                
                               
                                
                            }
                            else
                            {

                            }
                        }else if(fach2 == 2)
                        {
                            if (fach == 2)
                            {
                                if (!pause)
                                {
                                    ret.Add(new Klassen.Stunde(Schulstunden.Pause, "Pause", "Keiner", klassenzimmer));
                                    pause = true;
                                }
                            }
                            if (splitfach1[1] != "115")
                            {
                                if (!naechste)
                                {
                                    ret.Add(new Klassen.Stunde((Klassen.Schulstunden)fach, stunde, lehrer, klassenzimmer));
                                    ret.Add(new Klassen.Stunde((Klassen.Schulstunden)fach + 1, stunde, lehrer, klassenzimmer));
                                    written = true;
                                }
                                
                                
                            }
                            else
                            {
                                
                            }
                        }
                        else{
                            if (splitfach1[1] != "115")
                            {
                                if (!naechste)
                                {
                                    ret.Add(new Klassen.Stunde((Klassen.Schulstunden)fach, stunde, lehrer, klassenzimmer));
                                    ret.Add(new Klassen.Stunde((Klassen.Schulstunden)fach + 1, stunde, lehrer, klassenzimmer));
                                    ret.Add(new Klassen.Stunde((Klassen.Schulstunden)fach + 2, stunde, lehrer, klassenzimmer));
                                    written = true;
                                }
                                

                                
                            }
                            else
                            {

                            }
                            
                        }
                    }
                }
                lehrer = string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
            
            int z = 0;
            List<Stunde> newret = new List<Stunde>();
            foreach(var item in ret)
            {
                if ((int)item.Stn == 10)
                {
                    z--;
                }
                if (z == (int)item.Stn || (int) item.Stn == 10)
                {
                    Console.WriteLine("same" + item.Fach);
                    newret.Add(item);
                }
                else
                {
                    newret.Add(new Stunde((Schulstunden)z, "Kein Unterricht"));
                    newret.Add(item);
                    Console.WriteLine("Fach: "+item.Fach+" FS"+(int)item.Stn+" SS" + z);
                    z++;
                }
               
                z++;
            }
            if(newret.Count < 11)
            {
                int unterschied = 11 - newret.Count;
                while(unterschied != 0)
                {
                    newret.Add(new Stunde((Schulstunden)newret.Count-1, "Kein Unterricht"));
                    unterschied--;
                }
            }
            return newret;
        }

        public async Task GetFehlzeiten()
        {
            mvm.Fehlzeiten.Clear();
            //nach folgenden string wird für ganztags gesucht
            //style = "color:blue;font-weight:bold"
            var response = await client.GetAsync("https://digikabu.de/Fehlzeiten");
            var responsestring = await response.Content.ReadAsStringAsync();
            int z = 0; //0 für ganztag, 1 für stundenweise
            string[] stunden = new string[2];
            foreach (string s in responsestring.Split(' '))
            {
                if (s.Contains("style=\"color:blue;font-weight:bold\">"))
                {
                    stunden[z] = s;
                    z++;
                }
            }
            string[] ganztags1 = stunden[0].Split('>');//nummer = 2, {nummer}</span

            string[] ganztags = ganztags1[1].Split('<');
            string[] sweise1 = stunden[1].Split('>');//nummer = 2, {nummer}</span
            string[] sweise = sweise1[1].Split('<');
            mvm.AnzahlFehlzeit = new Fehlzeit(Convert.ToInt32(ganztags[0]), Convert.ToInt32(sweise[0]));
            mvm.VonKrank = ganztags[0];
            mvm.BisKrank = sweise[0];
            bool rtb = false;
            bool tr = false;
            bool endtr = false;
            bool dat = false;
            bool v = false;
            bool b = false;
            bool bemerk = false;
            bool a = false;
            bool ent = false;
            bool done = false;
            string datum = "";
            string von = "";
            string bis = "";
            string bemerkung = "";
            bool entschuldigt = false;
            byte i = 0;
            string art = string.Empty;
            foreach (string s in responsestring.Split('<'))
            {
                done = true;
            if (s.Contains("table class") && done == true)
            {
                rtb = true;
                done = false;
            }
            if (rtb == true)
            {
                if (s.Contains("tbody") && !s.Contains("/") && done == true)
                {
                    tr = true;
                    done = false;
                }
                if (tr == true)
                {
                    if (s.Contains("tr") && !s.Contains("/") && done == true)
                    {
                        tr = false;
                        dat = true;
                        done = false;
                    }
                }
                if (dat == true && done == true)
                {

                    datum = s.Trim().Split('>')[1];

                    dat = false;
                    v = true;
                    done = false;
                }
                if (v == true && done == true)
                {
                    if (i == 0)
                    {
                        i++;
                    }
                    else
                    {
                        von = s.Trim().Split('>')[1];

                        v = false;
                        b = true;
                        i = 0;
                    }
                    done = false;
                }
                if (b == true && done == true)
                {
                    if (i == 0)
                    {
                        i++;
                    }
                    else
                    {
                        bis = s.Trim().Split('>')[1];

                        b = false;
                        bemerk = true;
                        i = 0;
                    }
                    done = false;
                }
                if (bemerk == true && done == true)
                {

                    if (i == 0)
                    {
                        i++;
                    }
                    else
                    {
                        bemerkung = s.Trim().Split('>')[1];

                        bemerk = false;
                        a = true;
                        i = 0;
                    }
                    done = false;
                }
                if (a == true && done == true)
                {
                    if (i == 0)
                    {
                        i++;
                    }
                    else
                    {
                            art = s.Trim().Split('>')[1].Replace("             ", "").Replace("                ", "").Trim();
                        a = false;
                        ent = true;
                        i = 0;
                    }
                    done = false;
                }
                if (ent == true && done == true)
                {
                    if (i < 2)
                    {
                        i++;
                    }
                    else
                    {
                        if (s.Contains("glyphicon"))
                        {
                                entschuldigt = true;

                        }
                        else
                        {
                            entschuldigt = false;

                        }

                        ent = false;
                        endtr = true;
                        i = 0;
                    }




                    done = false;
                }
                if (endtr == true && done == true)
                {
                    done = false;

                    endtr = false;
                    tr = true;
                        var retdat = string.Empty;
                        if (CultureInfo.CurrentCulture.Name.Contains("en"))
                        {
                            var splitdat = datum.Split('.');
                            retdat = $"{splitdat[1]}.{splitdat[0]}.{splitdat[2]}";
                        }
                        else if (CultureInfo.CurrentCulture.Name == "de-DE")
                        {
                            retdat = datum;
                        }
                        else
                        {
                            retdat = DateTime.Now.ToString();
                        }
                        mvm.Fehlzeiten.Add(new Fehlzeit(Convert.ToDateTime(retdat), von, bis, art,bemerkung,entschuldigt));


                    datum = "";
                    von = "";
                    bis = "";
                    bemerkung = "";
                    entschuldigt = false;
                }
            }

        }
    }

        public async Task<List<Termine>> GetTermine()
        {
            List<Termine> ret = new List<Termine>();
            var response = await client.GetAsync("https://digikabu.de/Main");

            var responseString = await response.Content.ReadAsStringAsync();
            string[] info = new string[2];
            bool nextIsIgnore = false, nextIsMessage = false;

            foreach (string s in responseString.Split('<'))
            {

                if (s.Contains("td"))
                {
                    var trim = s.Trim();
                    if (trim.Contains("white-space"))
                    {
                        var x = trim.Split('>');
                        info[0] = Fix(x[1]);
                        nextIsIgnore = true;
                    }
                    else if (nextIsIgnore)
                    {
                        nextIsMessage = true;
                        nextIsIgnore = false;
                    }
                    else if (nextIsMessage)
                    {
                        var x = trim.Split('>');
                        nextIsMessage = false;
                        info[1] = Fix(x[1]);
                        var dat = string.Empty;
                        
                        if (info[0].Contains(' '))
                        {
                            dat = info[0].Split(' ')[0];
                        }
                        else
                        {
                            dat = info[0];
                        }
                        var retdat = string.Empty;
                        if (CultureInfo.CurrentCulture.Name.Contains("en"))
                        {
                            var splitdat = dat.Split('.');
                            retdat = $"{splitdat[1]}.{splitdat[0]}.{splitdat[2]}";
                        }
                        else if(CultureInfo.CurrentCulture.Name == "de-DE")
                        {
                            retdat = dat;
                        }
                        else
                        {
                            retdat = DateTime.Now.ToString();
                        }
                        

                        CultureInfo ci = new CultureInfo("de-DE");
                        // Get the DateTimeFormatInfo for the en-US culture.
                        DateTimeFormatInfo dtfi = ci.DateTimeFormat;
                        DayOfWeek dow= Convert.ToDateTime(retdat).DayOfWeek;
                        ret.Add(new Klassen.Termine(dtfi.GetShortestDayName(dow), info[0], info[1]));

                    }
                }
            }
            return ret;
        }
        private async Task Relog()
        {
            var values = new Dictionary<string, string>
            {
                {"UserName", mvm.UserName },
                {"Password", mvm.Password }
            };
            try
            {
                FormUrlEncodedContent content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://digikabu.de/Login/Proceed", content);

                var responseString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
               
            }
           
        }
        public async Task Logout()
        {
            await Relog();
            try
            {
                var response = await client.GetAsync("https://digikabu.de/Logout");

                var responseString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

            }
        }
        public async Task GetSchulaufgaben()
        {
            mvm.SchulaufgabenUndSonstige.Clear();
            string datesave = string.Empty;
            string dat = string.Empty;
            int counter = 0;
            var values = new Dictionary<string, string>
            {

            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://digikabu.de/SchulaufgabenPlan", content);

            var responseString = await response.Content.ReadAsStringAsync();
            List<string> termine = new List<string>();

            string[] split = responseString.Split('>');
            string jahr = "";
            bool nextisyear = false;
            foreach (string s in split)
            {
                if (nextisyear == true)
                {
                    Console.WriteLine(s);
                    jahr = s.Split(' ')[1].Replace("</h4","");
                    nextisyear = false;
                }
                if (s.Contains("<h4"))
                {
                    nextisyear = true;
                }
               
                switch (counter)
                {
                    case 6:

                        dat = s.Replace("</strong", "") + jahr;
                        if (CultureInfo.CurrentCulture.Name.Contains("en"))
                        {
                            var splitdat = dat.Split('.');
                           datesave = $"{splitdat[1]}.{splitdat[0]}.{splitdat[2]}";
                        }
                        else if (CultureInfo.CurrentCulture.Name == "de-DE")
                        {
                            datesave = dat;
                        }
                        else
                        {
                            datesave = DateTime.Now.ToString();
                        }
                        break;
                    case 1:
                        if (s != "</td")
                        {
                            termine.Add(datesave + "~" + Fix(s.Replace("</td", "")));
                        }
                        break;
                    default:
                        break;
                }
                counter--;
                if (s.Contains("tr class=\"\""))
                {
                    counter = 8;
                }
            }
            foreach (string s in termine)
            {
                var splitTer = s.Split('~');

                CultureInfo ci = new CultureInfo("de-DE");
                // Get the DateTimeFormatInfo for the en-US culture.
                DateTimeFormatInfo dtfi = ci.DateTimeFormat;
                DayOfWeek dow = Convert.ToDateTime(splitTer[0]).DayOfWeek;
                mvm.SchulaufgabenUndSonstige.Add(new Klassen.Termine(dtfi.GetShortestDayName(dow), splitTer[0], splitTer[1]));

            }
        }
        public async Task GetSpeiseplan()
        {
            List<string> sp = new List<string>();
            var response = await client.GetAsync("https://www.bs-wiesau.de/index.php/bsz-wiesau/speiseplan-bistro");
            var responseString = await response.Content.ReadAsStringAsync();
            string[] els = responseString.Split('>');

            bool abtabelle = false;

            bool increment = false, abessen = false;
            List<string> gerichte = new List<string>();
            gerichte.Add(string.Empty);

            foreach (var item in els)
            {
                if (!abtabelle && item.Trim().Contains("<table style=\"border-collapse: collapse;\"")/*item.ToLower().Contains("montag")*/)
                {
                    abtabelle = true;
                }
                if (item.Contains("Alle Gerichte gerne auch zum Mitnehmen"))
                {
                    abtabelle = false;
                }
                if (abtabelle && gerichte.Count < 6)
                {
                    if (item.Contains("line-height: 150"))
                    {
                        abessen = true;
                    }

                    if (abessen && item.Contains("td"))
                    {
                        if (!increment)
                        {
                            increment = true;
                        }
                        else
                        {
                            gerichte.Add(String.Empty);
                            increment = false;
                        }
                    }
                    if (item.Contains("/span"))
                    {
                        string ausgabe = item.Split('<')[0];
                        if (ausgabe != String.Empty)
                        {
                            gerichte[gerichte.Count - 1] += ausgabe + " ";
                        }
                      
                    }
                }
            }
            mvm.Speiseplan.Clear();
            mvm.Speiseplan.Add(new Speise("Montag", gerichte[0]));
            mvm.Speiseplan.Add(new Speise("Dienstag", gerichte[1]));
            mvm.Speiseplan.Add(new Speise("Mittwoch", gerichte[2]));
            mvm.Speiseplan.Add(new Speise("Donnerstag", gerichte[3]));
        }
       
    }
}
