using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    class Kunde
    {
        public string Kundenname { get; set; } = "";
        public string Zahlungsart { get; set; } = "";
        public string Ranking { get; set; } = "";

        public Kunde(string kundenname, string zahlungsart, string ranking)
        {
            Kundenname = kundenname;
            Zahlungsart = zahlungsart;
            Ranking = ranking;
        }
    }
}
