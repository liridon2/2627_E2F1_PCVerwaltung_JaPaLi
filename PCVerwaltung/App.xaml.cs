using PCVerwaltung.Classes;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static List<Case> Cases { get; } = new()
        {
            new Case("Fractal Design", "Meshify 2",       Formfaktor.ATX),
            new Case("Cooler Master",  "NR400",           Formfaktor.MicroATX),
            new Case("NZXT",           "H1",              Formfaktor.MiniITX),
            new Case("be quiet!",      "Pure Base 500DX", Formfaktor.ATX),
            new Case("Lian Li",        "O11 Dynamic",     Formfaktor.ATX),
        };

        public static List<CPU> CPUs { get; } = new()
        {
            new CPU("AMD", "Intel Core i5-13600K",SockelTyp.LGA1700, 14, 20, 3.5, 5.1),
            new CPU("AMD","AMD Ryzen 5 5600", SockelTyp.AM4, 6, 12, 3.5, 4.4),
            new CPU("Intel", "Intel Core i7-12700F", SockelTyp.LGA1700, 12, 20, 2.1, 4.9),
            new CPU("AMD", "AMD Ryzen 7 5700G", SockelTyp.AM4, 8, 16, 3.8, 4.6),
        };

        public static List<Mainboard> Mainboards { get; } = new()
        {
            new Mainboard("ASUS", "TUF GAMING B650-PLUS", Formfaktor.ATX, SockelTyp.AM5, Chipsatz.B650, 265 ),
            new Mainboard("MSI", "PRO B760M-A", Formfaktor.MicroATX, SockelTyp.LGA1700, Chipsatz.B760, 128 ),
            new Mainboard("Gigabyte", "B550I AORUS PRO AX", Formfaktor.MiniITX, SockelTyp.AM4, Chipsatz.B550, 64 ),
            new Mainboard("ASRock", "B650M Pro RS", Formfaktor.MicroATX, SockelTyp.AM5, Chipsatz.B650, 192 ),
            new Mainboard("ASUS", "ROG Strix Z690-A", Formfaktor.ATX, SockelTyp.LGA1700, Chipsatz.Z690, 192 ),
        };

        public static List<PC> PCs { get; } = new()
        {
            new PC(Cases[0], CPUs[0], Mainboards[0]),
            new PC(Cases[1], CPUs[1], Mainboards[1]),
            new PC(Cases[2], CPUs[2], Mainboards[2]),
            new PC(Cases[3], CPUs[3], Mainboards[4]),
            new PC(Cases[4], CPUs[4], Mainboards[3]),
        };

    }

}
