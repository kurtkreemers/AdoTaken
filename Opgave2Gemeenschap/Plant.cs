using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemeenschap
{
    public class Plant
    {
        public bool Changed { get; set; }
        public String PlantNaam { get; set; }
        public int PlantNr { get; set; }
        public int LevNr { get; set; }
        private Decimal prijs;
        public Decimal Prijs
        {
            get { return prijs; }
            set
            {
                prijs = value;
                Changed = true;
            }
        }
        private String kleur;
        public String Kleur
        {
            get { return kleur; }
            set
            {
                kleur = value;
                Changed = true;
            }
        }
        public Plant(String nPlantNaam, int nPlantNr, int nLevNr,
        Decimal nPrijs, String nKleur)
        {
            PlantNaam = nPlantNaam;
            PlantNr = nPlantNr;
            LevNr = nLevNr;
            Prijs = nPrijs;
            Kleur = nKleur;
            Changed = false;
        }
        public Plant()
        {

        }
    }
}
