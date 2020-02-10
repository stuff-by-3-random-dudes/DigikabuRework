using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigikabuRework;

namespace DigikabuRework.Klassen
{
    class WochenStundenPlan : BaseModel
    {
        
        #region Montag
        private List<Stunde> montag = new List<Stunde>();

        public List<Stunde> Montag
        {
            get { return montag; }
            set { montag = value;
                  OnPropertyChanged();
            }
        }
        #endregion
        #region Dienstag
        private List<Stunde> dienstag = new List<Stunde>();
        public List<Stunde> Dienstag
        {
            get { return dienstag; }
            set
            {
                dienstag = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Mittwoch
        private List<Stunde> mittwoch = new List<Stunde>();

        public List<Stunde> Mittwoch
        {
            get { return mittwoch; }
            set
            {
                mittwoch = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Donnerstag
        private List<Stunde> donnerstag = new List<Stunde>();

        public List<Stunde> Donnerstag
        {
            get { return donnerstag; }
            set
            {
                donnerstag = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Freitag

        private List<Stunde> freitag = new List<Stunde>();

        public List<Stunde> Freitag
        {
            get { return freitag; }
            set
            {
                freitag = value;
                OnPropertyChanged();
            }
        }
        #endregion
        private DateTime start;

        public DateTime Start
        {
            get { return start; }
            set { start = value;
                OnPropertyChanged();
            }
        }
        private DateTime end;

        public DateTime End
        {
            get { return end; }
            set { end = value;
                OnPropertyChanged();
            }
        }


        public WochenStundenPlan()
        {

        }
    }
}
