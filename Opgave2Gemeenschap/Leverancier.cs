using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemeenschap
{
    public class Leverancier
    {
        private int LevNrValue;
        private String NaamValue;
        private String AdresValue;
        private String PostNrValue;
        private String WoonplaatsValue;
        private System.Object versieValue;

        public bool Changed { get; set; }
        public int LevNr
        {
            get
            { return LevNrValue; }
            set
            { 
                LevNrValue = value;
                
            }
        }
        public String Naam
        {
            get
            { return NaamValue; }
            set
            { 
                NaamValue = value;
                Changed = true;
            }
        }
        public String Adres
        {
            get
            { return AdresValue; }
            set
            { 
                AdresValue = value;
                Changed = true;
            }
        }
        public String PostNr
        {
            get
            { return PostNrValue; }
            set
            {
                PostNrValue = value;
                Changed = true;
            }
        }
        public String Woonplaats
        {
            get
            { return WoonplaatsValue; }
            set
            { 
                WoonplaatsValue = value;
                Changed = true;
            }
        }

        public Object Versie
        {
            get { return versieValue; }
            set { versieValue = value; }
        }

        public Leverancier( Int32 levnr, String naam, String adres, String postnr, String woonpl, Object versie)
        {
            this.LevNr = levnr;
            this.Naam = naam;
            this.Adres = adres;
            this.PostNr = postnr;
            this.Woonplaats = woonpl;
            this.Versie = versie;
            this.Changed = false;
        }

        public Leverancier()
        {

        }
    }
}
