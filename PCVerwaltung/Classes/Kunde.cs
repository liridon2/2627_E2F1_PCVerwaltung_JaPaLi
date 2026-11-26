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
        private string eMail = "";

        public string Kundenname { get; set; } = "";
        public string Telefon { get; set; } = "";
        public Adresse Adresse { get; set; }

        public string EMail
        {
            get => eMail;
            set
            {
                if (!IstGueltigeEMail(value)) throw new Exception("Bitte eine gültige E-Mail-Adresse angeben.");

                eMail = value.Trim();
            }
        }

        public Zahlungsart Zahlungsart
        {
            get => zahlungsart;
            set
            {
                if (!IstErlaubt(ranking, value)) throw new Exception(Begruendung(ranking, value));

                zahlungsart = value;
            }
        }

        public Ranking Ranking
        {
            get => ranking;
            set
            {
                // sonst käme man über ein Downgrade an der Regel vorbei
                if (!IstErlaubt(value, zahlungsart)) throw new Exception(Begruendung(value, zahlungsart));

                ranking = value;
            }
        }

        public Kunde(string kundenname, Adresse adresse, string eMail, string telefon, Zahlungsart zahlungsart, Ranking ranking)
        {
            if (string.IsNullOrWhiteSpace(kundenname)) throw new Exception("Kundenname ist Pflicht.");
            if (adresse == null) throw new Exception("Adresse ist Pflicht.");
            if (!IstGueltigeEMail(eMail)) throw new Exception("Bitte eine gültige E-Mail-Adresse angeben.");
            if (!IstErlaubt(ranking, zahlungsart)) throw new Exception(Begruendung(ranking, zahlungsart));

            Kundenname = kundenname.Trim();
            Adresse = adresse;
            Telefon = telefon.Trim();

            // direkt auf die Felder, sonst blockieren sich die beiden Setter
            this.eMail = eMail.Trim();
            this.ranking = ranking;
            this.zahlungsart = zahlungsart;
        }

        // Ranking und Zahlungsart zusammen setzen, einzeln blockieren sich die Setter
        public void SetzeKonditionen(Zahlungsart zahlungsart, Ranking ranking)
        {
            if (!IstErlaubt(ranking, zahlungsart)) throw new Exception(Begruendung(ranking, zahlungsart));

            this.zahlungsart = zahlungsart;
            this.ranking = ranking;
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

        public static bool IstGueltigeEMail(string eMail)
        {
            if (string.IsNullOrWhiteSpace(eMail))
                return false;

            eMail = eMail.Trim();

            int at = eMail.IndexOf('@');
            int punkt = eMail.LastIndexOf('.');

            return at > 0
                && at == eMail.LastIndexOf('@')
                && punkt > at + 1
                && punkt < eMail.Length - 1
                && !eMail.Contains(' ');
        }

        public override string ToString() => $"{Kundenname} ({Ranking})";
    }
}
