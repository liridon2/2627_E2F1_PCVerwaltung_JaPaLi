using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung
{
    public abstract class HardwareKomponente
    {
        public string Hersteller { get; set; }
        public String Modell { get; set; }

        private double benchmarkScore = 0;

        public HardwareKomponente(string hersteller, String modell)
        {
            Hersteller = hersteller;
            Modell = modell;
        }

        public abstract void berechneBenchmark();
    }
}
