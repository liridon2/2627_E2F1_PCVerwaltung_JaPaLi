using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public class Mainboard : HardwareKomponente
    {
        public Guid Id { get; }
        public Formfaktor Formfaktor { get; set; } = Formfaktor.ATX;
        public SockelTyp Sockel { get; set; } = SockelTyp.AM5;
        public Chipsatz Chipsatz { get; set; } = Chipsatz.B660;
        public int MaxRAMGB { get; set; } = 0;

        public double Benchmark { get; set; } = 5;

        public Mainboard(string hersteller, string modell, Formfaktor formfaktor, SockelTyp sockel, Chipsatz chipsatz, int maxRAMGB) : base(hersteller, modell)
        {
            //Prüft ob Hersteller und modell nicht leer sind sonst wird eine Fehlermeldung geworfen
            if (string.IsNullOrWhiteSpace(hersteller)) throw new Exception("Board-Hersteller ist Pflicht.");
            if (string.IsNullOrWhiteSpace(modell)) throw new Exception("Board-Modell ist Pflicht.");

            Id = Guid.NewGuid();
            Formfaktor = formfaktor;
            Sockel = sockel;
            Chipsatz = chipsatz;
            MaxRAMGB = maxRAMGB;
            berechneBenchmark();
        }

        public override string ToString() => $"{Hersteller} {Modell} ({Formfaktor}, {Sockel})";

        public override void berechneBenchmark()
        {
            // Punkte direkt im Switch / if-else berechnen
            //setzt die Benchmark scores für die einzelnen Bauteile des Mainboards
            int formfaktorScore = Formfaktor switch
            {
                Formfaktor.MiniITX => 4,
                Formfaktor.MicroATX => 7,
                Formfaktor.ATX => 10,
                _ => 5
            };

            int sockelScore = Sockel switch
            {
                SockelTyp.AM4 => 7,
                SockelTyp.AM5 => 9,
                SockelTyp.LGA1200 => 6,
                SockelTyp.LGA1700 => 9,
                SockelTyp.LGA1151 => 5,
                SockelTyp.TR4 => 8,
                _ => 5
            };

            int chipsatzScore = Chipsatz switch
            {
                Chipsatz.Z690 => 9,
                Chipsatz.Z790 => 10,
                Chipsatz.B550 => 7,
                Chipsatz.B650 => 8,
                Chipsatz.B660 => 7,
                Chipsatz.B760 => 8,
                Chipsatz.H670 => 6,
                Chipsatz.H770 => 7,
                _ => 5
            };

            int ramScore = MaxRAMGB switch
            {
                <= 32 => 4,
                <= 64 => 6,
                <= 128 => 8,
                _ => 10
            };

            // Durchschnitt und runden
            Benchmark = (int)Math.Round((formfaktorScore + sockelScore + chipsatzScore + ramScore) / 4.0);
        }
    }
}
