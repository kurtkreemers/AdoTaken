using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemeenschap
{
    public class PlantenInfo
    {
        private String naamValue;
        private String soortValue;
        private String leverancierValue;
        private String kleurValue;
        private Decimal prijsValue;
        public decimal Prijs
        {
            get { return prijsValue; }
        }
        public string Kleur
        {
            get { return kleurValue; }
        }
        public string Leverancier
        {
            get { return leverancierValue; }
        }
        public string Soort
        {
            get { return soortValue; }
        }
        public string Naam
        {
            get { return naamValue; }
        } 

        public PlantenInfo(String naam, String soort, String leverancier, String kleur, Decimal prijs)
        {
            naamValue = naam;
            soortValue = soort;
            leverancierValue = leverancier;
            kleurValue = kleur;
            prijsValue = prijs;
        }
    }
}
