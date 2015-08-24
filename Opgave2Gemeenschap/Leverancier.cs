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
        public int LevNr
        {
            get
            { return LevNrValue; }
            set
            { LevNrValue = value; }
        }
        public String Naam
        {
            get
            { return NaamValue; }
            set
            { NaamValue = value; }
        }
        public String Adres
        {
            get
            { return AdresValue; }
            set
            { AdresValue = value; }
        }
        public String PostNr
        {
            get
            { return PostNrValue; }
            set
            { PostNrValue = value; }
        }
        public String Woonplaats
        {
            get
            { return WoonplaatsValue; }
            set
            { WoonplaatsValue = value; }
        }

        public Leverancier( Int32 levnr, String naam, String adres, String postnr, String woonpl)
        {
            this.LevNr = levnr;
            this.Naam = naam;
            this.Adres = adres;
            this.PostNr = postnr;
            this.Woonplaats = woonpl;
        }
    }
}
