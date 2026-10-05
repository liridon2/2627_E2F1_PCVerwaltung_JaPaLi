using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace PCVerwaltung.Classes
{
    public class CPU : HardwareKomponente
    {
        public SockelTyp SockelTyp { get; set; } = SockelTyp.AM5;
        public int Kerne { get; set; } = 0;
        public int Threads { get; set; } = 0;
        public double TaktGHz { get; set; } = 0.0;
        public double MaxTaktGHz { get; set; } = 0.0;
        public double Benchmark { get; private set; } = 5;

        public CPU(string hersteller, string modell, SockelTyp sockeltyp, int kerne, int threads, double taktGHz, double maxTaktGHz) : base(hersteller, modell)
        {
            SockelTyp = sockeltyp;
            Kerne = kerne;
            Threads = threads;
            TaktGHz = taktGHz;
            MaxTaktGHz = maxTaktGHz;
            berechneBenchmark();
        }

        public override void berechneBenchmark()
        {
            //double kerneScore = BerechneKerneScore();

            const int maxKerne = 32;

            double score = (double)Kerne / maxKerne * 5;
            double kerneScore = Math.Min(score, 5.0);

            double sockelScore = SockelTyp switch
            {
                // Ältere Sockel (1.0-2.0 Punkte)
                SockelTyp.LGA1151 => 1.0,
                SockelTyp.AM4 => 1.5,
                SockelTyp.sTRX4 => 2.0,

                // Mittel alte Sockel (2.5-3.5 Punkte)
                SockelTyp.LGA1200 => 2.5,
                SockelTyp.LGA1700 => 3.5,

                // Neueste Sockel (4.5-5.0 Punkte)
                SockelTyp.AM5 => 5.0,
                SockelTyp.LGA1851 => 4.5,

                _ => 0
            };
            
            Benchmark = Math.Round(kerneScore + sockelScore, 1);
        }
    }
}