using Lezecka_stena_evidence.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lezecka_stena_evidence.NewFolder;

// Třída pro lezce
public class Lezec
{
    public string Jmeno { get; set; }
    public DateTime DatumNarozeni { get; set; }
    public double Vyska { get; set; }
    public Lezec(string jmeno, DateTime datumNarozeni, double vyska)
    {
        Jmeno = jmeno;
        DatumNarozeni = datumNarozeni;
        Vyska = vyska;
    }
    public double VratVek()
    {
        TimeSpan vekSpan = DateTime.Now - DatumNarozeni;
        return vekSpan.TotalDays / 365.25;
    }
}
