using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public class Adresse
    {
        public string Strasse { get; set; } = "";
        public string Plz { get; set; } = "";
        public string Ort { get; set; } = "";

        public Adresse(string strasse, string plz, string ort)
        {
            if (string.IsNullOrWhiteSpace(strasse)) throw new Exception("Straße ist Pflicht.");
            if (string.IsNullOrWhiteSpace(plz)) throw new Exception("PLZ ist Pflicht.");
            if (string.IsNullOrWhiteSpace(ort)) throw new Exception("Ort ist Pflicht.");

            Strasse = strasse.Trim();
            Plz = plz.Trim();
            Ort = ort.Trim();
        }

        public override string ToString() => $"{Strasse}, {Plz} {Ort}";
    }
}
