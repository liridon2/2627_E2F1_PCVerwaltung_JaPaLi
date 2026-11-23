using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public class Kunde
    {
        private Ranking ranking;
        private Zahlungsart zahlungsart;

        public string Kundenname { get; set; } = "";

        public Zahlungsart Zahlungsart
        {
            get => zahlungsart;
            set
            {
                if (!IstErlaubt(ranking, value))
                    throw new InvalidOperationException(Begruendung(ranking, value));

                zahlungsart = value;
            }
        }

        public Ranking Ranking
        {
            get => ranking;
            set
            {
                // sonst käme man über ein Downgrade an der Regel vorbei
                if (!IstErlaubt(value, zahlungsart))
                    throw new InvalidOperationException(Begruendung(value, zahlungsart));

                ranking = value;
            }
        }

        public Kunde(string kundenname, Zahlungsart zahlungsart, Ranking ranking)
        {
            if (string.IsNullOrWhiteSpace(kundenname))
                throw new ArgumentException("Kundenname ist Pflicht.", nameof(kundenname));

            if (!IstErlaubt(ranking, zahlungsart))
                throw new InvalidOperationException(Begruendung(ranking, zahlungsart));

            Kundenname = kundenname.Trim();

            // direkt auf die Felder, sonst blockieren sich die beiden Setter
            this.ranking = ranking;
            this.zahlungsart = zahlungsart;
        }

        // bar geht immer, finanzieren erst ab STANDARD
        public static bool IstErlaubt(Ranking ranking, Zahlungsart zahlungsart)
            => zahlungsart == Zahlungsart.Bar || ranking != Ranking.LOW;

        public static string Begruendung(Ranking ranking, Zahlungsart zahlungsart)
        {
            if (IstErlaubt(ranking, zahlungsart))
                return "";

            return $"Kunden mit dem Ranking {Ranking.LOW} können ausschließlich bar zahlen.\n"
                 + $"Für eine {Zahlungsart.Finanzierung} muss das Ranking mindestens "
                 + $"{Ranking.STANDARD} sein.";
        }

        public static IEnumerable<Zahlungsart> ErlaubteZahlungsarten(Ranking ranking)
            => Enum.GetValues(typeof(Zahlungsart))
                   .Cast<Zahlungsart>()
                   .Where(z => IstErlaubt(ranking, z));

        public override string ToString() => $"{Kundenname} ({Ranking})";
    }
}
