using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemeenschap
{
    public class Soort
    {
        private Int32 soortNrValue;
        private String soortNaamValue;
        
        public Int32 SoortNr
        {
            get { return soortNrValue; }
            set { soortNrValue = value; }
        }

        public String SoortNaam
        {
            get { return soortNaamValue; }
            set { soortNaamValue = value; }
        }
        public Soort(Int32 soortNr,String soortNaam)
        {
            this.SoortNr = soortNr;
            this.SoortNaam = soortNaam;
        }
    }
}
