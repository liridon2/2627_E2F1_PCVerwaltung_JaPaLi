using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public class Kunde
    {
        public string Kundenname { get; set; } = "";
        public Finanzierung Zahlungsart { get; set; }
        public string Ranking { get; set; } = "";

        public Kunde(string kundenname, Finanzierung zahlungsart, string ranking)
        {
            Kundenname = kundenname;
            Zahlungsart = zahlungsart;
            Ranking = ranking;
        }
    }
}
