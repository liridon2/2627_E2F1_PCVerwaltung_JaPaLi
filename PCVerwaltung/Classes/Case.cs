using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public class Case : HardwareKomponente
    {
        public Formfaktor Formfaktor { get; set; } = Formfaktor.ATX;
        public double Benchmark { get; protected set; } = 0;

        public Case(string hersteller, string modell, Formfaktor formfaktor) : base(hersteller,modell)
        {
            //Hersteller und modell müssen gefüllt sein
//            if (string.IsNullOrWhiteSpace(hersteller)) throw new Exception("Herstelelr  ist Pflicht.");
  //          if (string.IsNullOrEmpty(modell)) throw new Exception("Modell ist Pflicht.");

            this.Formfaktor = formfaktor;


            berechneBenchmark();
        }

        public override void berechneBenchmark()
        {

            Benchmark = Formfaktor switch
            {
                Formfaktor.ATX => 10,
                Formfaktor.MicroATX => 7,
                Formfaktor.MiniITX => 4,
                _ => 0
            };

        }
    }
}
