using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public enum Formfaktor
    {
        ATX,
        MicroATX,
        MiniITX
    }

    public enum RAMTyp
    {
        DDR4,
        DDR5
    }
    public enum SockelTyp
    {
        AM4,
        AM5,
        AM7,
        LGA1200,
        LGA1700,
        LGA1151,
        TR4,
        sTRX4,
        LGA1851
    }

    public enum FestplattenTyp
    {
        HDD,
        SSD,
        NVMe
    }

    public enum FestplattenSchnittstelle
    {
        SATA,
        PCIe
    }

    public enum Chipsatz
    {
        Z690,
        Z790,
        B550,
        B650,
        B660,
        B760,
        H670,
        H770
    }

    public enum VideoAusgaenge
    {
        HDMI,
        DisplayPort,
        DVI,
        VGA
    }

    public enum NetzteilEffizienz
    {
        _80PlusBronze,
        _80PlusSilber,
        _80PlusGold,
        _80PlusPlatin
    }

    public enum Finanzierung
    {
        Rechnung,
        Bar,
        Ratenzahlung,
        Leasing
    }
}
